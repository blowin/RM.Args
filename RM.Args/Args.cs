using RM.Args.ArgumentMarshalers;

namespace RM.Args;

public class Args
{
    private string _schema;

    private Dictionary<char, ArgumentMarshaler> _marshalers = new Dictionary<char, ArgumentMarshaler>();

    private HashSet<char> _argsFound = new HashSet<char>();
    private IEnumerator<string> _currentArgument;
    private List<string> _argsList;

    public Args(string schema, string[] args)
    {
        _schema = schema;
        _argsList = new List<string>(args);
        Parse();
    }

    private void Parse()
    {
        ParseSchema();
        ParseArguments();
    }

    private bool ParseSchema()
    {
        foreach (string element in _schema.Split(","))
        {
            if (element.Length > 0)
            {
                ParseSchemaElement(element.Trim());
            }
        }

        return true;
    }

    private void ParseSchemaElement(string element)
    {
        char elementId = element[0];
        string elementTail = element.Substring(1);
        ValidateSchemaElementId(elementId);
        if (elementTail.Length == 0)
            _marshalers.Add(elementId, new BooleanArgumentMarshaler());
        else if (elementTail.Equals("*"))
            _marshalers.Add(elementId, new StringArgumentMarshaler());
        else if (elementTail.Equals("#"))
            _marshalers.Add(elementId, new IntegerArgumentMarshaler());
        else if (elementTail.Equals("##"))
            _marshalers.Add(elementId, new DoubleArgumentMarshaler());
        else
            throw new ArgsException(ArgsException.ErrorCode.InvalidFormat, elementId, elementTail);
    }

    private void ValidateSchemaElementId(char elementId)
    {
        if (!char.IsLetter(elementId))
        {
            throw new ArgsException(ArgsException.ErrorCode.InvalidArgumentName,
                elementId, null);
        }
    }

    private void ParseArguments()
    {
        for (_currentArgument = _argsList.GetEnumerator(); _currentArgument.MoveNext();)
        {
            string arg = _currentArgument.Current;
            ParseArgument(arg);
        }
    }

    private void ParseArgument(string arg)
    {
        if (arg.StartsWith("-"))
            ParseElements(arg);
    }

    private void ParseElements(string arg)
    {
        for (int i = 1; i < arg.Length; i++)
            ParseElement(arg[i]);
    }

    private void ParseElement(char argChar)
    {
        if (SetArgument(argChar))
            _argsFound.Add(argChar);
        else
        {
            throw new ArgsException(ArgsException.ErrorCode.UnexpectedArgument,
                argChar, null);
        }
    }

    private bool SetArgument(char argChar)
    {
        ArgumentMarshaler m = _marshalers.TryGetValue(argChar, out var r) ? r : null;
        if (m == null)
            return false;
        try
        {
            m.Set(_currentArgument);
            return true;
        }
        catch (ArgsException e)
        {
            e.SetErrorArgumentId(argChar);
            throw;
        }
    }

    public int Cardinality()
    {
        return _argsFound.Count;
    }

    public string Usage()
    {
        if (_schema.Length > 0)
            return "-[" + _schema + "]";
        else
            return "";
    }

    public bool GetBoolean(char arg)
    {
        ArgumentMarshaler am = _marshalers.TryGetValue(arg, out var r) ? r : null;
        bool b = false;
        try
        {
            b = am != null && (bool)am.Get();
        }
        catch (InvalidCastException e)
        {
            b = false;
        }

        return b;
    }

    public string GetString(char arg)
    {
        ArgumentMarshaler am = _marshalers.TryGetValue(arg, out var r) ? r : null;
        try
        {
            return am == null ? "" : (string)am.Get();
        }
        catch (InvalidCastException e)
        {
            return "";
        }
    }

    public int GetInt(char arg)
    {
        ArgumentMarshaler am = _marshalers.TryGetValue(arg, out var r) ? r : null;
        try
        {
            return am == null ? 0 : (int)am.Get();
        }
        catch (Exception e)
        {
            return 0;
        }
    }

    public double GetDouble(char arg)
    {
        ArgumentMarshaler am = _marshalers.TryGetValue(arg, out var r) ? r : null;
        try
        {
            return am == null ? 0 : (double)am.Get();
        }
        catch (Exception e)
        {
            return 0.0;
        }
    }

    public bool Has(char arg)
    {
        return _argsFound.Contains(arg);
    }
}
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
        ParseSchema();
        ParseArguments();
    }

    private void ParseSchema()
    {
        foreach (string element in _schema.Split(","))
        {
            if (string.IsNullOrWhiteSpace(element))
                continue;

            var trimmedElement = element.Trim();
            char elementId = trimmedElement[0];
            if (!char.IsLetter(elementId))
                throw new ArgsException(ArgsException.ErrorCode.InvalidArgumentName, elementId, null);

            string elementTail = trimmedElement.Substring(1);
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
    }

    private void ParseArguments()
    {
        for (_currentArgument = _argsList.GetEnumerator(); _currentArgument.MoveNext();)
        {
            string arg = _currentArgument.Current;
            if (!arg.StartsWith("-"))
                continue;

            foreach (var argChar in arg.Skip(1))
            {
                if (!SetArgument(argChar))
                    throw new ArgsException(ArgsException.ErrorCode.UnexpectedArgument, argChar, null);

                _argsFound.Add(argChar);
            }
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

    public T Get<T>(char arg, T defaultValue = default)
    {
        var am = _marshalers.TryGetValue(arg, out var r) ? r : null;
        if (am == null)
            return defaultValue;

        var value = am.Get();
        return value is T castValue ? castValue : defaultValue;
    }

    public bool Has(char arg)
    {
        return _argsFound.Contains(arg);
    }
}
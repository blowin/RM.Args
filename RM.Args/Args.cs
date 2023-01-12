using RM.Args.ArgumentMarshalers;

namespace RM.Args;

public class Args
{
    private readonly string _schema;
    private readonly Dictionary<char, ArgumentMarshaler> _marshalers = new Dictionary<char, ArgumentMarshaler>();

    public Args(string schema, string[] args)
    {
        _schema = schema;
        ParseSchema();
        ParseArguments(args);
    }

    private void ParseSchema()
    {
        foreach (var element in _schema.Split(","))
        {
            if (string.IsNullOrWhiteSpace(element))
                continue;

            var trimmedElement = element.Trim();
            var elementId = trimmedElement[0];
            if (!char.IsLetter(elementId))
                throw new ArgsException(ArgsException.ErrorCode.InvalidArgumentName, elementId, null);

            var elementTail = trimmedElement.Substring(1);
            if (elementTail.Length == 0)
                _marshalers.Add(elementId, new BooleanArgumentMarshaler(elementId));
            else if (elementTail.Equals("*"))
                _marshalers.Add(elementId, new StringArgumentMarshaler(elementId));
            else if (elementTail.Equals("#"))
                _marshalers.Add(elementId, new IntegerArgumentMarshaler(elementId));
            else if (elementTail.Equals("##"))
                _marshalers.Add(elementId, new DoubleArgumentMarshaler(elementId));
            else
                throw new ArgsException(ArgsException.ErrorCode.InvalidFormat, elementId, elementTail);
        }
    }

    private void ParseArguments(string[] args)
    {
        var argsList = new List<string>(args);
        for (var currentArgument = argsList.GetEnumerator(); currentArgument.MoveNext();)
        {
            var arg = currentArgument.Current;
            if (!arg.StartsWith("-"))
                continue;

            foreach (var argChar in arg.Skip(1))
            {
                if (!SetArgument(argChar, currentArgument))
                    throw new ArgsException(ArgsException.ErrorCode.UnexpectedArgument, argChar, null);
            }
        }
    }

    private bool SetArgument(char argChar, IEnumerator<string> currentArgument)
    {
        if (!_marshalers.TryGetValue(argChar, out var m))
            return false;
        try
        {
            m.Set(currentArgument);
            return true;
        }
        catch (ArgsException e)
        {
            e.SetErrorArgumentId(argChar);
            throw;
        }
    }

    public int Cardinality() => _marshalers.Values.Count(e => e.HasValue);

    public string Usage()
    {
        return _schema.Length > 0 ? 
            "-[" + _schema + "]" : 
            "";
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
        return _marshalers.TryGetValue(arg, out var marshaler) && marshaler.HasValue;
    }
}
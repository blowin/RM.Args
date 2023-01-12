using RM.Args.ArgumentMarshalers;

namespace RM.Args;

public class Args
{
    private readonly string _schema;
    private readonly Dictionary<char, ArgumentMarshaler> _marshalers;

    public Args(string schema, string[] args)
    {
        _schema = schema;
        _marshalers = new Dictionary<char, ArgumentMarshaler>();
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
                throw new InvalidArgumentNameException(elementId);

            var elementTail = trimmedElement.Substring(1);
            var marshaler = CreateMarshaler(elementId, elementTail);
            _marshalers.Add(elementId, marshaler);
        }
    }

    private ArgumentMarshaler CreateMarshaler(char elementId, string elementTail)
    {
        if (elementTail.Length == 0)
            return new BooleanArgumentMarshaler(elementId);
        return elementTail switch
        {
            "*" => new StringArgumentMarshaler(elementId),
            "#" => new IntegerArgumentMarshaler(elementId),
            "[#]" => new IntegerArrayArgumentMarshaler(elementId),
            "##" => new DoubleArgumentMarshaler(elementId),
            _ => throw new InvalidArgsFormatException(elementId, elementTail)
        };
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
                if (!_marshalers.TryGetValue(argChar, out var m))
                    throw new UnexpectedArgumentException(argChar);
                m.Set(currentArgument);
            }
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

    public bool Has(char arg) => _marshalers.TryGetValue(arg, out var marshaler) && marshaler.HasValue;
}
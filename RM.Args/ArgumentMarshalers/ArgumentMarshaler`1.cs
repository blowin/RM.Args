namespace RM.Args.ArgumentMarshalers;

public abstract class ArgumentMarshaler<T> : ArgumentMarshaler
{
    private T _value;
    private char _elementId;
    private bool _hasValue;

    public override bool HasValue => _hasValue;

    public ArgumentMarshaler(char elementId)
    {
        _elementId = elementId;
    }

    public override void Set(IEnumerator<string> currentArgument)
    {
        if (!currentArgument.MoveNext())
            throw new MissingArgsException(typeof(T), _elementId);
        
        string parameter = currentArgument.Current;
        try
        {
            _value = Parse(parameter);
            _hasValue = true;
        }
        catch (FormatException)
        {
            throw new InvalidArgsException(typeof(T), _elementId, parameter);
        }
    }

    public override object Get() => _value;

    protected abstract T Parse(string value);
}
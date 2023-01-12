namespace RM.Args.ArgumentMarshalers;

public class IntegerArgumentMarshaler : ArgumentMarshaler
{
    private int _intValue = 0;

    public override void Set(IEnumerator<string> currentArgument)
    {
        if (!currentArgument.MoveNext())
            throw new ArgsException(ArgsException.ErrorCode.MissingInteger);

        string parameter = null;
        try
        {
            parameter = currentArgument.Current;
            _intValue = int.Parse(parameter);
        }
        catch (FormatException e)
        {
            throw new ArgsException(ArgsException.ErrorCode.InvalidInteger, parameter);
        }
    }

    public override object Get()
    {
        return _intValue;
    }
}
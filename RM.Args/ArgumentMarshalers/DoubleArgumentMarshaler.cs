namespace RM.Args.ArgumentMarshalers;

public class DoubleArgumentMarshaler : ArgumentMarshaler
{
    private double _doubleValue = 0;

    protected override void SetCore(IEnumerator<string> currentArgument)
    {
        if (!currentArgument.MoveNext())
            throw new ArgsException(ArgsException.ErrorCode.MissingDouble);

        string parameter = null;
        try
        {
            parameter = currentArgument.Current;
            _doubleValue = double.Parse(parameter);
        }
        catch (FormatException e)
        {
            throw new ArgsException(ArgsException.ErrorCode.InvalidDouble, parameter);
        }
    }

    public override object Get()
    {
        return _doubleValue;
    }
}
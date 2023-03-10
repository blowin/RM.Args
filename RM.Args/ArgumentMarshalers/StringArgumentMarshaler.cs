namespace RM.Args.ArgumentMarshalers;

public class StringArgumentMarshaler : ArgumentMarshaler
{
    private string _stringValue = "";

    public override void Set(IEnumerator<string> currentArgument)
    {
        if (!currentArgument.MoveNext())
            throw new ArgsException(ArgsException.ErrorCode.MissingString);
        _stringValue = currentArgument.Current;
    }

    public override object Get()
    {
        return _stringValue;
    }
}
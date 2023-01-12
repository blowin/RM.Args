namespace RM.Args.ArgumentMarshalers;

public class BooleanArgumentMarshaler : ArgumentMarshaler
{
    private bool _booleanValue = false;

    public override void Set(IEnumerator<string> currentArgument)
    {
        _booleanValue = true;
    }

    public override object Get()
    {
        return _booleanValue;
    }
}
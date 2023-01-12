namespace RM.Args.ArgumentMarshalers;

public abstract class ArgumentMarshaler
{
    public abstract void Set(IEnumerator<string> currentArgument);
    public abstract object Get();
}
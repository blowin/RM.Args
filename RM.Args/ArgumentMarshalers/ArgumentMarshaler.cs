namespace RM.Args.ArgumentMarshalers;

public abstract class ArgumentMarshaler
{
    public abstract bool HasValue { get; }

    public abstract void Set(IEnumerator<string> currentArgument);
    
    public abstract object Get();
}

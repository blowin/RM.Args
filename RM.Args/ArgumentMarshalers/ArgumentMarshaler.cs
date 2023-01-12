namespace RM.Args.ArgumentMarshalers;

public abstract class ArgumentMarshaler
{
    public bool HasValue { get; private set; }

    public void Set(IEnumerator<string> currentArgument)
    {
        SetCore(currentArgument);
        HasValue = true;
    }
    
    public abstract object Get();

    protected abstract void SetCore(IEnumerator<string> currentArgument);
}
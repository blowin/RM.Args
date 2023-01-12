namespace RM.Args.ArgumentMarshalers;

public class IntegerArgumentMarshaler : ArgumentMarshaler<int>
{
    public IntegerArgumentMarshaler(char elementId) : base(elementId)
    {
    }

    protected override int Parse(string value) => int.Parse(value);
}
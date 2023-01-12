namespace RM.Args.ArgumentMarshalers;

public class BooleanArgumentMarshaler : ArgumentMarshaler<bool>
{
    public BooleanArgumentMarshaler(char elementId) : base(elementId)
    {
    }

    protected override bool Parse(string value) => bool.Parse(value);
}
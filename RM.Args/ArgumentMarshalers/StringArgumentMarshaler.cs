namespace RM.Args.ArgumentMarshalers;

public class StringArgumentMarshaler : ArgumentMarshaler<string>
{
    public StringArgumentMarshaler(char elementId) : base(elementId)
    {
    }

    protected override string Parse(string value) => value;
}
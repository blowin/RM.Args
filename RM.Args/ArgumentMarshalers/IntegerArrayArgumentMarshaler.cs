namespace RM.Args.ArgumentMarshalers;

public class IntegerArrayArgumentMarshaler : ArgumentMarshaler<int[]>
{
    public IntegerArrayArgumentMarshaler(char elementId) : base(elementId)
    {
    }

    protected override int[] Parse(string value) => value.Split(',').Select(int.Parse).ToArray();
}
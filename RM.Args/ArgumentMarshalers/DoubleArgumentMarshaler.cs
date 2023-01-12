namespace RM.Args.ArgumentMarshalers;

public class DoubleArgumentMarshaler : ArgumentMarshaler<double>
{
    public DoubleArgumentMarshaler(char elementId) : base(elementId)
    {
    }

    protected override double Parse(string value) => double.Parse(value);
}
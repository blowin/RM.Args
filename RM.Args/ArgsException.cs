namespace RM.Args;

public class InvalidArgsException : ArgsException
{
    private const string FallbackErrorParameter = "TILT";
    
    public string ErrorParameter { get; }
    
    public InvalidArgsException(Type type, char elementId, string parameter) 
        : base(string.Format("Argument -{0} expects a {1} but was '{2}'.", elementId, type.Name, parameter ?? FallbackErrorParameter), elementId)
    {
        ErrorParameter = parameter ?? FallbackErrorParameter;
    }
}

public class MissingArgsException : ArgsException
{
    public MissingArgsException(Type type, char elementId) 
        : base(string.Format("Could not find {0} parameter for -{1}.", type.Name, elementId), elementId)
    {
        
    }
}

public class UnexpectedArgumentException : ArgsException
{
    public UnexpectedArgumentException(char elementId)
        : base(string.Format("Argument -{0} unexpected.", elementId), elementId)
    {
        
    }
}

public class InvalidArgumentNameException : ArgsException
{
    public InvalidArgumentNameException(char elementId)
        : base(string.Format("Invalid argument -{0} name.", elementId), elementId)
    {
    }
}

public class InvalidArgsFormatException : ArgsException
{
    public InvalidArgsFormatException(char elementId, string tail)
        : base(string.Format("Invalid argument format -{0}{1}.", elementId, tail), elementId)
    {
    }
}

public abstract class ArgsException : Exception
{
    public char ErrorArgumentId { get; private set; }
    
    public ArgsException(string message, char errorArgumentId) : base(message)
    {
        ErrorArgumentId = errorArgumentId;
    }
}
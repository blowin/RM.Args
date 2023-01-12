namespace RM.Args;

public class InvalidArgsException : ArgsException
{
    public InvalidArgsException(Type type, char elementId, string parameter) 
        : base(string.Format("Argument -{0} expects a {1} but was '{2}'.", elementId, type.Name, parameter), elementId, parameter)
    {
        
    }
}

public class MissingArgsException : ArgsException
{
    public MissingArgsException(Type type, char elementId) 
        : base(string.Format("Could not find {0} parameter for -{1}.", type.Name, elementId), elementId, null)
    {
        
    }
}

public class ArgsException : Exception
{
    protected char _errorArgumentId = '\0';
    private string _errorParameter = "TILT";
    private ErrorCode _errorCode = ErrorCode.Ok;

    public ArgsException()
    {
    }

    public ArgsException(string message, char errorArgumentId, string errorParameter) : base(message)
    {
        _errorArgumentId = errorArgumentId;
        _errorParameter = errorParameter;
    }

    public ArgsException(ErrorCode errorCode)
    {
        _errorCode = errorCode;
    }

    public ArgsException(ErrorCode errorCode, string errorParameter)
    {
        _errorCode = errorCode;
        _errorParameter = errorParameter;
    }

    public ArgsException(ErrorCode errorCode, char errorArgumentId,
        string errorParameter)
    {
        _errorCode = errorCode;
        _errorParameter = errorParameter;
        _errorArgumentId = errorArgumentId;
    }

    public char GetErrorArgumentId()
    {
        return _errorArgumentId;
    }

    public void SetErrorArgumentId(char errorArgumentId)
    {
        _errorArgumentId = errorArgumentId;
    }

    public string GetErrorParameter()
    {
        return _errorParameter;
    }

    public void SetErrorParameter(string errorParameter)
    {
        _errorParameter = errorParameter;
    }

    public ErrorCode GetErrorCode()
    {
        return _errorCode;
    }

    public void SetErrorCode(ErrorCode errorCode)
    {
        _errorCode = errorCode;
    }

    public virtual string ErrorMessage()
    {
        switch (_errorCode)
        {
            case ErrorCode.Ok:
                throw new Exception("TILT: Should not get here.");
            case ErrorCode.UnexpectedArgument:
                return string.Format("Argument -{0} unexpected.", _errorArgumentId);
        }

        return "";
    }

    public enum ErrorCode
    {
        Ok,
        InvalidFormat,
        UnexpectedArgument,
        InvalidArgumentName,
    }
}
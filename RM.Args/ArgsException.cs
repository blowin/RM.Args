namespace RM.Args;

public class ArgsException : Exception
{
    private char _errorArgumentId = '\0';
    private string _errorParameter = "TILT";
    private ErrorCode _errorCode = ErrorCode.Ok;

    public ArgsException()
    {
    }

    public ArgsException(string message) : base(message)
    {
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

    public string ErrorMessage()
    {
        switch (_errorCode)
        {
            case ErrorCode.Ok:
                throw new Exception("TILT: Should not get here.");
            case ErrorCode.UnexpectedArgument:
                return string.Format("Argument -{0} unexpected.", _errorArgumentId);
            case ErrorCode.MissingString:
                return string.Format("Could not find string parameter for -{0}.",
                    _errorArgumentId);
            case ErrorCode.InvalidInteger:
                return string.Format("Argument -{0} expects an integer but was '{1}'.",
                    _errorArgumentId, _errorParameter);
            case ErrorCode.MissingInteger:
                return string.Format("Could not find integer parameter for -{0}.",
                    _errorArgumentId);
            case ErrorCode.InvalidDouble:
                return string.Format("Argument -{0} expects a double but was '{1}'.",
                    _errorArgumentId, _errorParameter);
            case ErrorCode.MissingDouble:
                return string.Format("Could not find double parameter for -{0}.",
                    _errorArgumentId);
        }

        return "";
    }

    public enum ErrorCode
    {
        Ok,
        InvalidFormat,
        UnexpectedArgument,
        InvalidArgumentName,
        MissingString,
        MissingInteger,
        InvalidInteger,
        MissingDouble,
        InvalidDouble
    }
}
namespace RM.Args;

internal class Program
{
    public static void Main(string[] args)
    {
        Args arg = new Args("l,p#,d*", args);
        var logging = arg.GetBoolean('l');
        int port = arg.GetInt('p');
        string directory = arg.GetString('d');
    }
}
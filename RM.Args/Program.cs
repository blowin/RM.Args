namespace RM.Args;

internal class Program
{
    public static void Main(string[] args)
    {
        Args arg = new Args("l,p#,d*", args);
        var logging = arg.Get<bool>('l');
        int port = arg.Get<int>('p');
        string directory = arg.Get<string>('d');
    }
}
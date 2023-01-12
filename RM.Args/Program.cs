namespace RM.Args;

internal class Program
{
    public static void Main(string[] args)
    {
        var arg = new Args("l,p#,d*", args);
        var logging = arg.Get<bool>('l');
        var port = arg.Get<int>('p');
        var directory = arg.Get<string>('d');
    }
}
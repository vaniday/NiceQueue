using NUnitLite;

public class Program
{
    public static void Main(string[] args)
    {
#if DNX451
        new AutoRun().Execute(args);
#else
        new AutoRun().Execute(typeof(Program).GetTypeInfo().Assembly, Console.Out, Console.In, args);
#endif
    }
}

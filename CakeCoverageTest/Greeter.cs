namespace CakeCoverageTest
{
    public class Greeter
    {
        public string Greet(string arg)
        {
            if (string.IsNullOrEmpty(arg))
            {
                return "Hello World";
            }
            else
            {
                return "Hello " + arg;
            }
        }
    }
}
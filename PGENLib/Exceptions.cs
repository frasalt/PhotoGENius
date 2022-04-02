namespace PGENLib
{
    public class InvalidPfmFileFormat : Exception
    {
        public InvalidPfmFileFormat(string errMessage)
        {
            Console.WriteLine(errMessage);
        }
    }
    public class RuntimeError : Exception
    {
        public RuntimeError(string errMessage)
        {
            Console.WriteLine(errMessage);
        }
    }
}
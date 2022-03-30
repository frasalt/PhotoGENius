namespace PGENLib
{
    public class InvalidPfmFileFormat : Exception
    {
        public InvalidPfmFileFormat(string errMessage)
        {
            Console.WriteLine(errMessage);
        }
    }
}
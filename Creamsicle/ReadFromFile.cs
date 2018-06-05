namespace Creamsicle
{
    class ReadFromFile
    {
        internal static string MainFile(string fileName)
        {
            string line;
            string fileContents = string.Empty;

            System.IO.StreamReader hwinfoLog = new System.IO.StreamReader(fileName);
            while((line = hwinfoLog.ReadLine()) != null)
            {
                fileContents += line;
            }
            hwinfoLog.Close();

            return fileContents;
        }
    }
}

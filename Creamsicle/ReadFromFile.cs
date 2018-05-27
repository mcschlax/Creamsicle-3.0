namespace Creamsicle
{
    class ReadFromFile
    {
        private static void MainFile(string[] args)
        {
            string line;

            System.IO.StreamReader hwinfoLog = new System.IO.StreamReader(@"C:\Users\Glitc\Desktop\logs.csv");
            while((line = hwinfoLog.ReadLine()) != null)
            {
                System.Console.WriteLine(line);
            }
            hwinfoLog.Close();
            System.Console.ReadLine();
        }
    }
}

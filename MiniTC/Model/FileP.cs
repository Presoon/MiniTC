namespace MiniTC.Model
{
    public class FileP
    {
        public string File { get; set; }
        public string Name { get; set; }
        
        public FileP(string fileP, string name)
        {
            File = fileP;
            Name = name;
        }
    }
}
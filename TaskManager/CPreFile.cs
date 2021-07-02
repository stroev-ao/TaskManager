namespace TaskManager
{
    public class CPreFile
    {
        public string FullPath { get; set; }
        public string AdditionalPath { get; set; }
        
        public CPreFile(string fullPath, string additionalPath)
        {
            FullPath = fullPath;
            AdditionalPath = additionalPath;
        }

        public override string ToString()
        {
            return FullPath;
        }
    }
}

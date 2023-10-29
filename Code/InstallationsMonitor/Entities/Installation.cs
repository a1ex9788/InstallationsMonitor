namespace InstallationsMonitor.Entities
{
    public class Installation
    {
        public Installation(string programName)
        {
            this.ProgramName = programName;
            this.FileOperationsNumber = 0;
        }

        public int Id { get; set; }

        public string ProgramName { get; set; }

        public int FileOperationsNumber { get; set; }
    }
}
namespace InstallationsMonitor.Entities
{
    public class Installation
    {
        public int Id { get; set; }

        public string ProgramName { get; set; }

        public int FileOperationsNumber { get; set; }
    }
}
using System;

namespace InstallationsMonitor.Entities
{
    public class Installation
    {
        public Installation(string programName, DateTime dateTime)
        {
            this.ProgramName = programName;
            this.DateTime = dateTime;
            this.FileOperationsNumber = 0;
        }

        public int Id { get; set; }

        public string ProgramName { get; set; }

        public DateTime DateTime { get; set; }

        public int FileOperationsNumber { get; set; }
    }
}
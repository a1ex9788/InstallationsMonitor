using System;

namespace InstallationsMonitor.Entities
{
    public class Installation
    {
        public Installation(string programName, DateTime dateTime)
        {
            this.ProgramName = programName;
            this.DateTime = dateTime;
        }

        public int Id { get; set; }

        public string ProgramName { get; set; }

        public DateTime DateTime { get; set; }
    }
}
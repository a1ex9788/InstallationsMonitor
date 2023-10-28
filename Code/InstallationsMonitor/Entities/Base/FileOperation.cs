using System;

namespace InstallationsMonitor.Entities.Base
{
    public class FileOperation
    {
        public int Id { get; set; }

        public string FileName { get; set; }

        public DateTime DateTime { get; set; }

        public int InstallationId { get; set; }
    }
}
using System;

namespace InstallationsMonitor.Domain.Base
{
    public class FileOperation
    {
        public FileOperation(string filePath, DateTime dateTime, int installationId)
        {
            this.FilePath = filePath;
            this.DateTime = dateTime;
            this.InstallationId = installationId;
        }

        public int Id { get; set; }

        public string FilePath { get; set; }

        public DateTime DateTime { get; set; }

        public int InstallationId { get; set; }
    }
}
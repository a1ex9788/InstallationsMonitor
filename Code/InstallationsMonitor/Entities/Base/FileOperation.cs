using System;

namespace InstallationsMonitor.Entities.Base
{
    public class FileOperation
    {
        public FileOperation(string fileName, DateTime dateTime, int installationId)
        {
            this.FileName = fileName;
            this.DateTime = dateTime;
            this.InstallationId = installationId;
        }

        public int Id { get; set; }

        public string FileName { get; set; }

        public DateTime DateTime { get; set; }

        public int InstallationId { get; set; }
    }
}
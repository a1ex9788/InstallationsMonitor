using InstallationsMonitor.Entities.Base;
using System;

namespace InstallationsMonitor.Entities
{
    public class FileRenaming : FileOperation
    {
        public FileRenaming(string filePath, DateTime dateTime, int installationId, string oldPath)
            : base(filePath, dateTime, installationId)
        {
            this.OldPath = oldPath;
        }

        public string OldPath { get; set; }
    }
}
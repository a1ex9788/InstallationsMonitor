using InstallationsMonitor.Domain.Base;
using System;

namespace InstallationsMonitor.Domain
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
using InstallationsMonitor.Entities.Base;
using System;

namespace InstallationsMonitor.Entities
{
    public class FileRenaming : FileOperation
    {
        public FileRenaming(string fileName, DateTime dateTime, int installationId, string oldName)
            : base(fileName, dateTime, installationId)
        {
            this.OldName = oldName;
        }

        public string OldName { get; set; }
    }
}
using InstallationsMonitor.Entities.Base;
using System;

namespace InstallationsMonitor.Entities
{
    public class FileChange : FileOperation
    {
        public FileChange(string filePath, DateTime dateTime, int installationId)
            : base(filePath, dateTime, installationId)
        {
        }
    }
}
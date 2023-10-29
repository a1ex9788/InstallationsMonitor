using InstallationsMonitor.Entities.Base;
using System;

namespace InstallationsMonitor.Entities
{
    public class FileChange : FileOperation
    {
        public FileChange(string fileName, DateTime dateTime, int installationId)
            : base(fileName, dateTime, installationId)
        {
        }
    }
}
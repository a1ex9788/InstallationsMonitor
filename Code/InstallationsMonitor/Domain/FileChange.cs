using InstallationsMonitor.Domain.Base;
using System;

namespace InstallationsMonitor.Domain
{
    public class FileChange : FileOperation
    {
        public FileChange(string filePath, DateTime dateTime, int installationId)
            : base(filePath, dateTime, installationId)
        {
        }
    }
}
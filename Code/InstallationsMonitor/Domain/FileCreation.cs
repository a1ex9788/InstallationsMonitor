using InstallationsMonitor.Domain.Base;
using System;

namespace InstallationsMonitor.Domain
{
    public class FileCreation : FileOperation
    {
        public FileCreation(string filePath, DateTime dateTime, int installationId)
            : base(filePath, dateTime, installationId)
        {
        }
    }
}
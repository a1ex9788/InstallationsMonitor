using InstallationsMonitor.Entities.Base;
using System;

namespace InstallationsMonitor.Entities
{
    public class FileCreation : FileOperation
    {
        public FileCreation(string filePath, DateTime dateTime, int installationId)
            : base(filePath, dateTime, installationId)
        {
        }
    }
}
using InstallationsMonitor.Entities.Base;
using System;

namespace InstallationsMonitor.Entities
{
    public class FileCreation : FileOperation
    {
        public FileCreation(string fileName, DateTime dateTime, int installationId)
            : base(fileName, dateTime, installationId)
        {
        }
    }
}
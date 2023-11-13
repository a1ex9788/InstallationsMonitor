using InstallationsMonitor.Entities.Base;
using System;

namespace InstallationsMonitor.Entities
{
    public class FileDeletion : FileOperation
    {
        public FileDeletion(string filePath, DateTime dateTime, int installationId)
            : base(filePath, dateTime, installationId)
        {
        }
    }
}
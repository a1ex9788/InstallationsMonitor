using InstallationsMonitor.Entities.Base;
using System;

namespace InstallationsMonitor.Entities
{
    public class FileDeletion : FileOperation
    {
        public FileDeletion(string fileName, DateTime dateTime, int installationId)
            : base(fileName, dateTime, installationId)
        {
        }
    }
}
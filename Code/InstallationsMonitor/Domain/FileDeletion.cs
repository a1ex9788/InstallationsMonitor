using InstallationsMonitor.Domain.Base;
using System;

namespace InstallationsMonitor.Domain
{
    public class FileDeletion : FileOperation
    {
        public FileDeletion(string filePath, DateTime dateTime, int installationId)
            : base(filePath, dateTime, installationId)
        {
        }
    }
}
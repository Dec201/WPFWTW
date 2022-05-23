using System;
using System.IO;

namespace WPFWTW.Logger
{
    public class FileLogger : ILogger
    {

        private readonly string _path;
        private readonly DateTime _currentDate = DateTime.UtcNow;

        public FileLogger(string path)
        {
            _path = path;
        }

        public void LogError(string errorCode, string fileName)
        {
            Log(errorCode, "Error: ", fileName);
        }

        public void LogInfo(string infoCode, string fileName)
        {
            Log(infoCode, "Information: ", fileName);
        }


        private void Log(string message, string messageType, string fileName)
        {

            using(var streamWriter = new StreamWriter(_path, true))
            {
                streamWriter.WriteLine("File Name: " + fileName + ", " + messageType + message + ", Date/Time: " + _currentDate);
            }


        }


    }
}

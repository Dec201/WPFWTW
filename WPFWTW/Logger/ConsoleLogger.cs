using System;

namespace WPFWTW.Logger
{
    public class ConsoleLogger : ILogger
    {

        private readonly DateTime _currentDate = DateTime.UtcNow;

        public ConsoleLogger()
        {

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

                Console.WriteLine("File Name: " + fileName + ", " + messageType + message + ", Date/Time: " + _currentDate);
            
        }


    }

}

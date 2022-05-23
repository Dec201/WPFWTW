using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFWTW.Logger
{
    public interface ILogger
    {

        void LogError(string errorCode, string fileName);

        void LogInfo(string infoCode, string fileName);

    }
}

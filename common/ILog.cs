using System;

namespace OnlineExamAPI.common
{
    public interface ILog
    {
        void Information(object message);  
        void Warning(string message);  
        void Debug(string message);  
        void Error(string message); 
    }
}
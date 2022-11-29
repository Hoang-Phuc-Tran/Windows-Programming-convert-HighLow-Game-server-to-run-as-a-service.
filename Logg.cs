using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
    * CLASS NAME:  Logg
    * PURPOSE : This is to log the nessacary log to log file
    */
namespace AsynchronousServer
{
    public class Logg
    {
        static string pathName = ConfigurationManager.AppSettings["pathFile"];

        public static object _lock = new object();

        /* 
        Name	: Log
        Purpose : It is to write the message to log file
        Inputs	: NONE
        Outputs	: NONE
        Returns	: Nothing
        */
        public static void Log(string message)
        {
            lock (_lock)
            {
                using (StreamWriter writer = new StreamWriter(pathName, true))
                {
                    writer.WriteLine(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt: ") + message);
                    writer.Close();
                }
            }
            
        }
    }
}

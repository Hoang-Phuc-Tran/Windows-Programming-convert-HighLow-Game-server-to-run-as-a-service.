/*
 * Project:	    A - 06 : Services
 * Author:	    Hoang Phuc Tran - 8789102
                Bumsu Yi - 8110678
 * Date:		December 28, 2022
 * Description: An application call the MyServer
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace AsynchronousServer
{
    /*
    * CLASS NAME:  Program
    * PURPOSE : This class is used to call the MyServer
    */
    internal class Program
    {
        static void Main(string[] args)
        {
            MyServer myServer = new MyServer();

        }
    }
}

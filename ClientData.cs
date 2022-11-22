/*
 * Project:	    A - 05 : TCP/IP
 * Author:	    Hoang Phuc Tran - 8789102
                Bumsu Yi - 8110678
 * Date:		December 21, 2022
 * Description: An application is used to store the data from client
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AsynchronousServer
{
    /*
    * CLASS NAME:  ClientData
    * PURPOSE : This class is used to store the data from client
    */
    class ClientData
    {
        public TcpClient client { get; set; }
        public byte[] readData { get; set; }
        public int numberClient;

        /*
         CLASS NAME:  ClientData
        * PURPOSE : a new ClientData object - given a set of attribute values
        */
        public ClientData(TcpClient client)
        {
            this.client = client;
            this.readData = new byte[1024];
            
            // 127.0.0.1:9999, the last digit is the client number as an identifier
            string clientEndPoint = client.Client.LocalEndPoint.ToString();
            char[] point = { '.', ':' };
            string[] splitedData = clientEndPoint.Split(point);
            this.numberClient = int.Parse(splitedData[3]);

            Console.WriteLine("User {0}'s connected successfully", numberClient);
        }
    }
}

/*
 * Project:	    A - 06 : MyServer.cs
 * Author:	    Hoang Phuc Tran - 8789102
                Bumsu Yi - 8110678
 * Date:		December 28, 2022
 * Description: An application is the sever for the Hi-lo Game
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Runtime.Remoting;
using System.Security.Policy;

namespace AsynchronousServer
{
    /*
    * CLASS NAME:  MyServer
    * PURPOSE : This class is used to as a sever for hi-lo Game
    */
    public class MyServer
    {
        TcpClient acceptClient;

        ClientData clientData;
        NetworkStream stream;
        public TcpListener listener = null;

        // get min and max from the App.config File
         int minNum = Convert.ToInt32(ConfigurationManager.AppSettings["minNum"]);
         int maxNum = Convert.ToInt32(ConfigurationManager.AppSettings["maxNum"]);
         int ranNum;
         int curMin;
         int curMax;
         bool win;
        bool exit = true;

        /*  -- Method Header Comment
        Name	: MyServer -- CONSTRUCTOR
        Purpose : to instantiate a new MainWindow object - given a set of attribute values
        Inputs	: NONE
        Outputs	: NONE
        Returns	: Nothing
        */
        public MyServer()
        {
            curMin = minNum;
            curMax = maxNum;
            win = false;
            ranNum = SetRanNum();
            

            // starting server
            AsyncServerStart();
        }

        /*  -- Method Header Comment
	    Name	: AsyncServerStart
	    Purpose : this property is used to asynchronize when sever starts.
	    Inputs	:	NONE
	    Outputs	:	NONE
	    Returns	:	NONE
        */
        private void AsyncServerStart()
        {
            listener = new TcpListener(new IPEndPoint(IPAddress.Any, 9999));
            
            listener.Start();
            Logg.Log("Waiting for a connection...");

            while (exit)
            {
                try
                {
                    acceptClient = listener.AcceptTcpClient();
                    Logg.Log("Range is " + minNum + " and " + maxNum);
                    Logg.Log("Random number is: " + ranNum);

                    clientData = new ClientData(acceptClient);
                    stream = acceptClient.GetStream();
                    clientData.client.GetStream().BeginRead(clientData.readData, 0, clientData.readData.Length, new AsyncCallback(DataReceived), clientData);

                    // send data to client
                    string dataToClient = "Game has started, your guess range is " + curMin + " to " + curMax + "\n";
                    byte[] msg = System.Text.Encoding.UTF8.GetBytes(dataToClient);
                    stream.Write(msg, 0, msg.Length);
                }
                catch (Exception ex) 
                {
                    Logg.Log("Exception: " + ex + "\n");
                }
            }
        }

        /*  -- Method Header Comment
	    Name	: DataReceived
	    Purpose : this property is used to receive the data.
	    Inputs	:	IAsyncResult      ar
	    Outputs	:	NONE
	    Returns	:	NONE
        */
        private void DataReceived(IAsyncResult ar)
        {
            // Get data from the client
            ClientData callbackClient = ar.AsyncState as ClientData;
            int bytesRead = callbackClient.client.GetStream().EndRead(ar);
            string readString = Encoding.Default.GetString(callbackClient.readData, 0, bytesRead);

            if(readString == "Disconnected")
            {
                Logg.Log("Disconnected" + "\n");
                stream.Close();
                acceptClient.Close();
                win = false;
                ranNum = SetRanNum();
                return;
            }

           
            // convert string to interger
            int guess = Convert.ToInt32(readString);

            SendResponseToClient(guess);

            callbackClient.client.GetStream().BeginRead(callbackClient.readData, 0, callbackClient.readData.Length, new AsyncCallback(DataReceived), callbackClient);
        }

        /*  -- Method Header Comment
	    Name	: SendResponseToClient
	    Purpose : this property is used to validate the range of Hilo game and send it back to client
	    Inputs	:	int      guess
	    Outputs	:	NONE
	    Returns	:	NONE
        */
        private void SendResponseToClient(int guess)
        {
            string dataToClient;
           
            if(guess > ranNum && guess <= curMax && win == false)
            {
                curMax = guess - 1;
                dataToClient = "Guess is too high! Your guess range is " + curMin + " to " + curMax + "\n";
            }
            else if(guess == ranNum && win == false)
            {
                dataToClient = "Congratulations!!! Your number " + ranNum + " is correct!\n" +
                    "DO YOU WANT TO PLAY AGIAN! Type: 1 to Play Again/ 2 to Exit!\n";
                win = true;
            }
            else if(guess < ranNum && guess >= curMin && win == false)
            {
                curMin = guess + 1;
                dataToClient = "Guess is too low! Your guess range is " + curMin + " to " + curMax + "\n";
            }
            else
            {
                dataToClient = "Your guess is out of range!\n";
            }

            try
            {
                if(win == true && guess == 1)
                {
                    curMin = minNum;
                    curMax = maxNum;
                    ranNum = SetRanNum();
                    win = false;
                    dataToClient = "Game has started, your guess range is " + curMin + " to " + curMax + "\n";
                    Logg.Log("Range is " + minNum + " and " + maxNum);
                    Logg.Log("Random number is: " + ranNum);
                }

                if (win == true && guess == 2)
                {
                    Console.WriteLine("Disconnected");
                    dataToClient = "Disconnected";
                    exit= true;
                    acceptClient.Close();
                    stream.Close();
                    
                }


                // send the message back to client
                byte[] msg = System.Text.Encoding.UTF8.GetBytes(dataToClient);
                stream.Write(msg, 0, msg.Length);
            }
            catch (Exception ex)
            {
                Logg.Log("Exception: " + ex + "\n");
            }
        }

        /*  -- Method Header Comment
	    Name	: SetRanNum
	    Purpose : this property is used to generate random number
	    Inputs	:	NONE
	    Outputs	:	NONE
	    Returns	:	NONE
        */
        private int SetRanNum()
        {
            Random r = new Random();
            int rInt = r.Next(minNum, maxNum);
            return rInt;
        }
    }
}

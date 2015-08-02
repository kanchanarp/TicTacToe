using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.IO;

namespace TicTacToeGUI
{
    public class clsClient
    {
        TicTacToe frmTicTacToe = null;

        public Thread thread_receive_client;
        private string wServerIP;
        const int SERVERPORT = 100;
        bool wReceivingClient = true;
        NetworkStream clientSockStream;
        TcpClient tcpClient;

        #region Client
        public clsClient(TicTacToe frmTicTacToe)
        {
            this.frmTicTacToe = frmTicTacToe;       
        }
        public void ConnectServer()
        {

            IPHostEntry localHostEntry = Dns.GetHostByName(Dns.GetHostName());
            wServerIP = localHostEntry.AddressList[0].ToString();
            byte[] buf = new byte[1];
            
            thread_receive_client = new Thread(new ThreadStart(ThreadReceivingClient));
            thread_receive_client.Start();
            SendsSetPacket();
        }


        private void ThreadReceivingClient()
        {
            
            try
            {

                byte[] buf = new byte[512];
                int bytesReceived = 0;

                tcpClient = new TcpClient(wServerIP, SERVERPORT);
                clientSockStream = tcpClient.GetStream();


                wReceivingClient = true;

                while (wReceivingClient)
                {

                    try
                    {
                        bytesReceived = clientSockStream.Read(buf, 0, 3);
                    }
                    catch
                    {
                        return;
                    }

                    if (bytesReceived > 0)
                    {


                        if (buf[0] == byte.Parse(ConvertToAscii("X").ToString()))
                        {
                            frmTicTacToe.setPlay(0);
                            continue;
                        }
                        else if (buf[0] == byte.Parse(ConvertToAscii("O").ToString()))
                        {
                            frmTicTacToe.setPlay(1);
                            continue;
                        }
                        if (buf[0] == byte.Parse(ConvertToAscii("D").ToString()))
                        {
                            frmTicTacToe.Disconnect();
                            continue;
                        }
                        int wRow = int.Parse(Convert.ToChar(buf[0]).ToString());
                        int wColumn = int.Parse(Convert.ToChar(buf[1]).ToString());

                        int type = int.Parse(Convert.ToChar(buf[2]).ToString());

                        if ((wRow >= 0 && wRow < 3) && (wColumn >= 0 && wColumn < 3))
                        {
    
                            int[] vec = { wRow, wColumn };
                            frmTicTacToe.netPlay(vec, type);
                        }

                    } 

                } 

            }
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                MessageBox.Show("An error ocurred: " + ex.Message + "\n" + ex.StackTrace);
                //objTicTacToe.mnDisconnect_Click(null, null);
                return;
            }
        }
#endregion

        #region Functions for sending packets/disconnect
        public void SendPacketTCP(Byte[] pDados)
        {
            try
            {
                    if (clientSockStream == null)
                        return;

                    if (clientSockStream.CanWrite)
                    {
                        clientSockStream.Write(pDados, 0, 3);
                        clientSockStream.Flush();
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error ocurred: " + ex.Message + "\n" + ex.StackTrace);
                //objTicTacToe.mnDisconnect_Click(null, null);
                return;
            }

        }

        public void SendMove(int wRow, int wColumn,int type)
        {
            
            byte[] buf = new byte[3];
            buf[0] = byte.Parse(ConvertToAscii(wRow.ToString()).ToString());
            buf[1] = byte.Parse(ConvertToAscii(wColumn.ToString()).ToString());
            buf[2] = byte.Parse(ConvertToAscii(type.ToString()).ToString());
            SendPacketTCP(buf);

        }

  
        public void SendsSetPacket()
        {
            byte[] buf = new byte[3];
            buf[0] = byte.Parse(ConvertToAscii("S").ToString());
            buf[1] = 0;
            buf[2] = 0;
            SendPacketTCP(buf);

        }
        public void Disconnect()
        {
            byte[] buf = new byte[3];
            buf[0] = byte.Parse(ConvertToAscii("D").ToString());
            buf[1] = 0;
            buf[2] = 0;
            SendPacketTCP(buf);
                thread_receive_client.Abort();

                wReceivingClient = false;

                if (clientSockStream != null)
                    clientSockStream.Close();

                if (tcpClient != null)
                    tcpClient.Close();
        }

        private static int ConvertToAscii(string character)
        {
            if (character.Length == 1)
            {
                System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                int intAsciiCode = (int)asciiEncoding.GetBytes(character)[0];
                return (intAsciiCode);
            }
            else
            {
                throw new ApplicationException("Character is not valid.");
            }

        }
    }
    #endregion 
}

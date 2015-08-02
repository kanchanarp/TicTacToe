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
    public class clsServer
    {
        TicTacToe frmTicTacToe = null;

        public Thread thread_receive_server;
        private string wServerIP;
        const int SERVERPORT = 100;
        bool wReceivingServer = true;
        NetworkStream serverSockStream;
        TcpListener tcpListener;
        Socket soTcpServer;
        int play = 1;

        #region Server

        public clsServer(TicTacToe frmTicTacToe)
        {
            this.frmTicTacToe = frmTicTacToe;
        }
        public void setPlay(int play) {
            this.play = play;
        }
        public void StartServer()
        {
            thread_receive_server = new Thread(new ThreadStart(ThreadReceivingServer));
            thread_receive_server.Start();
        }


        private void ThreadReceivingServer()
        {
            try
            {
                byte[] buf = new byte[512];
                IPHostEntry localHostEntry = Dns.GetHostByName(Dns.GetHostName());
                int bytesReceived = 0;

                tcpListener = new TcpListener(localHostEntry.AddressList[0], SERVERPORT);

                tcpListener.Start();

                soTcpServer = tcpListener.AcceptSocket();

                serverSockStream = new NetworkStream(soTcpServer);

                wReceivingServer = true;

                while (wReceivingServer)
                {

                    try
                    {
                        bytesReceived = serverSockStream.Read(buf, 0, 3);
                    }
                    catch
                    {
                        return;
                    }

                    if (bytesReceived > 0)
                    {

                        if (buf[0] == byte.Parse(ConvertToAscii("S").ToString()))
                        {

                            SendsStartPacket(play);
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

                            int[] vec={wRow,wColumn};
                            frmTicTacToe.netPlay(vec,type);
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
                
                    if (serverSockStream == null)
                        return;

                    if (serverSockStream.CanWrite)
                    {
                        serverSockStream.Write(pDados, 0, 3);
                        serverSockStream.Flush();
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

        public void SendsStartPacket(int play)
        {
            string playT = (play == 1 ? "X" : "O");
            byte[] buf = new byte[3];
            buf[0] = byte.Parse(ConvertToAscii(playT).ToString());
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
           
                thread_receive_server.Abort();

                wReceivingServer = false;

                if (serverSockStream != null)
                    serverSockStream.Close();

                if (tcpListener != null)
                    tcpListener.Stop();

                if (soTcpServer != null)
                    soTcpServer.Shutdown(SocketShutdown.Both);

            

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

        #endregion

    }
}

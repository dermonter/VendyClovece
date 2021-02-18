using SimpleServer.Backend;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SimpleServer.Server
{
    public class ServerTcp
    {
        private TcpListener socket;
        private int port;
        private const int BUFFER_SIZE = 1024 * 1024;
        private ServerHandle serverHandle;

        public ServerTcp(int port = 42069)
        {
            this.port = port;
            socket = new TcpListener(IPAddress.Any, port);
            serverHandle = new ServerHandle();
        }

        public void Run()
        {
            try
            {
                socket.Start();

                while (true)
                {
                    Console.WriteLine("Waiting for a connection");

                    TcpClient client = socket.AcceptTcpClient();
                    client.ReceiveBufferSize = BUFFER_SIZE;
                    client.SendBufferSize = BUFFER_SIZE;

                    NetworkStream stream = client.GetStream();
                    int receiveSize;
                    byte[] receiveBuffer = new byte[BUFFER_SIZE];
                    using (ByteBuffer buffer = new ByteBuffer())
                    {
                        // read packet from client
                        receiveSize = stream.Read(receiveBuffer, 0, receiveBuffer.Length);

                        // might write junk bytes to the end but it should not matter
                        byte[] tempBuffer = new byte[receiveSize];
                        Array.Copy(receiveBuffer, 0, tempBuffer, 0, receiveSize);
                        buffer.WriteBytes(tempBuffer);

                        serverHandle.HandlePackets(client, buffer);
                    }

                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("Socket exception: {0}", e);
            }
            finally
            {
                socket.Stop();
            }
        }
    }
}

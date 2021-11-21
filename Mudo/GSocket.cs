using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Mudo
{
    class GSocket : Socket
    {
        public enum RWMode
        {
            Read,
            Write,
            ReadWrite
        }

        uint id;
        string addr;
        int port;
        public delegate void ValueReceivedEventHandler(uint value_id, float value);
        public event ValueReceivedEventHandler ValueReceived;
        public delegate void ConnectedEventHandler();
        public event ConnectedEventHandler ServerConnected;
        NetworkStream stream = null;
        byte[] first = new byte[1];
        RWMode mode = RWMode.Read;
        void Send(uint value_id, float value, bool blocking = false)
        {
            if (mode == RWMode.Read) throw new UnauthorizedAccessException();
            throw new NotImplementedException(); //Send
        }

        void Read()
        {
            if (mode == RWMode.Write) throw new UnauthorizedAccessException();
            throw new NotImplementedException(); //Xml Parse, Event Raise, Read wait task
        }

        void GConnect()
        {
            //TODO: Request Socket Permission via the GMetaSocket
            this.Connect(addr, port);
        }

        void OnConnectStateChange()
        {   
            if(this.Connected)
            {
                if(stream == null)
                    stream = new NetworkStream(this);
                if(mode != RWMode.Write)
                    stream.ReadAsync(first, 0, 1).ContinueWith((Task p)=>Read());
                ServerConnected.Invoke();
            }
        }

        public GSocket(uint id, string addr, int port, RWMode mode) : base(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp) {
            this.id = id;
            this.addr = addr;
            this.port = port;
            this.mode = mode;
            Task a = new Task(() => GConnect());
            a.ContinueWith((Task p) => OnConnectStateChange());
            a.Start();

        }
    }
}

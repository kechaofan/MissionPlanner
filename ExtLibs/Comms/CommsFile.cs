﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.IO;

namespace MissionPlanner.Comms
{
    public class CommsFile : CommsBase, ICommsSerial
    {
        // Methods
        public void Close() { BaseStream.Close(); }
        public void DiscardInBuffer() { }

        public int bps { get; set; }
        int currentbps = 0;
        int sleepvalue = 1;
        DateTime lastread = DateTime.MinValue;

        //void DiscardOutBuffer();
        public void Open(string filename)
        {
            bps = 3000;
            PortName = filename;
            BaseStream = File.OpenRead(PortName);
        }

        public void Open()
        {
            bps = 3000;
            BaseStream = File.OpenRead(PortName);
        }
        public int Read(byte[] buffer, int offset, int count)
        {
            if (count > 1)
                System.Threading.Thread.Sleep(sleepvalue);

            if (currentbps > bps)
                sleepvalue++;

            if (lastread.Second != DateTime.Now.Second)
            {
                Console.WriteLine("commfile read bps {0} - {1}", currentbps, sleepvalue);
                currentbps = 0;
                lastread = DateTime.Now;
                sleepvalue = 1;
            }
            currentbps += count;
            return BaseStream.Read(buffer, offset, count);
        }
        //int Read(char[] buffer, int offset, int count);
        public int ReadByte() { return BaseStream.ReadByte(); }
        public int ReadChar() { return BaseStream.ReadByte(); }
        public string ReadExisting() { return ""; }
        public string ReadLine() { return ""; }
        //string ReadTo(string value);
        public void Write(string text) { }
        public void Write(byte[] buffer, int offset, int count) { }
        //void Write(char[] buffer, int offset, int count);
        public void WriteLine(string text) { }

        public void toggleDTR() { }

        // Properties
        public Stream BaseStream { get; private set; }
        public int BaudRate { get; set; }
        public int BytesToRead { get { if (!BaseStream.CanRead) return 0; return (int)(BaseStream.Length - BaseStream.Position); } }
        public int BytesToWrite { get; set; }
        public int DataBits  { get; set; }
        public bool DtrEnable { get; set; }
        public bool IsOpen { get { if (BaseStream != null && BaseStream.CanRead) { return BaseStream.Position < BaseStream.Length; } return false; } }

        public Parity Parity { get; set; }

        public string PortName { get; set; }
        public int ReadBufferSize { get; set; }
        public int ReadTimeout { get; set; }
        public bool RtsEnable { get; set; }
        public StopBits StopBits { get; set; }
        public int WriteBufferSize { get; set; }
        public int WriteTimeout { get; set; }
    }
}

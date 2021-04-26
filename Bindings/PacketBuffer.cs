using System;
using System.Collections.Generic;
using System.Text;

namespace Bindings
{
    class PacketBuffer : IDisposable
    {
        List<byte> Buff;
        byte[] ReadBuff;
        int ReadPos;
        bool BuffUpdate = false;

        public PacketBuffer()
        {
            Buff = new List<byte>();
            ReadPos = 0;
        }

        public int GetReadPos()
        {
            return ReadPos;
        }

        public byte[] ToArray()
        {
            return Buff.ToArray();
        }

        public int Count()
        {
            return Buff.Count;
        }

        public int Length()
        {
            return Count() - ReadPos;
        }

        public void Clear()
        {
            Buff.Clear();
            ReadPos = 0;
        }

        // Write Data
        public void AddInt(int input)
        {
            Buff.AddRange(BitConverter.GetBytes(input));
            BuffUpdate = true;
        }

        public void AddFloat(float input)
        {
            Buff.AddRange(BitConverter.GetBytes(input));
            BuffUpdate = true;
        }

        public void AddDouble(double input)
        {
            Buff.AddRange(BitConverter.GetBytes(input));
            BuffUpdate = true;
        }

        public void AddShort(short input)
        {
            Buff.AddRange(BitConverter.GetBytes(input));
            BuffUpdate = true;
        }

        public void AddLong(long input)
        {
            Buff.AddRange(BitConverter.GetBytes(input));
            BuffUpdate = true;
        }

        public void AddString(string input)
        {
            Buff.AddRange(BitConverter.GetBytes(input.Length));
            Buff.AddRange(Encoding.ASCII.GetBytes(input));
            BuffUpdate = true;
        }

        public void AddByte(byte input)
        {
            Buff.Add(input);
            BuffUpdate = true;
        }

        public void AddBool(bool input)
        {
            Buff.Add(input ? (byte)1 : (byte)0);
            BuffUpdate = true;
        }

        public void AddBytes(byte[] input)
        {
            Buff.AddRange(input);
            BuffUpdate = true;
        }

        // Read Data
        public int GetInt(bool peek = true)
        {
            if (Count() > ReadPos)
            {
                if (BuffUpdate)
                {
                    ReadBuff = ToArray();
                    BuffUpdate = false;
                }

                int res = BitConverter.ToInt32(ReadBuff, ReadPos);
                if (peek && Count() > ReadPos) ReadPos += 4;
                return res;
            }
            throw new Exception("Packet buffer à dépassé ça limite!");
        }

        public float GetFloat(bool peek = true)
        {
            if (Count() > ReadPos)
            {
                if (BuffUpdate)
                {
                    ReadBuff = ToArray();
                    BuffUpdate = false;
                }

                float res = BitConverter.ToSingle(ReadBuff, ReadPos);
                if (peek && Count() > ReadPos) ReadPos += 4;
                return res;
            }
            throw new Exception("Packet buffer à dépassé ça limite!");
        }

        public double GetDouble(bool peek = true)
        {
            if (Count() > ReadPos)
            {
                if (BuffUpdate)
                {
                    ReadBuff = ToArray();
                    BuffUpdate = false;
                }

                double res = BitConverter.ToDouble(ReadBuff, ReadPos);
                if (peek && Count() > ReadPos) ReadPos += 8;
                return res;
            }
            throw new Exception("Packet buffer à dépassé ça limite!");
        }

        public short ReadShort(bool peek = true)
        {

            if (Count() > ReadPos)
            {
                if (BuffUpdate)
                {
                    ReadBuff = ToArray();
                    BuffUpdate = false;
                }
                short res = BitConverter.ToInt16(ReadBuff, ReadPos);
                if (peek & Count() > ReadPos) ReadPos += 2;
                return res;
            }
            throw new Exception("Packet buffer à dépassé ça limite!");
        }

        public long ReadLong(bool peek = true)
        {
            if (Count() > ReadPos)
            {
                if (BuffUpdate)
                {
                    ReadBuff = ToArray();
                    BuffUpdate = false;
                }
                long res = BitConverter.ToInt64(ReadBuff, ReadPos);
                if (peek & Count() > ReadPos) ReadPos += 8;
                return res;
            }
            throw new Exception("Packet buffer à dépassé ça limite!");
        }

        public string GetString(bool peek = true)
        {
            int length = GetInt();
            if (BuffUpdate)
            {
                ReadBuff = ToArray();
                BuffUpdate = false;
            }

            string res = Encoding.ASCII.GetString(ReadBuff, ReadPos, length);
            if (peek && Count() > ReadPos)
            {
                if (res.Length > 0)
                {
                    ReadPos += length;
                }
            }
            return res;
        }

        public byte GetByte(bool peek = true)
        {
            if (Count() > ReadPos)
            {
                if (BuffUpdate)
                {
                    ReadBuff = ToArray();
                    BuffUpdate = false;
                }

                byte res = ReadBuff[ReadPos];
                if (peek && Count() > ReadPos) ReadPos += 1;
                return res;
            }
            throw new Exception("Packet buffer à dépassé ça limite!");
        }

        public bool GetBool(bool peek = true)
        {
            if (Count() > ReadPos)
            {
                if (BuffUpdate)
                {
                    ReadBuff = ToArray();
                    BuffUpdate = false;
                }

                byte res = ReadBuff[ReadPos];
                if (peek && Count() > ReadPos) ReadPos += 1;
                return res == 1 ? true : false;
            }
            throw new Exception("Packet buffer à dépassé ça limite!");
        }

        public byte[] GetBytes(int length, bool peek = true)
        {
            if (BuffUpdate)
            {
                ReadBuff = ToArray();
                BuffUpdate = false;
            }

            byte[] res = Buff.GetRange(ReadPos, length).ToArray();
            if (peek)
            {
                ReadPos += length;
            }
            return res;
        }

        // IDisposable
        private bool DisposeValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!DisposeValue)
            {
                if (disposing)
                {
                    Buff.Clear();
                }
                ReadPos = 0;
            }
            DisposeValue = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

using System;
using System.IO;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

namespace TibSunLegacy.Util
{
    public static class StreamWritingExtensions
    {
        public static void WriteBytes(
            this Stream AByteStream,
            [CanBeNull] byte[] ABytes)
        {
            if (AByteStream == null)
                throw new ArgumentNullException("AByteStream");

            if (ABytes == null)
                return;
            
            for (uint I = 0; I < ABytes.LongLength; I++)
                AByteStream.WriteByte(ABytes[I]);
        }

        public static void CopyBytes(
            this Stream AByteStream,
            Stream ATargetStream,
            uint ACount,
            uint ABufferSize = 1024)
        {
            if (AByteStream == null)
                throw new ArgumentNullException("AByteStream");
            if (ATargetStream == null)
                throw new ArgumentNullException("ATargetStream");

            if (ABufferSize == 0)
                throw new ArgumentOutOfRangeException("ABufferSize");

            uint nRemain = ACount;
            byte[] aBuffer = new byte[ABufferSize];

            while (nRemain > 0)
            {
                uint nToRead = System.Math.Min(ABufferSize, nRemain);
                int iRead = AByteStream.Read(aBuffer, 0, (int)nToRead);
                if (iRead < nToRead)
                    throw new EndOfStreamException();

                ATargetStream.Write(aBuffer, 0, iRead);
                nRemain -= (uint)iRead;
            }
        }

        public static void WriteUInt16(
            this Stream AByteStream,
            ushort AUInt16,
            bool ALittleEndian = true)
        {
            if (AByteStream == null)
                throw new ArgumentNullException("AByteStream");

            byte[] aWrite = BitConverter.GetBytes(AUInt16);

            if (BitConverter.IsLittleEndian != ALittleEndian)
                Array.Reverse(aWrite);

            AByteStream.WriteBytes(aWrite);
        }
        public static void WriteUInt32(
            this Stream AByteStream,
            uint AUInt32,
            bool ALittleEndian = true)
        {
            if (AByteStream == null)
                throw new ArgumentNullException("AByteStream");

            byte[] aWrite = BitConverter.GetBytes(AUInt32);

            if (BitConverter.IsLittleEndian != ALittleEndian)
                Array.Reverse(aWrite);

            AByteStream.WriteBytes(aWrite);
        }
        public static void WriteUInt64(
            this Stream AByteStream,
            ulong AUInt64,
            bool ALittleEndian = true)
        {
            if (AByteStream == null)
                throw new ArgumentNullException("AByteStream");

            byte[] aWrite = BitConverter.GetBytes(AUInt64);

            if (BitConverter.IsLittleEndian != ALittleEndian)
                Array.Reverse(aWrite);

            AByteStream.WriteBytes(aWrite);
        }

        public static void WriteInt16(
            this Stream AByteStream,
            short AInt16,
            bool ALittleEndian = true)
        {
            if (AByteStream == null)
                throw new ArgumentNullException("AByteStream");

            byte[] aWrite = BitConverter.GetBytes(AInt16);

            if (BitConverter.IsLittleEndian != ALittleEndian)
                Array.Reverse(aWrite);

            AByteStream.WriteBytes(aWrite);
        }
        public static void WriteInt32(
            this Stream AByteStream,
            int AInt32,
            bool ALittleEndian = true)
        {
            if (AByteStream == null)
                throw new ArgumentNullException("AByteStream");

            byte[] aWrite = BitConverter.GetBytes(AInt32);

            if (BitConverter.IsLittleEndian != ALittleEndian)
                Array.Reverse(aWrite);

            AByteStream.WriteBytes(aWrite);
        }
        public static void WriteInt64(
            this Stream AByteStream,
            long AInt64,
            bool ALittleEndian = true)
        {
            if (AByteStream == null)
                throw new ArgumentNullException("AByteStream");

            byte[] aWrite = BitConverter.GetBytes(AInt64);

            if (BitConverter.IsLittleEndian != ALittleEndian)
                Array.Reverse(aWrite);

            AByteStream.WriteBytes(aWrite);
        }

        public static void WriteFloat(
            this Stream AByteStream,
            float AFloat,
            bool ALittleEndian = true)
        {
            if (AByteStream == null)
                throw new ArgumentNullException("AByteStream");

            byte[] aWrite = BitConverter.GetBytes(AFloat);

            if (BitConverter.IsLittleEndian != ALittleEndian)
                Array.Reverse(aWrite);

            AByteStream.WriteBytes(aWrite);
        }
        public static void WriteDouble(
            this Stream AByteStream,
            double ADouble,
            bool ALittleEndian = true)
        {
            if (AByteStream == null)
                throw new ArgumentNullException("AByteStream");

            byte[] aWrite = BitConverter.GetBytes(ADouble);

            if (BitConverter.IsLittleEndian != ALittleEndian)
                Array.Reverse(aWrite);

            AByteStream.WriteBytes(aWrite);
        }

        public static void WriteUtf8Zt(
            this Stream AByteStream,
            string AString)
        {
            if (AByteStream == null)
                throw new ArgumentNullException("AByteStream");
            if (AString == null)
                throw new ArgumentNullException("AString");

            if (AString.Contains(Convert.ToChar(0x00)))
                throw new ArgumentException("Input string contains the 0x00 character.");

            byte[] aWrite = Encoding.UTF8.GetBytes(AString);
            AByteStream.WriteBytes(aWrite);
            AByteStream.WriteByte(0x00);
        }

        public static void WriteAscii(
            this Stream AByteStream,
            string AString,
            int ALength)
        {
            if (AByteStream == null)
                throw new ArgumentNullException("AByteStream");

            if (ALength < 0)
                throw new ArgumentOutOfRangeException("ALength");
            if (AString.Any(AChar => Convert.ToUInt32(AChar) > 0xEF))
                throw new ArgumentException("String contains invalid characters.");

            int iDiff = AString.Length - ALength;
            if (iDiff > 0)
                AString = AString.Substring(0, ALength);

            while (iDiff < 0)
            {
                AString += "\x00";
                iDiff++;
            }

            byte[] aBytes = Encoding.ASCII.GetBytes(AString);
            AByteStream.WriteBytes(aBytes);
        }
        public static void WriteAsciiZt(
            this Stream AByteStream,
            string AString)
        {
            if (AString == null)
                throw new ArgumentNullException("AString");
            if (AString == null)
                throw new ArgumentNullException("AString");

            if (AString.Contains(Convert.ToChar(0x00)))
                throw new ArgumentException("Input string contains the 0x00 character.");
            if (AString.Any(AChar => Convert.ToUInt32(AChar) > 0xEF))
                throw new ArgumentException("String contains invalid characters.");

            byte[] aWrite = Encoding.ASCII.GetBytes(AString);
            AByteStream.WriteBytes(aWrite);
            AByteStream.WriteByte(0x00);
        }
    }
}
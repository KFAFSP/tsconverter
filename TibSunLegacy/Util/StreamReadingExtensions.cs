using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using JetBrains.Annotations;

namespace TibSunLegacy.Util
{
    public static class StreamReadingExtensions
    {
        [NotNull]
        public static byte[] ReadBytes(
            this Stream AByteStream,
            uint ACount)
        {
            if (AByteStream == null)
                throw new ArgumentNullException("AByteStream");

            byte[] aResult = new byte[ACount];

            for (uint I = 0; I < ACount; I++)
            {
                int iRead = AByteStream.ReadByte();
                if (iRead == -1)
                    throw new EndOfStreamException();

                aResult[I] = (byte)iRead;
            }

            return aResult;
        }

        public static byte SafeReadByte(
            this Stream AByteStream)
        {
            int iRead = AByteStream.ReadByte();
            if (iRead == -1)
                throw new EndOfStreamException();

            return (byte)iRead;
        }
        public static bool SafeReadExact(
            this Stream AByteStream,
            byte AValue)
        {
            return AByteStream.SafeReadByte() == AValue;
        }

        public static void Skip(
            this Stream AByteStream,
            int ACount)
        {
            if (AByteStream == null)
                throw new ArgumentNullException("AByteStream");

            if (AByteStream.CanSeek)
                AByteStream.Seek(ACount, SeekOrigin.Current);
            else
                for (int I = 0; I < ACount; I++)
                    AByteStream.SafeReadByte();
        }

        public static ushort ReadUInt16(
            this Stream AByteStream,
            bool ALittleEndian = true)
        {
            if (AByteStream == null)
                throw new ArgumentNullException("AByteStream");

            byte[] aRead = AByteStream.ReadBytes(2);

            if (BitConverter.IsLittleEndian != ALittleEndian)
                Array.Reverse(aRead);

            return BitConverter.ToUInt16(aRead, 0);
        }
        public static uint ReadUInt32(
            this Stream AByteStream,
            bool ALittleEndian = true)
        {
            if (AByteStream == null)
                throw new ArgumentNullException("AByteStream");

            byte[] aRead = AByteStream.ReadBytes(4);

            if (BitConverter.IsLittleEndian != ALittleEndian)
                Array.Reverse(aRead);

            return BitConverter.ToUInt32(aRead, 0);
        }
        public static ulong ReadUInt64(
            this Stream AByteStream,
            bool ALittleEndian = true)
        {
            if (AByteStream == null)
                throw new ArgumentNullException("AByteStream");

            byte[] aRead = AByteStream.ReadBytes(8);

            if (BitConverter.IsLittleEndian != ALittleEndian)
                Array.Reverse(aRead);

            return BitConverter.ToUInt64(aRead, 0);
        }

        public static short ReadInt16(
            this Stream AByteStream,
            bool ALittleEndian = true)
        {
            if (AByteStream == null)
                throw new ArgumentNullException("AByteStream");

            byte[] aRead = AByteStream.ReadBytes(2);

            if (BitConverter.IsLittleEndian != ALittleEndian)
                Array.Reverse(aRead);

            return BitConverter.ToInt16(aRead, 0);
        }
        public static int ReadInt32(
            this Stream AByteStream,
            bool ALittleEndian = true)
        {
            if (AByteStream == null)
                throw new ArgumentNullException("AByteStream");

            byte[] aRead = AByteStream.ReadBytes(4);

            if (BitConverter.IsLittleEndian != ALittleEndian)
                Array.Reverse(aRead);

            return BitConverter.ToInt32(aRead, 0);
        }
        public static long ReadInt64(
            this Stream AByteStream,
            bool ALittleEndian = true)
        {
            if (AByteStream == null)
                throw new ArgumentNullException("AByteStream");

            byte[] aRead = AByteStream.ReadBytes(8);

            if (BitConverter.IsLittleEndian != ALittleEndian)
                Array.Reverse(aRead);

            return BitConverter.ToInt64(aRead, 0);
        }

        public static float ReadFloat(
            this Stream AByteStream,
            bool ALittleEndian = true)
        {
            if (AByteStream == null)
                throw new ArgumentNullException("AByteStream");

            byte[] aRead = AByteStream.ReadBytes(4);

            if (BitConverter.IsLittleEndian != ALittleEndian)
                Array.Reverse(aRead);

            return BitConverter.ToSingle(aRead, 0);
        }
        public static double ReadDouble(
            this Stream AByteStream,
            bool ALittleEndian = true)
        {
            if (AByteStream == null)
                throw new ArgumentNullException("AByteStream");

            byte[] aRead = AByteStream.ReadBytes(8);

            if (BitConverter.IsLittleEndian != ALittleEndian)
                Array.Reverse(aRead);

            return BitConverter.ToDouble(aRead, 0);
        }

        [NotNull]
        public static string ReadUtf8Zt(
            this Stream AByteStream,
            int ALimit = ushort.MaxValue)
        {
            if (AByteStream == null)
                throw new ArgumentNullException("AByteStream");

            List<byte> lResult = new List<byte>();

            do
            {
                int iRead = AByteStream.ReadByte();
                if (iRead == -1)
                    throw new EndOfStreamException();

                byte bRead = (byte)iRead;
                if (bRead == 0x00)
                    break;

                lResult.Add(bRead);
            } while (lResult.Count < ALimit);

            return Encoding.UTF8.GetString(lResult.ToArray());
        }
        
        [NotNull]
        public static string ReadAscii(
            this Stream AByteStream,
            int ALength,
            bool ATrimZeroes = true)
        {
            if (AByteStream == null)
                throw new ArgumentNullException("AByteStream");

            if (ALength < 0)
                throw new ArgumentOutOfRangeException("ALength");

            string sRead = Encoding.ASCII.GetString(AByteStream.ReadBytes((uint)ALength));

            if (ATrimZeroes)
            {
                int iIndex = sRead.IndexOf("\x00", StringComparison.InvariantCulture);
                if (iIndex != -1)
                    sRead = sRead.Substring(0, iIndex);
            }

            return sRead;
        }
        [NotNull]
        public static string ReadAsciiZt(
            this Stream AByteStream,
            int ALimit = ushort.MaxValue)
        {
            if (AByteStream == null)
                throw new ArgumentNullException("AByteStream");

            List<byte> lResult = new List<byte>();

            do
            {
                int iRead = AByteStream.ReadByte();
                if (iRead == -1)
                    throw new EndOfStreamException();

                byte bRead = (byte)iRead;
                if (bRead == 0x00)
                    break;

                lResult.Add(bRead);
            } while (lResult.Count < ALimit);

            return Encoding.ASCII.GetString(lResult.ToArray());
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace CarAssist.Obd
{
    public static class Converter
    {
        static public void DecodeSupportedPids(byte[] data, int offset, IList<int> supportedPids)
        {
            if (data == null)
                return;

            for (int byteIdx = 0; byteIdx < data.Length; byteIdx++)
            {
                for (int i = 0; i < 8; i++)
                {
                    if ((data[byteIdx] << i & 0x80) == 0x80)
                        supportedPids.Add(byteIdx * 8 + i + 1 + offset);
                }
            }
        }

        static public string DecodeSupportedPids(int pids, int offset, IList<int> supportedPids)
        {
            Converter.DecodeSupportedPids(Converter.HexStringToByteArray(Convert.ToString((int)pids, 16)), offset, supportedPids);

            return string.Join(", ", supportedPids);
        }

        public static byte[] HexStringToByteArray(string hex)
        {
            if (hex.Length % 2 != 0)
                hex = hex.PadLeft(hex.Length+1, '0');
            if (string.IsNullOrEmpty(hex))
                return null;

            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        public static string ToBinary(int value)
        {
            return Convert.ToString(value, 2);
        }

        public static UInt32 BitReverse(UInt32 value)
        {
            UInt32 left = (UInt32)1 << 31;
            UInt32 right = 1;
            UInt32 result = 0;

            for (int i = 31; i >= 1; i -= 2)
            {
                result |= (value & left) >> i;
                result |= (value & right) << i;
                left >>= 1;
                right <<= 1;
            }
            return result;
        }
    }
}

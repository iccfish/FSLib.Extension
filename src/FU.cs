using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
	/// <summary>
	/// FishUtility
	/// </summary>
	public static class FU
	{
		/// <summary>
		/// 转换十六进制字符为对应的数据
		/// </summary>
		/// <param name="byteChar">0-9, A-F, a-f</param>
		/// <returns></returns>
		public static byte ConvertHexCharToByte(char byteChar)
		{
			if (byteChar >= '0' && byteChar <= '9') return (byte)(byteChar - '0');
			if (byteChar >= 'A' && byteChar <= 'F') return (byte)(byteChar - 'A' + 10);
			if (byteChar >= 'a' && byteChar <= 'f') return (byte)(byteChar - 'a' + 10);

			throw new ArgumentOutOfRangeException();
		}

		/// <summary>
		/// 转换十六进制字符串为数组
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static byte[] ConvertHexStringToBytes(string str)
		{
			var buffer = new byte[str.Length / 2];
			for (int i = 0; i < buffer.Length; i++)
			{
				buffer[i] = (byte)((ConvertHexCharToByte(str[i * 2]) << 4) + ConvertHexCharToByte(str[i * 2 + 1]));
			}

			return buffer;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSLib.Extension
{
	/// <summary>
	/// 二进制辅助类
	/// </summary>
	public class BinaryUtil
	{
		/// <summary>
		/// 将两个字节转换为一个byte表示
		/// </summary>
		/// <param name="c1"></param>
		/// <param name="c2"></param>
		/// <returns></returns>
		public static byte ByteFromChar(char c1, char c2) => (byte)(c1.ToHexByte() << 4 | c2.ToHexByte());

		/// <summary>
		/// 将一个十六进制字符串转换为字节数组
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static byte[] BytesFromHexString(string s)
		{
			var buffer = new byte[s.Length / 2];
			var ca     = s.ToCharArray();

			for (int i = 0; i < buffer.Length; i++)
			{
				buffer[i] = ByteFromChar(ca[i * 2], ca[i * 2 + 1]);
			}

			return buffer;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSLib.Extension.FishLib
{
	/// <summary>
	/// 二进制相关辅助工具
	/// </summary>
	public class HexUtils
	{
		/// <summary>
		/// 将十六进制 <see cref="char"/> 转换为对应的数值
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public static byte ByteFromChar(char c) => (byte)(c >= 'a' ? c - 'a' + 10 : c >= 'A' ? c - 'A' + 10 : c - '0');

		/// <summary>
		/// 将两位十六进制 <see cref="char"/> 转换为对应的数值
		/// </summary>
		/// <param name="c1"></param>
		/// <param name="c2"></param>
		/// <returns></returns>
		public static byte ByteFromChar(char c1, char c2) => (byte)(ByteFromChar(c1) << 4 | ByteFromChar(c2));

		/// <summary>
		/// 将十六进制字符串转成十六进制数组
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static byte[] BytesFromHexString(string s)
		{
			var buffer = new byte[s.Length / 2];
			var ca = s.ToCharArray();

			for (int i = 0; i < buffer.Length; i++)
			{
				buffer[i] = ByteFromChar(ca[i * 2], ca[i * 2 + 1]);
			}

			return buffer;
		}

		/// <summary>
		/// 将byte数组转换为十六进制字符串
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public static string BytesToHexString(byte[] data)
		{
			var sb = new StringBuilder(data.Length * 2);
			foreach (var b in data)
			{
				sb.Append(b.ToString("x2"));
			}

			return sb.ToString();
		}
	}
}

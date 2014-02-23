using System.Diagnostics;
using System.Data;
using System.Collections;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System;
using System.IO;

namespace System
{
	
}

namespace FSLib.IO.SerializeHelper
{
	/// <summary>
	/// 二进制序列化辅助类
	/// </summary>
	public static class BinarySerializeHelper
	{

		/// <summary>
		/// 从文件中反序列化对象
		/// </summary>
		/// <param name="FileName">文件名</param>
		/// <returns>原对象</returns>
		public static object DeserializeFromFile(string FileName)
		{
			using (FileStream stream = new FileStream(FileName, FileMode.Open))
			{
				object res = stream.DeserializeFromStream();
				stream.Close();
				return res;
			}
		}

		/// <summary>
		/// 从字节数组中反序列化
		/// </summary>
		/// <param name="array">字节数组</param>
		/// <returns>序列化结果</returns>
		public static object DeserialzieFromBytes(byte[] array)
		{
			object result = null;
			if (array == null || array.Length == 0)
				return result;

			using (var ms=new System.IO.MemoryStream())
			{
				ms.Write(array, 0, array.Length);
				ms.Seek(0, SeekOrigin.Begin);

				result = ms.DeserializeFromStream();
				ms.Close();
			}

			return result;
		}
	}
}

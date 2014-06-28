using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSLib.Data
{
	using System.Data;
	using System.IO;
	using System.IO.Compression;

	public static class DataSetExtension
	{		/// <summary>
		/// 将DataSet转换成压缩的字节数组
		/// </summary>
		/// <param name="dataset"></param>
		/// <returns></returns>
		public static byte[] ToCompressedByteBuffer(this DataSet dataset)
		{
			if (dataset == null)
				return null;

			using (var ms = new MemoryStream())
			{
				using (var gzs = new GZipStream(ms, CompressionMode.Compress))
				{
					dataset.WriteXml(gzs);
				}
				return ms.ToArray();
			}
		}

		/// <summary>
		/// 从指定的缓冲字节数组中读取DataSet
		/// </summary>
		/// <param name="dataset"></param>
		/// <param name="buffer"></param>
		/// <returns></returns>
		public static DataSet ReadFromCompressedByteBuffer(this DataSet dataset, byte[] buffer)
		{
			dataset = dataset ?? new DataSet();

			using (var ms=new MemoryStream(buffer))
			{
				using (var gzs=new GZipStream(ms, CompressionMode.Decompress))
				{
					dataset.ReadXml(gzs);
				}
			}

			return dataset;
		}

	}
}

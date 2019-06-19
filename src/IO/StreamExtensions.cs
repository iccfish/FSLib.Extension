using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// ReSharper disable once CheckNamespace
namespace System.IO
{
	using ComponentModel;

	using Compression;

	using FSLib.Extension;

#if !NET20 && !NET35
	using System.Threading.Tasks;

	using Threading;

#endif


	/// <summary>
	/// 针对 <see cref="System.IO.Stream"/> 的扩展方法
	/// </summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static class FishStreamExtensions
	{
		/// <summary>
		/// 从流中读入一个 <see cref="T:System.Int16"/>
		/// </summary>
		/// <param name="stream">要读取的流</param>
		/// <returns>读取的 <see cref="T:System.Int16"/></returns>
		public static ushort ReadUInt16(this System.IO.Stream stream)
		{
			return BitConverter.ToUInt16(stream.ReadBuffer(2), 0);
		}

		/// <summary>
		/// 从流中读入一个 <see cref="T:System.Int32"/>
		/// </summary>
		/// <param name="stream">要读取的流</param>
		/// <returns>读取的 <see cref="T:System.Int32"/></returns>
		public static uint ReadUInt32(this Stream stream)
		{
			return BitConverter.ToUInt32(stream.ReadBuffer(4), 0);
		}

		/// <summary>
		/// 从流中读入一个 <see cref="T:System.Int64"/>
		/// </summary>
		/// <param name="stream">要读取的流</param>
		/// <returns>读取的 <see cref="T:System.Int64"/></returns>
		public static ulong ReadUInt64(this Stream stream)
		{
			return BitConverter.ToUInt64(stream.ReadBuffer(8), 0);
		}

		/// <summary>
		/// 从流中读入一个 <see cref="T:System.Int16"/>
		/// </summary>
		/// <param name="stream">要读取的流</param>
		/// <returns>读取的 <see cref="T:System.Int16"/></returns>
		public static short ReadInt16(this System.IO.Stream stream)
		{
			return BitConverter.ToInt16(stream.ReadBuffer(2), 0);
		}

		/// <summary>
		/// 从流中读入一个 <see cref="T:System.Int32"/>
		/// </summary>
		/// <param name="stream">要读取的流</param>
		/// <returns>读取的 <see cref="T:System.Int32"/></returns>
		public static int ReadInt32(this Stream stream)
		{
			return BitConverter.ToInt32(stream.ReadBuffer(4), 0);
		}

		/// <summary>
		/// 从流中读入一个 <see cref="T:System.Int64"/>
		/// </summary>
		/// <param name="stream">要读取的流</param>
		/// <returns>读取的 <see cref="T:System.Int64"/></returns>
		public static long ReadInt64(this Stream stream)
		{
			return BitConverter.ToInt64(stream.ReadBuffer(8), 0);
		}

		/// <summary>
		/// 从流中读入一个 <see cref="T:System.Int64"/>
		/// </summary>
		/// <param name="stream">要读取的流</param>
		/// <returns>读取的 <see cref="T:System.Int64"/></returns>
		public static double ReadDouble(this Stream stream)
		{
			return BitConverter.ToDouble(stream.ReadBuffer(sizeof(double)), 0);
		}

		/// <summary>
		/// 从流中读入一个缓冲数组
		/// </summary>
		/// <param name="stream">要读取的流</param>
		/// <param name="length">读取的字节长度</param>
		/// <param name="lengthRequired">是否要求指定的字节数，如果读取不到，则抛出异常</param>
		/// <returns>缓冲数组</returns>
		public static byte[] ReadBuffer(this Stream stream, int length, bool lengthRequired = true)
		{
			var count = 0;


			var buffer = stream.ReadBuffer(length, out count);
			if (count < length && lengthRequired)
				throw new EndOfStreamException();

			return buffer;
		}

		/// <summary>
		/// 从流中读入一个缓冲数组
		/// </summary>
		/// <param name="stream">要读取的流</param>
		/// <param name="length">读取的字节长度</param>
		/// <param name="readedBytesCount">返回已经读取到的长度</param>
		/// <returns>缓冲数组</returns>
		public static byte[] ReadBuffer(this Stream stream, int length, out int readedBytesCount)
		{
			var buffer = new byte[length];
			var count = 0;
			readedBytesCount = 0;

			while (readedBytesCount < length && (count = stream.Read(buffer, readedBytesCount, buffer.Length - readedBytesCount)) > 0)
			{
				readedBytesCount += count;
			}

			return buffer;
		}

		/// <summary>
		/// 从流中读入一个缓冲数组
		/// </summary>
		/// <param name="stream">要读取的流</param>
		/// <param name="buffer">缓冲数组</param>
		/// <returns>缓冲数组</returns>
		[Obsolete("此函数没有很大的意义，将会被移除。This method will be removed soon due to no means.")]
		public static byte[] FillBuffer(this Stream stream, byte[] buffer)
		{
			if (stream.Read(buffer, 0, buffer.Length) != buffer.Length)
			{
				throw new Exception();
			}

			return buffer;
		}

		/// <summary>
		/// 读取所有的数据到内存流中
		/// </summary>
		/// <param name="stream">要读取的流</param>
		/// <param name="readBufferSize">读取的缓冲区长度，默认为 4KB</param>
		/// <returns>包含所有数据的 <see cref="MemoryStream"/> </returns>
		[CanBeNull]
		public static MemoryStream ReadToEnd([NotNull] this Stream stream, int readBufferSize = 0x400)
		{
			if (!stream.CanRead)
				return null;

			var end = stream as MemoryStream;
			if (end != null)
				return end;

			var ms = new MemoryStream();
			var buffer = new byte[readBufferSize];
			var count = 0;
			while ((count = stream.Read(buffer, 0, buffer.Length)) > 0)
				ms.Write(buffer, 0, count);
			ms.Flush();
			ms.Seek(0, SeekOrigin.Begin);

			return ms;
		}


		/// <summary>
		/// 压缩原始流
		/// </summary>
		/// <param name="stream">要写入的目标流</param>
		/// <returns>供写入的压缩流</returns>
		public static Stream Zip(this Stream stream)
		{
			return new System.IO.Compression.GZipStream(stream, CompressionMode.Compress);
		}

		/// <summary>
		/// 解压缩原始流
		/// </summary>
		/// <param name="stream">供读取的压缩流</param>
		/// <returns>供读取的解压缩流</returns>
		public static Stream UnZip(this Stream stream)
		{
			return new GZipStream(stream, CompressionMode.Decompress);
		}

		/// <summary>
		/// 获得当前流位置的显示字符串格式
		/// </summary>
		/// <param name="stream"></param>
		/// <returns></returns>
		public static string GetPositionString(this Stream stream)
		{
			return string.Format("0x{0:X8}", stream.Position);
		}

		/// <summary>
		/// 将指定的缓冲数组全部写入流中
		/// </summary>
		/// <param name="stream">目标流</param>
		/// <param name="buffer">缓冲数组</param>
		public static T Write<T>(this T stream, byte[] buffer) where T : Stream
		{
			stream.Write(buffer, 0, buffer.Length);
			return stream;
		}

		/// <summary>
		/// 将指定的缓冲数组全部写入流中
		/// </summary>
		/// <param name="stream">目标流</param>
		/// <param name="buffer">缓冲数组</param>
		public static T Write<T>(this T stream, IEnumerable<byte> buffer) where T : Stream
		{
			stream.Write(buffer.ToArray());
			return stream;
		}

		/// <summary>
		/// 将目标值写入流中
		/// </summary>
		/// <param name="stream">当前流</param>
		/// <param name="value">值</param>
		public static T Write<T>(this T stream, int value) where T : Stream
		{
			stream.Write(BitConverter.GetBytes(value));
			return stream;
		}

		/// <summary>
		/// 将目标值写入流中
		/// </summary>
		/// <param name="stream">当前流</param>
		/// <param name="value">值</param>
		public static T Write<T>(this T stream, uint value) where T : Stream
		{
			stream.Write(BitConverter.GetBytes(value));
			return stream;
		}

		/// <summary>
		/// 将目标值写入流中
		/// </summary>
		/// <param name="stream">当前流</param>
		/// <param name="value">值</param>
		public static T Write<T>(this T stream, short value) where T : Stream
		{
			stream.Write(BitConverter.GetBytes(value));
			return stream;
		}

		/// <summary>
		/// 将目标值写入流中
		/// </summary>
		/// <param name="stream">当前流</param>
		/// <param name="value">值</param>
		public static T Write<T>(this T stream, ushort value) where T : Stream
		{
			stream.Write(BitConverter.GetBytes(value));
			return stream;
		}

		/// <summary>
		/// 将目标值写入流中
		/// </summary>
		/// <param name="stream">当前流</param>
		/// <param name="value">值</param>
		public static T Write<T>(this T stream, long value) where T : Stream
		{
			stream.Write(BitConverter.GetBytes(value));
			return stream;
		}

		/// <summary>
		/// 将目标值写入流中
		/// </summary>
		/// <param name="stream">当前流</param>
		/// <param name="value">值</param>
		public static T Write<T>(this T stream, ulong value) where T : Stream
		{
			stream.Write(BitConverter.GetBytes(value));
			return stream;
		}

#if !NET20 && !NET35 && !NET40

		/// <summary>
		/// 从流中读入一个 <see cref="T:System.Int16"/>
		/// </summary>
		/// <param name="stream">要读取的流</param>
		/// <param name="cancellationToken">取消操作的TOKEN</param>
		/// <returns>读取的 <see cref="T:System.Int16"/></returns>
		public static async Task<ushort> ReadUInt16Async(this System.IO.Stream stream, CancellationToken? cancellationToken = null)
		{
			return BitConverter.ToUInt16(await stream.ReadBufferAsync(2, cancellationToken), 0);
		}

		/// <summary>
		/// 从流中读入一个 <see cref="T:System.Int32"/>
		/// </summary>
		/// <param name="stream">要读取的流</param>
		/// <param name="cancellationToken">取消操作的TOKEN</param>
		/// <returns>读取的 <see cref="T:System.Int32"/></returns>
		public static async Task<uint> ReadUInt32Async(this Stream stream, CancellationToken? cancellationToken = null)
		{
			return BitConverter.ToUInt32(await stream.ReadBufferAsync(4, cancellationToken), 0);
		}

		/// <summary>
		/// 从流中读入一个 <see cref="T:System.Int64"/>
		/// </summary>
		/// <param name="stream">要读取的流</param>
		/// <param name="cancellationToken">取消操作的TOKEN</param>
		/// <returns>读取的 <see cref="T:System.Int64"/></returns>
		public static async Task<ulong> ReadUInt64Async(this Stream stream, CancellationToken? cancellationToken = null)
		{
			return BitConverter.ToUInt64(await stream.ReadBufferAsync(8, cancellationToken), 0);
		}

		/// <summary>
		/// 从流中读入一个 <see cref="T:System.Int16"/>
		/// </summary>
		/// <param name="cancellationToken">取消操作的TOKEN</param>
		/// <param name="stream">要读取的流</param>
		/// <returns>读取的 <see cref="T:System.Int16"/></returns>
		public static async Task<short> ReadInt16Async(this System.IO.Stream stream, CancellationToken? cancellationToken = null)
		{
			return BitConverter.ToInt16(await stream.ReadBufferAsync(2, cancellationToken), 0);
		}

		/// <summary>
		/// 从流中读入一个 <see cref="T:System.Int32"/>
		/// </summary>
		/// <param name="stream">要读取的流</param>
		/// <param name="cancellationToken">取消操作的TOKEN</param>
		/// <returns>读取的 <see cref="T:System.Int32"/></returns>
		public static async Task<int> ReadInt32Async(this Stream stream, CancellationToken? cancellationToken = null)
		{
			return BitConverter.ToInt32(await stream.ReadBufferAsync(4, cancellationToken), 0);
		}

		/// <summary>
		/// 从流中读入一个 <see cref="T:System.Int64"/>
		/// </summary>
		/// <param name="stream">要读取的流</param>
		/// <param name="cancellationToken">取消操作的TOKEN</param>
		/// <returns>读取的 <see cref="T:System.Int64"/></returns>
		public static async Task<long> ReadInt64Async(this Stream stream, CancellationToken? cancellationToken = null)
		{
			return BitConverter.ToInt64(await stream.ReadBufferAsync(8, cancellationToken), 0);
		}

		/// <summary>
		/// 从流中读入一个 <see cref="T:System.Int64"/>
		/// </summary>
		/// <param name="stream">要读取的流</param>
		/// <param name="cancellationToken">取消操作的TOKEN</param>
		/// <returns>读取的 <see cref="T:System.Int64"/></returns>
		public static async Task<double> ReadDoubleAsync(this Stream stream, CancellationToken? cancellationToken = null)
		{
			return BitConverter.ToDouble(await stream.ReadBufferAsync(sizeof(double), cancellationToken), 0);
		}

		/// <summary>
		/// 从流中读入一个缓冲数组
		/// </summary>
		/// <param name="stream">要读取的流</param>
		/// <param name="length">读取的字节长度</param>
		/// <param name="cancellationToken">取消操作的TOKEN</param>
		/// <returns>缓冲数组</returns>
		public static async Task<byte[]> ReadBufferAsync(this Stream stream, int length, CancellationToken? cancellationToken = null)
		{
			var (buffer, _) = await stream.ReadBufferAsync(length, true, cancellationToken);
			return buffer;
		}

		/// <summary>
		/// 从流中读入一个缓冲数组
		/// </summary>
		/// <param name="stream">要读取的流</param>
		/// <param name="length">读取的字节长度</param>
		/// <param name="cancellationToken">取消操作的TOKEN</param>
		/// <param name="lengthRequired">是否要求指定的字节数，如果读取不到，则抛出异常</param>
		/// <returns>缓冲数组</returns>
		public static async Task<(byte[] buffer, int count)> ReadBufferAsync(this Stream stream, int length, bool lengthRequired = true, CancellationToken? cancellationToken = null)
		{
			var buffer = new byte[length];
			var count = await stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken ?? CancellationToken.None);
			if (count < length && lengthRequired)
				throw new EndOfStreamException();

			return (buffer, count);
		}

		/// <summary>
		/// 读取所有的数据到内存流中
		/// </summary>
		/// <param name="stream">要读取的流</param>
		/// <param name="readBufferSize">读取的缓冲区长度，默认为 4KB</param>
		/// <param name="cancellationToken">取消操作的TOKEN</param>
		/// <returns>包含所有数据的 <see cref="MemoryStream"/> </returns>
		[CanBeNull]
		public static async Task<MemoryStream> ReadToEndAsync([NotNull] this Stream stream, int readBufferSize = 0x400, CancellationToken? cancellationToken = null)
		{
			if (!stream.CanRead)
				return null;

			if (stream is MemoryStream end)
				return end;

			var ms = new MemoryStream();
			var buffer = new byte[readBufferSize];
			var token = cancellationToken ?? CancellationToken.None;
			int count;
			while ((count = await stream.ReadAsync(buffer, 0, buffer.Length, token)) > 0)
				await ms.WriteAsync(buffer, 0, count, token);
			ms.Flush();
			ms.Seek(0, SeekOrigin.Begin);

			return ms;
		}


		/// <summary>
		/// 将指定的缓冲数组全部写入流中
		/// </summary>
		/// <param name="stream">目标流</param>
		/// <param name="cancellationToken">取消操作的TOKEN</param>
		/// <param name="buffer">缓冲数组</param>
		public static async Task<T> WriteAsync<T>(this T stream, byte[] buffer, int? length = null, CancellationToken? cancellationToken = null) where T : Stream
		{
			await stream.WriteAsync(buffer, 0, length ?? buffer.Length, cancellationToken ?? CancellationToken.None);
			return stream;
		}

		/// <summary>
		/// 将指定的缓冲数组全部写入流中
		/// </summary>
		/// <param name="cancellationToken">取消操作的TOKEN</param>
		/// <param name="stream">目标流</param>
		/// <param name="buffer">缓冲数组</param>
		public static async Task<T> WriteAsync<T>(this T stream, IEnumerable<byte> buffer, CancellationToken? cancellationToken = null) where T : Stream
		{
			var buf = buffer.ToArray();
			await stream.WriteAsync(buf, 0, buf.Length, cancellationToken ?? CancellationToken.None);
			return stream;
		}

		/// <summary>
		/// 将目标值写入流中
		/// </summary>
		/// <param name="stream">当前流</param>
		/// <param name="cancellationToken">取消操作的TOKEN</param>
		/// <param name="value">值</param>
		public static async Task<T> WriteAsync<T>(this T stream, int value, CancellationToken? cancellationToken = null) where T : Stream
		{
			await stream.WriteAsync(BitConverter.GetBytes(value), cancellationToken);
			return stream;
		}

		/// <summary>
		/// 将目标值写入流中
		/// </summary>
		/// <param name="stream">当前流</param>
		/// <param name="cancellationToken">取消操作的TOKEN</param>
		/// <param name="value">值</param>
		public static async Task<T> WriteAsync<T>(this T stream, uint value, CancellationToken? cancellationToken = null) where T : Stream
		{
			await stream.WriteAsync(BitConverter.GetBytes(value), cancellationToken);
			return stream;
		}

		/// <summary>
		/// 将目标值写入流中
		/// </summary>
		/// <param name="stream">当前流</param>
		/// <param name="cancellationToken">取消操作的TOKEN</param>
		/// <param name="value">值</param>
		public static async Task<T> WriteAsync<T>(this T stream, short value, CancellationToken? cancellationToken = null) where T : Stream
		{
			await stream.WriteAsync(BitConverter.GetBytes(value), cancellationToken);
			return stream;
		}

		/// <summary>
		/// 将目标值写入流中
		/// </summary>
		/// <param name="stream">当前流</param>
		/// <param name="cancellationToken">取消操作的TOKEN</param>
		/// <param name="value">值</param>
		public static async Task<T> WriteAsync<T>(this T stream, ushort value, CancellationToken? cancellationToken = null) where T : Stream
		{
			await stream.WriteAsync(BitConverter.GetBytes(value), cancellationToken);
			return stream;
		}

		/// <summary>
		/// 将目标值写入流中
		/// </summary>
		/// <param name="cancellationToken">取消操作的TOKEN</param>
		/// <param name="stream">当前流</param>
		/// <param name="value">值</param>
		public static async Task<T> WriteAsync<T>(this T stream, long value, CancellationToken? cancellationToken = null) where T : Stream
		{
			await stream.WriteAsync(BitConverter.GetBytes(value), cancellationToken);
			return stream;
		}

		/// <summary>
		/// 将目标值写入流中
		/// </summary>
		/// <param name="stream">当前流</param>
		/// <param name="cancellationToken">取消操作的TOKEN</param>
		/// <param name="value">值</param>
		public static async Task<T> WriteAsync<T>(this T stream, ulong value, CancellationToken? cancellationToken = null) where T : Stream
		{
			await stream.WriteAsync(BitConverter.GetBytes(value), cancellationToken);
			return stream;
		}
#endif
	}
}

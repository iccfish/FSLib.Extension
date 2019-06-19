using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.IO
{
	using System.Runtime.InteropServices.ComTypes;
	using System.Threading;
#if NET40 || NET45 || NET46 || NET47 || NETSTANDARD1_6_1 || NETSTANDARD2_0 || NETSTANDARD3_0
	using Threading.Tasks;
#endif

	/// <summary>
	/// 包装现有Stream以提供进度变化通知的类
	/// </summary>
	public class StreamWithEventsWrapper : Stream
	{
		Stream _innerStream;

		/// <summary>
		/// 位置发生变化
		/// </summary>
		public event EventHandler PositionChanged;

		/// <summary>
		/// 引发 <see cref="PositionChanged" /> 事件
		/// </summary>
		protected virtual void OnPositionChanged()
		{
			var handler = PositionChanged;
			if (handler != null)
				handler(this, EventArgs.Empty);
		}

		/// <summary>
		/// 创建 <see cref="StreamWithEventsWrapper" />  的新实例(StreamWithEventsWrapper)
		/// </summary>
		/// <param name="innerStream"></param>
		public StreamWithEventsWrapper(Stream innerStream)
		{
			_innerStream = innerStream;
		}

		/// <summary>
		/// 当在派生类中重写时，将清除该流的所有缓冲区，并使得所有缓冲数据被写入到基础设备。
		/// </summary>
		/// <exception cref="T:System.IO.IOException">发生 I/O 错误。</exception>
		public override void Flush()
		{
			_innerStream.Flush();
		}

		/// <summary>
		/// 当在派生类中重写时，设置当前流中的位置。
		/// </summary>
		/// <returns>
		/// 当前流中的新位置。
		/// </returns>
		/// <param name="offset">相对于 <paramref name="origin"/> 参数的字节偏移量。</param><param name="origin"><see cref="T:System.IO.SeekOrigin"/> 类型的值，指示用于获取新位置的参考点。</param><exception cref="T:System.IO.IOException">发生 I/O 错误。</exception><exception cref="T:System.NotSupportedException">流不支持查找，例如在流通过管道或控制台输出构造的情况下即为如此。</exception><exception cref="T:System.ObjectDisposedException">在流关闭后调用方法。</exception>
		public override long Seek(long offset, SeekOrigin origin)
		{
			var pos = _innerStream.Seek(offset, origin);
			OnPositionChanged();
			return pos;
		}

		/// <summary>
		/// 当在派生类中重写时，设置当前流的长度。
		/// </summary>
		/// <param name="value">所需的当前流的长度（以字节表示）。</param><exception cref="T:System.IO.IOException">发生 I/O 错误。</exception><exception cref="T:System.NotSupportedException">流不支持写入和查找，例如在流通过管道或控制台输出构造的情况下即为如此。</exception><exception cref="T:System.ObjectDisposedException">在流关闭后调用方法。</exception>
		public override void SetLength(long value)
		{
			_innerStream.SetLength(value);
			OnPositionChanged();
		}

		/// <summary>
		/// 当在派生类中重写时，从当前流读取字节序列，并将此流中的位置提升读取的字节数。
		/// </summary>
		/// <returns>
		/// 读入缓冲区中的总字节数。如果当前可用的字节数没有请求的字节数那么多，则总字节数可能小于请求的字节数；如果已到达流的末尾，则为零 (0)。
		/// </returns>
		/// <param name="buffer">字节数组。此方法返回时，该缓冲区包含指定的字符数组，该数组的 <paramref name="offset"/> 和 (<paramref name="offset"/> + <paramref name="count"/> -1) 之间的值由从当前源中读取的字节替换。</param><param name="offset"><paramref name="buffer"/> 中的从零开始的字节偏移量，从此处开始存储从当前流中读取的数据。</param><param name="count">要从当前流中最多读取的字节数。</param><exception cref="T:System.ArgumentException"><paramref name="offset"/> 与 <paramref name="count"/> 的和大于缓冲区长度。</exception><exception cref="T:System.ArgumentNullException"><paramref name="buffer"/> 为 null。</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="offset"/> 或 <paramref name="count"/> 为负。</exception><exception cref="T:System.IO.IOException">发生 I/O 错误。</exception><exception cref="T:System.NotSupportedException">流不支持读取。</exception><exception cref="T:System.ObjectDisposedException">在流关闭后调用方法。</exception>
		public override int Read(byte[] buffer, int offset, int count)
		{
			var readcount = _innerStream.Read(buffer, offset, count);
			if (readcount > 0)
				OnPositionChanged();
			return readcount;
		}

		/// <summary>
		/// 当在派生类中重写时，向当前流中写入字节序列，并将此流中的当前位置提升写入的字节数。
		/// </summary>
		/// <param name="buffer">字节数组。此方法将 <paramref name="count"/> 个字节从 <paramref name="buffer"/> 复制到当前流。</param><param name="offset"><paramref name="buffer"/> 中的从零开始的字节偏移量，从此处开始将字节复制到当前流。</param><param name="count">要写入当前流的字节数。</param><exception cref="T:System.ArgumentException"><paramref name="offset"/> 与 <paramref name="count"/> 的和大于缓冲区长度。</exception><exception cref="T:System.ArgumentNullException"><paramref name="buffer"/> 为 null。</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="offset"/> 或 <paramref name="count"/> 为负。</exception><exception cref="T:System.IO.IOException">发生 I/O 错误。</exception><exception cref="T:System.NotSupportedException">流不支持写入。</exception><exception cref="T:System.ObjectDisposedException">在流关闭后调用方法。</exception>
		public override void Write(byte[] buffer, int offset, int count)
		{
			_innerStream.Write(buffer, offset, count);
			OnPositionChanged();
		}

		/// <summary>
		/// 当在派生类中重写时，获取指示当前流是否支持读取的值。
		/// </summary>
		/// <returns>
		/// 如果流支持读取，为 true；否则为 false。
		/// </returns>
		public override bool CanRead
		{
			get { return _innerStream.CanRead; }
		}

		/// <summary>
		/// 当在派生类中重写时，获取指示当前流是否支持查找功能的值。
		/// </summary>
		/// <returns>
		/// 如果流支持查找，为 true；否则为 false。
		/// </returns>
		public override bool CanSeek
		{
			get { return _innerStream.CanSeek; }
		}

		/// <summary>
		/// 当在派生类中重写时，获取指示当前流是否支持写入功能的值。
		/// </summary>
		/// <returns>
		/// 如果流支持写入，为 true；否则为 false。
		/// </returns>
		public override bool CanWrite
		{
			get { return _innerStream.CanWrite; }
		}

		/// <summary>
		/// 当在派生类中重写时，获取用字节表示的流长度。
		/// </summary>
		/// <returns>
		/// 用字节表示流长度的长值。
		/// </returns>
		/// <exception cref="T:System.NotSupportedException">从 Stream 派生的类不支持查找。</exception><exception cref="T:System.ObjectDisposedException">在流关闭后调用方法。</exception>
		public override long Length
		{
			get { return _innerStream.Length; }
		}

		/// <summary>
		/// 当在派生类中重写时，获取或设置当前流中的位置。
		/// </summary>
		/// <returns>
		/// 流中的当前位置。
		/// </returns>
		/// <exception cref="T:System.IO.IOException">发生 I/O 错误。</exception><exception cref="T:System.NotSupportedException">流不支持查找。</exception><exception cref="T:System.ObjectDisposedException">在流关闭后调用方法。</exception>
		public override long Position
		{
			get { return _innerStream.Position; }
			set
			{
				_innerStream.Position = value;
				OnPositionChanged();
			}
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			_innerStream.Dispose();
		}

		/// <summary>
		/// 从流中读取一个字节，并将流内的位置向前推进一个字节，或者如果已到达流的末尾，则返回 -1。
		/// </summary>
		/// <returns>
		/// 转换为 Int32 的无符号字节，或者如果到达流的末尾，则为 -1。
		/// </returns>
		/// <exception cref="T:System.NotSupportedException">流不支持读取。</exception><exception cref="T:System.ObjectDisposedException">在流关闭后调用方法。</exception>
		public override int ReadByte()
		{
			var data = base.ReadByte();
			OnPositionChanged();
			return data;
		}

		/// <summary>
		/// 将一个字节写入流内的当前位置，并将流内的位置向前推进一个字节。
		/// </summary>
		/// <param name="value">要写入流的字节。</param><exception cref="T:System.IO.IOException">发生 I/O 错误。</exception><exception cref="T:System.NotSupportedException">该流不支持写入，或者该流已关闭。</exception><exception cref="T:System.ObjectDisposedException">在流关闭后调用方法。</exception>
		public override void WriteByte(byte value)
		{
			base.WriteByte(value);
			OnPositionChanged();
		}

#if !NETSTANDARD1_6_1 && !NETSTANDARD2_0 && !NETSTANDARD3_0

		/// <summary>
		/// 等待挂起的异步读取完成。 （考虑使用<see cref="M:System.IO.Stream.ReadAsync(System.Byte[],System.Int32,System.Int32)"/>进行替换；请参见“备注”部分。）
		/// </summary>
		/// <returns>
		/// 从流中读取的字节数，介于零 (0) 和所请求的字节数之间。 流仅在流的末尾返回零 (0)，否则应一直阻止到至少有 1 个字节可用为止。
		/// </returns>
		/// <param name="asyncResult">对要完成的挂起异步请求的引用。</param><exception cref="T:System.ArgumentNullException"><paramref name="asyncResult"/> 为 null。</exception><exception cref="T:System.ArgumentException">处于挂起状态的读取操作的句柄不可用。 - 或 - 悬挂操作不支持读取。</exception><exception cref="T:System.InvalidOperationException"><paramref name="asyncResult"/> 并非源自当前流上的 <see cref="M:System.IO.Stream.BeginRead(System.Byte[],System.Int32,System.Int32,System.AsyncCallback,System.Object)"/> 方法。</exception><exception cref="T:System.IO.IOException">此流关闭或发生内部错误。</exception>
		public override int EndRead(IAsyncResult asyncResult)
		{
			var result = base.EndRead(asyncResult);
			OnPositionChanged();
			return result;
		}

		/// <summary>
		/// 结束异步写操作。 （考虑使用<see cref="M:System.IO.Stream.WriteAsync(System.Byte[],System.Int32,System.Int32)"/>进行替换；请参见“备注”部分。）
		/// </summary>
		/// <param name="asyncResult">对未完成的异步 I/O 请求的引用。</param><exception cref="T:System.ArgumentNullException"><paramref name="asyncResult"/> 为 null。</exception><exception cref="T:System.ArgumentException">处于挂起状态的写入操作的句柄不可用。 - 或 - 悬挂操作不支持写入。</exception><exception cref="T:System.InvalidOperationException"><paramref name="asyncResult"/> 并非源自当前流上的 <see cref="M:System.IO.Stream.BeginWrite(System.Byte[],System.Int32,System.Int32,System.AsyncCallback,System.Object)"/> 方法。</exception><exception cref="T:System.IO.IOException">此流关闭或发生内部错误。</exception>
		public override void EndWrite(IAsyncResult asyncResult)
		{
			base.EndWrite(asyncResult);
			OnPositionChanged();
		}

#endif

#if NET45 || NETSTANDARD1_6_1 || NETSTANDARD2_0 || NETSTANDARD3_0

		/// <summary>
		/// 从当前流异步读取字节序列，将流中的位置向前移动读取的字节数，并监控取消请求。
		/// </summary>
		/// <returns>
		/// 表示异步读取操作的任务。 <paramref name="TResult"/> 参数的值包含读入缓冲区的总字节数。 如果当前可用字节数少于所请求的字节数，则该结果值可能小于所请求的字节数，或者如果已到达流的末尾时，则为 0（零）。
		/// </returns>
		/// <param name="buffer">数据写入的缓冲区。</param><param name="offset"><paramref name="buffer"/> 中的字节偏移量，从该偏移量开始写入从流中读取的数据。</param><param name="count">最多读取的字节数。</param><param name="cancellationToken">针对取消请求监视的标记。 默认值为 <see cref="P:System.Threading.CancellationToken.None"/>。</param><exception cref="T:System.ArgumentNullException"><paramref name="buffer"/> 为 null。</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="offset"/> 或 <paramref name="count"/> 为负。</exception><exception cref="T:System.ArgumentException"><paramref name="offset"/> 与 <paramref name="count"/> 的和大于缓冲区长度。</exception><exception cref="T:System.NotSupportedException">流不支持读取。</exception><exception cref="T:System.ObjectDisposedException">流已被释放。</exception><exception cref="T:System.InvalidOperationException">该流正在由其前一次读取操作使用。</exception>
		public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			var readcount = await base.ReadAsync(buffer, offset, count, cancellationToken).ConfigureAwait(false);
			OnPositionChanged();
			return readcount;
		}

		/// <summary>
		/// 将字节序列异步写入当前流，通过写入的字节数提前该流的当前位置，并监视取消请求数。
		/// </summary>
		/// <returns>
		/// 表示异步写入操作的任务。
		/// </returns>
		/// <param name="buffer">从中写入数据的缓冲区。</param><param name="offset"><paramref name="buffer"/> 中的从零开始的字节偏移量，从此处开始将字节复制到该流。</param><param name="count">最多写入的字节数。</param><param name="cancellationToken">针对取消请求监视的标记。 默认值为 <see cref="P:System.Threading.CancellationToken.None"/>。</param><exception cref="T:System.ArgumentNullException"><paramref name="buffer"/> 为 null。</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="offset"/> 或 <paramref name="count"/> 为负。</exception><exception cref="T:System.ArgumentException"><paramref name="offset"/> 与 <paramref name="count"/> 的和大于缓冲区长度。</exception><exception cref="T:System.NotSupportedException">流不支持写入。</exception><exception cref="T:System.ObjectDisposedException">流已被释放。</exception><exception cref="T:System.InvalidOperationException">该流正在由其前一次写入操作使用。</exception>
		public override async Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			await base.WriteAsync(buffer, offset, count, cancellationToken);
			OnPositionChanged();
		}
#endif
	}
}

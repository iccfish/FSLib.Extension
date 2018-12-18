namespace FSLib.Extension.FishLib
{
	using System;

	/// <summary>
	/// Promise事件参数
	/// </summary>
	public class DeferredEventArgs<T> : EventArgs
	{

		/// <summary>
		/// 获得当前的结果
		/// </summary>
		public T Result { get; set; }

		/// <summary>
		/// 获得当前的错误
		/// </summary>
		public Exception Exception { get; set; }

		/// <summary>
		/// 创建 <see cref="DeferredEventArgs{T}" />  的新实例(DeferredEventArgs)
		/// </summary>
		/// <param name="result"></param>
		/// <param name="exception"></param>
		public DeferredEventArgs(T result, Exception exception)
		{
			Result = result;
			Exception = exception;
		}
	}
}
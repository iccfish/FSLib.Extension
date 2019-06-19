namespace FSLib.Extension
{
	using System;

	public class Deferred<T> : DeferredSource<T>
	{
		public Deferred(bool captureContext = true)
			: base(captureContext)
		{
			
		}
		/// <summary>
		/// 标记为任务失败
		/// </summary>
		/// <param name="ex">任务的异常</param>
		/// <param name="result">设置的结果</param>
		public new void Reject(Exception ex = null, T result = default(T))
		{
			base.Reject(ex, result);
		}

		/// <summary>
		/// 标记为已完成
		/// </summary>
		/// <param name="result">任务的结果</param>
		public new void Resolve(T result)
		{
			base.Resolve(result);
		}

		/// <summary>
		/// 通知进度发生变化
		/// </summary>
		/// <param name="progressObject"></param>
		public new void Notify(object progressObject)
		{
			base.Notify(progressObject);
		}

		/// <summary>
		/// 获得对应的Promise对象
		/// </summary>
		/// <returns></returns>
		public DeferredSource<T> Promise()
		{
			return this;
		}
	}
}

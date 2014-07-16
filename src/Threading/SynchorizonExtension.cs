namespace System.Threading
{
	public static class SynchorizonExtension
	{
		/// <summary>
		/// 同步一个不需要参数的回调到目标线程
		/// </summary>
		/// <param name="context"></param>
		/// <param name="callback"></param>
		public static void Send(this SynchronizationContext context, SendOrPostCallback callback)
		{
			context.Send(callback, null);
		}
		/// <summary>
		/// 异步一个不需要参数的回调到目标线程
		/// </summary>
		/// <param name="context"></param>
		/// <param name="callback"></param>
		public static void Post(this SynchronizationContext context, SendOrPostCallback callback)
		{
			context.Post(callback, null);
		}

		/// <summary>
		/// 同步一个不需要参数的回调到目标线程
		/// </summary>
		/// <param name="context"></param>
		/// <param name="callback"></param>
		public static void Send(this SynchronizationContext context, Action callback)
		{
			if (callback == null)
				return;

			context.Send(_ => callback(), null);
		}

		/// <summary>
		/// 异步一个不需要参数的回调到目标线程
		/// </summary>
		/// <param name="context"></param>
		/// <param name="callback"></param>
		public static void Post(this SynchronizationContext context, Action callback)
		{
			if (callback == null)
				return;

			context.Post(_ => callback(), null);
		}

		/// <summary>
		/// 同步一个不需要参数的回调到目标线程
		/// </summary>
		/// <param name="context"></param>
		/// <param name="callback"></param>
		public static void Send<T>(this SynchronizationContext context, Action<T> callback, T arg)
		{
			if (callback == null)
				return;

			context.Send(_ => callback(arg), null);
		}

		/// <summary>
		/// 异步一个不需要参数的回调到目标线程
		/// </summary>
		/// <param name="context"></param>
		/// <param name="callback"></param>
		public static void Post<T>(this SynchronizationContext context, Action<T> callback, T arg)
		{
			if (callback == null)
				return;

			context.Post(_ => callback(arg), null);
		}
	}
}


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
	}
}


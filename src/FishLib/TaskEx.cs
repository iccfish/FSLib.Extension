#if NET_GT_4
namespace FSLib.Extension.FishLib
{
	using System;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;

	/// <summary>
	/// 对 <see cref="Task"/> 的扩展方法
	/// </summary>
	public class TaskEx
	{
		/// <summary>
		/// 竞争运行一组任务，直到有一个完成或全部失败
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="factory"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		public static async Task<T> RunTaskVie<T>(Func<CancellationToken, int, Task<T>> factory, int count)
		{
			var tcs = new TaskCompletionSource<T>();
			var cts = new CancellationTokenSource();

			var tasks = Enumerable.Range(0, count).Select(s => factory(cts.Token, s)).ToArray();

			void CheckTaskStatus()
			{
				if (tasks.All(s => s.Status == TaskStatus.Canceled || s.Status == TaskStatus.Faulted))
				{
					tcs.SetException(new Exception("没有任务成功完成"));
				}
			}

			foreach (var task in tasks)
			{
#pragma warning disable 4014
				task.ContinueWith(t =>
#pragma warning restore 4014
				{
					if (t.IsFaulted)
					{
						var dummy = t.Exception;

						CheckTaskStatus();
					}
					else if (t.IsCanceled)
					{
						//已取消，不管
						CheckTaskStatus();
					}
					else
					{
						tcs.TrySetResult(t.Result);

						//取消其它请求
						cts.Cancel();
					}
				},
					TaskContinuationOptions.ExecuteSynchronously);
				task.Start();
			}

			return await tcs.Task;
		}
	}
}
#endif
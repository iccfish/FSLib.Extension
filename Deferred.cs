using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading;

namespace System
{
	public class Deferred<T>
	{
		int _finished;

		/// <summary>
		/// 获得或设置当前的状态
		/// </summary>
		public SynchronizationContext Context { get; private set; }

		/// <summary>
		/// 获得当前的结果
		/// </summary>
		public T Result { get; private set; }

		/// <summary>
		/// 获得当前的错误
		/// </summary>
		public Exception Exception { get; private set; }

		public void Reject(Exception ex = null)
		{
			if (Interlocked.Exchange(ref _finished, 1) != 0)
				return;

			Exception = ex;
			if (Context == null)
				OnOperationFailed(new DeferredEventArgs(Result, Exception));
			else Context.Post(_ => OnOperationFailed(new DeferredEventArgs(Result, Exception)), null);
		}

		public void ResolveWith(T result)
		{
			if (Interlocked.Exchange(ref _finished, 2) != 0)
				return;

			Result = result;
			if (Context == null)
				OnOperationSuccess(new DeferredEventArgs(Result, Exception));
			else Context.Post(_ => OnOperationSuccess(new DeferredEventArgs(Result, Exception)), null);
		}

		public Deferred()
		{
			Context = SynchronizationContext.Current;
		}

		public Deferred<T> Done(Action<object, DeferredEventArgs> action)
		{
			if (_finished == 2)
			{
				if (Context == null) action(this, new DeferredEventArgs(Result, Exception));
				else Context.Post(_ => action(this, new DeferredEventArgs(Result, Exception)), null);
			}
			else OperationSuccess += new EventHandler<DeferredEventArgs>(action);

			return this;
		}

		public Deferred<T> Fail(Action<object, DeferredEventArgs> action)
		{
			if (_finished == 1)
			{
				if (Context == null) action(this, new DeferredEventArgs(Result, Exception));
				else Context.Post(_ => action(this, new DeferredEventArgs(Result, Exception)), null);
			}
			else OperationFailed += new EventHandler<DeferredEventArgs>(action);

			return this;
		}

		public Deferred<T> Always(Action<object, DeferredEventArgs> action)
		{
			if (action == null)
				throw new ArgumentNullException("action", "action is null.");

			if (_finished != 0)
			{
				if (Context == null) action(this, new DeferredEventArgs(Result, Exception));
				else Context.Post(_ => action(this, new DeferredEventArgs(Result, Exception)), null);
			}
			else OperationCompleted += new EventHandler<DeferredEventArgs>(action);

			return this;
		}

		#region 事件

		public class DeferredEventArgs : EventArgs
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
			/// 创建 <see cref="DeferredEventArgs" />  的新实例(DeferredEventArgs)
			/// </summary>
			/// <param name="result"></param>
			/// <param name="exception"></param>
			public DeferredEventArgs(T result, Exception exception)
			{
				Result = result;
				Exception = exception;
			}
		}

		public event EventHandler BeforeStart;

		/// <summary>
		/// 引发 <see cref="BeforeStart" /> 事件
		/// </summary>
		protected virtual void OnBeforeStart()
		{
			var handler = BeforeStart;
			if (handler != null)
				handler(this, EventArgs.Empty);
		}


		public event EventHandler<DeferredEventArgs> OperationSuccess;

		/// <summary>
		/// 引发 <see cref="OperationSuccess" /> 事件
		/// </summary>
		/// <param name="ea">包含此事件的参数</param>
		protected virtual void OnOperationSuccess(DeferredEventArgs ea)
		{
			var handler = OperationSuccess;
			if (handler != null)
				handler(this, ea);

			if (Context == null)
				OnOperationCompleted(new DeferredEventArgs(Result, Exception));
			else Context.Post(_ => OnOperationCompleted(new DeferredEventArgs(Result, Exception)), null);
		}

		public event EventHandler<DeferredEventArgs> OperationFailed;

		/// <summary>
		/// 引发 <see cref="OperationFailed" /> 事件
		/// </summary>
		/// <param name="ea">包含此事件的参数</param>
		protected virtual void OnOperationFailed(DeferredEventArgs ea)
		{
			var handler = OperationFailed;
			if (handler != null)
				handler(this, ea);

			if (Context == null)
				OnOperationCompleted(new DeferredEventArgs(Result, Exception));
			else Context.Post(_ => OnOperationCompleted(new DeferredEventArgs(Result, Exception)), null);
		}

		public event EventHandler<DeferredEventArgs> OperationCompleted;

		/// <summary>
		/// 引发 <see cref="OperationCompleted" /> 事件
		/// </summary>
		/// <param name="ea">包含此事件的参数</param>
		protected virtual void OnOperationCompleted(DeferredEventArgs ea)
		{
			var handler = OperationCompleted;
			if (handler != null)
				handler(this, ea);
		}


		#endregion
	}
}

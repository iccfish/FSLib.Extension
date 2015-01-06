namespace System.FishLib
{
	using System.Threading;

	public abstract class DeferredSource<T>
	{
		protected int _finished;

		public DeferredSource(bool captureContext = true)
		{
			if (captureContext)
				Context = SynchronizationContext.Current;
		}

		/// <summary>
		/// 获得或设置当前的状态
		/// </summary>
		public SynchronizationContext Context { get; private set; }

		/// <summary>
		/// 获得当前的结果
		/// </summary>
		public T Result { get; protected set; }

		/// <summary>
		/// 获得当前的错误
		/// </summary>
		public Exception Exception { get; protected set; }

		/// <summary>
		/// 注册完成时应该调用的回调
		/// </summary>
		/// <param name="action"></param>
		/// <returns></returns>
		public DeferredSource<T> Done(Action<object, DeferredEventArgs<T>> action)
		{
			if (action == null)
				throw new ArgumentNullException("action", "action is null.");

			if (_finished == 2)
			{
				if (Context == null) action(this, new DeferredEventArgs<T>(Result, Exception));
				else Context.Post(_ => action(this, new DeferredEventArgs<T>(Result, Exception)), null);
			}
			else OperationSuccess += new EventHandler<DeferredEventArgs<T>>(action);

			return this;
		}

		/// <summary>
		/// 注册失败时应该调用的回调
		/// </summary>
		/// <param name="action"></param>
		/// <returns></returns>
		public DeferredSource<T> Fail(Action<object, DeferredEventArgs<T>> action)
		{
			if (action == null)
				throw new ArgumentNullException("action", "action is null.");

			if (_finished == 1)
			{
				if (Context == null) action(this, new DeferredEventArgs<T>(Result, Exception));
				else Context.Post(_ => action(this, new DeferredEventArgs<T>(Result, Exception)), null);
			}
			else OperationFailed += new EventHandler<DeferredEventArgs<T>>(action);

			return this;
		}

		/// <summary>
		/// 注册当进度发生变化时需要进行的回调
		/// </summary>
		/// <param name="action"></param>
		/// <returns></returns>
		public DeferredSource<T> Progress(Action<object, DeferredProgressEventArgs> action)
		{
			if (action == null)
				throw new ArgumentNullException("action", "action is null.");

			ProgressChanged += new EventHandler<DeferredProgressEventArgs>(action);

			return this;
		}

		/// <summary>
		/// 注册无论成功或失败都必须执行的回调
		/// </summary>
		/// <param name="action"></param>
		/// <returns></returns>
		public DeferredSource<T> Always(Action<object, DeferredEventArgs<T>> action)
		{
			if (action == null)
				throw new ArgumentNullException("action", "action is null.");

			if (_finished != 0)
			{
				if (Context == null) action(this, new DeferredEventArgs<T>(Result, Exception));
				else Context.Post(_ => action(this, new DeferredEventArgs<T>(Result, Exception)), null);
			}
			else OperationCompleted += new EventHandler<DeferredEventArgs<T>>(action);

			return this;
		}

		#region 结果设置

		/// <summary>
		/// 标记为任务失败
		/// </summary>
		/// <param name="ex">任务的异常</param>
		/// <param name="result">设置的结果</param>
		protected void Reject(Exception ex = null, T result = default(T))
		{
			if (Interlocked.Exchange(ref _finished, 1) != 0)
				return;

			Result = result;
			Exception = ex;
			OnOperationFailed(new DeferredEventArgs<T>(Result, Exception));
		}

		/// <summary>
		/// 标记为已完成
		/// </summary>
		/// <param name="result">任务的结果</param>
		protected void Resolve(T result)
		{
			if (Interlocked.Exchange(ref _finished, 2) != 0)
				return;

			Result = result;
			Exception = null;
			OnOperationSuccess(new DeferredEventArgs<T>(Result, Exception));
		}

		/// <summary>
		/// 通知进度变化
		/// </summary>
		/// <param name="progressObject"></param>
		protected void Notify(object progressObject)
		{
			OnProgressChanged(new DeferredProgressEventArgs(progressObject));
		}

		#endregion

		#region 事件

		void TriggerHandler(EventHandler handler)
		{
			if (handler == null)
				return;

			if (Context == null)
				handler(this, EventArgs.Empty);
			else Context.Post(() => handler(this, EventArgs.Empty));
		}
		void TriggerHandler<T>(EventHandler<T> handler, T evargs) where T : EventArgs
		{
			if (handler == null)
				return;

			if (Context == null)
				handler(this, evargs);
			else Context.Post(() => handler(this, evargs));
		}

		public event EventHandler BeforeStart;

		/// <summary>
		/// 引发 <see cref="BeforeStart" /> 事件
		/// </summary>
		protected virtual void OnBeforeStart()
		{
			TriggerHandler(BeforeStart);
		}


		public event EventHandler<DeferredEventArgs<T>> OperationSuccess;

		/// <summary>
		/// 引发 <see cref="OperationSuccess" /> 事件
		/// </summary>
		/// <param name="ea">包含此事件的参数</param>
		protected virtual void OnOperationSuccess(DeferredEventArgs<T> ea)
		{
			TriggerHandler(OperationSuccess, ea);
			OnOperationCompleted(ea);
		}

		public event EventHandler<DeferredEventArgs<T>> OperationFailed;

		/// <summary>
		/// 引发 <see cref="OperationFailed" /> 事件
		/// </summary>
		/// <param name="ea">包含此事件的参数</param>
		protected virtual void OnOperationFailed(DeferredEventArgs<T> ea)
		{
			TriggerHandler(OperationFailed, ea);
			OnOperationCompleted(ea);
		}

		public event EventHandler<DeferredEventArgs<T>> OperationCompleted;

		/// <summary>
		/// 引发 <see cref="OperationCompleted" /> 事件
		/// </summary>
		/// <param name="ea">包含此事件的参数</param>
		protected virtual void OnOperationCompleted(DeferredEventArgs<T> ea)
		{
			TriggerHandler(OperationCompleted, ea);
		}

		/// <summary>
		/// 进度发生变化
		/// </summary>
		public event EventHandler<DeferredProgressEventArgs> ProgressChanged;

		/// <summary>
		/// 引发 <see cref="ProgressChanged" /> 事件
		/// </summary>
		/// <param name="ea">包含此事件的参数</param>
		protected virtual void OnProgressChanged(DeferredProgressEventArgs ea)
		{
			TriggerHandler(ProgressChanged, ea);
		}

		#endregion

	}
}
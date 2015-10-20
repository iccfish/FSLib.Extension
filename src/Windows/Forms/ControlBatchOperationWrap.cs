using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Windows.Forms
{
	using System.Linq.Expressions;
	using System.Reflection;

	class ControlBatchOperationWrap<T> : IDisposable where T : Control
	{
		static Dictionary<Type, object> _updateDispatcher = new Dictionary<Type, object>();

		static Action<T>[] LookupUpdateDispatcher<T>(T obj)
		{
			object data;
			var t = typeof(T);

			if (!_updateDispatcher.TryGetValue(t, out data))
			{
				lock (_updateDispatcher)
				{
					if (!_updateDispatcher.TryGetValue(t, out data))
					{
						//查找方法
						var mbu = t.GetMethod("BeginUpdate", BindingFlags.Instance | BindingFlags.Public);
						var meu = t.GetMethod("EndUpdate", BindingFlags.Instance | BindingFlags.Public);

						data = null;
						if (mbu != null && meu != null)
						{
							var mbui = Expression.Parameter(t);
							var mbue = Expression.Call(mbui, mbu);
							var mbua = Expression.Lambda<Action<T>>(mbue, mbui).Compile();

							var mbei = Expression.Parameter(t);
							var mbee = Expression.Call(mbei, meu);
							var mbea = Expression.Lambda<Action<T>>(mbee, mbei).Compile();

							data = new[] { mbua, mbea };
						}
						_updateDispatcher.Add(t, data);
					}
				}
			}

			return data as Action<T>[];
		}


		/// <summary>
		/// 获得绑定的控件
		/// </summary>
		public T Control { get; private set; }

		/// <summary>
		/// 创建 <see cref="ControlBatchOperationWrap{T}" />  的新实例(ControlBatchOperationWrap)
		/// </summary>
		/// <param name="control"></param>
		public ControlBatchOperationWrap(T control)
		{
			if (control == null)
				throw new ArgumentNullException(nameof(control));

			Control = control;
			control.SuspendLayout();

			var actions = LookupUpdateDispatcher(control);
			actions?[0].Invoke(control);
		}

		void ResumeLayout()
		{
			var control = Control;
			if (control == null || control.IsDisposed)
				return;

			var actions = LookupUpdateDispatcher(control);
			actions?[1].Invoke(control);
		}


		#region Dispose方法实现

		bool _disposed;

		/// <summary>
		/// 释放资源
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (_disposed) return;
			_disposed = true;

			ResumeLayout();

			//挂起终结器
			GC.SuppressFinalize(this);
		}

		#endregion


	}
}

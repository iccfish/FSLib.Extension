using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// ReSharper disable once CheckNamespace
namespace System.Windows.Forms
{
	using System.Linq.Expressions;
	using System.Reflection;

	class ControlBatchOperationWrap<T> : IDisposable where T : Control
	{
		static readonly Action<T> ActionBeginUpdate, ActionEndUpdate;

		static ControlBatchOperationWrap()
		{
			var t = typeof(T);
			//查找方法
			var mbu = t.GetMethod("BeginUpdate", BindingFlags.Instance | BindingFlags.Public, null, new Type[0], new ParameterModifier[0]);
			var meu = t.GetMethod("EndUpdate", BindingFlags.Instance | BindingFlags.Public, null, new Type[0], new ParameterModifier[0]);

			if (mbu != null && meu != null)
			{
				var mbui = Expression.Parameter(t, "thisobj");
				var mbue = Expression.Call(mbui, mbu);
				var mbua = Expression.Lambda<Action<T>>(mbue, mbui).Compile();

				var mbei = Expression.Parameter(t, "thisobj");
				var mbee = Expression.Call(mbei, meu);
				var mbea = Expression.Lambda<Action<T>>(mbee, mbei).Compile();

				ActionBeginUpdate = mbua;
				ActionEndUpdate = mbea;
			}
		}

		/// <summary>
		/// 获得绑定的控件
		/// </summary>
		public T Control { get; }

		/// <summary>
		/// 创建 <see cref="ControlBatchOperationWrap{T}" />  的新实例(ControlBatchOperationWrap)
		/// </summary>
		/// <param name="control"></param>
		public ControlBatchOperationWrap(T control)
		{
			Control = control ?? throw new ArgumentNullException(nameof(control));
			control.SuspendLayout();

			ActionBeginUpdate?.Invoke(control);
		}

		void ResumeLayout()
		{
			var control = Control;
			if (control == null || control.IsDisposed)
				return;

			control.ResumeLayout(true);
			ActionEndUpdate?.Invoke(control);
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

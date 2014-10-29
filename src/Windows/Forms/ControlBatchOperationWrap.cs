using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Windows.Forms
{
	class ControlBatchOperationWrap : IDisposable
	{
		/// <summary>
		/// 获得绑定的控件
		/// </summary>
		public Control Control { get; private set; }

		/// <summary>
		/// 创建 <see cref="ControlBatchOperationWrap" />  的新实例(ControlBatchOperationWrap)
		/// </summary>
		/// <param name="control"></param>
		public ControlBatchOperationWrap(Control control)
		{
			if (control == null)
				throw new ArgumentNullException("control");

			Control = control;
			control.SuspendLayout();

			if (control is ListView)
				(control as ListView).BeginUpdate();
			else if (control is ListBox)
				(control as ListBox).BeginUpdate();
			else if (control is TreeView)
				(control as TreeView).BeginUpdate();
		}

		void ResumeLayout()
		{
			var control = Control;
			if (control == null || control.IsDisposed)
				return;

			if (control is ListView)
				(control as ListView).EndUpdate();
			else if (control is ListBox)
				(control as ListBox).EndUpdate();
			else if (control is TreeView)
				(control as TreeView).EndUpdate();
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

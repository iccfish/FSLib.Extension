using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace System.Windows.Forms
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static class ListViewExtensionMethods
	{
		/// <summary>
		/// 移除选中项，并返回移除的项
		/// </summary>
		/// <param name="view"></param>
		/// <returns></returns>
		public static ListViewItem[] RemoveSelectedItems(this ListView view)
		{
			if (view.SelectedItems.Count == 0)
				return null;

			var items = view.SelectedItems.Cast<ListViewItem>().ToArray();
			view.SuspendLayout();
			items.ForEach(view.Items.Remove);
			view.ResumeLayout();

			return items;
		}

		/// <summary>
		/// 全选
		/// </summary>
		/// <param name="view"></param>
		public static void SelectAll(this ListView view)
		{
			view.Items.Cast<ListViewItem>().ForEach(s => s.Selected = true);
		}

		/// <summary>
		/// 全部不选
		/// </summary>
		/// <param name="view"></param>
		public static void SelectNone(this ListView view)
		{
			view.Items.Cast<ListViewItem>().ForEach(s => s.Selected = false);
		}

		/// <summary>
		/// 反选
		/// </summary>
		/// <param name="view"></param>
		public static void SelectInverse(this ListView view)
		{
			view.Items.Cast<ListViewItem>().ForEach(s => s.Selected = !s.Selected);
		}

		/// <summary>
		/// 全选
		/// </summary>
		/// <param name="view"></param>
		public static void CheckAll(this ListView view)
		{
			view.Items.Cast<ListViewItem>().ForEach(s => s.Checked = true);
		}

		/// <summary>
		/// 全部不选
		/// </summary>
		/// <param name="view"></param>
		public static void CheckNone(this ListView view)
		{
			view.Items.Cast<ListViewItem>().ForEach(s => s.Checked = false);
		}

		/// <summary>
		/// 反选
		/// </summary>
		/// <param name="view"></param>
		public static void CheckInverse(this ListView view)
		{
			view.Items.Cast<ListViewItem>().ForEach(s => s.Checked = !s.Checked);
		}

	}
}

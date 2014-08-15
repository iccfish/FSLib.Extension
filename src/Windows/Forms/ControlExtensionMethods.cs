using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace System.Windows.Forms
{
	using Drawing;

	using FishExtension;

	using Linq.Expressions;
	using Reflection;

	/// <summary>
	/// 关于控件的扩展方法
	/// </summary>
	public static class ControlExtensionMethods
	{
		/// <summary>
		/// 保持指定的控件始终居中
		/// </summary>
		/// <param name="control">要保持居中的控件</param>
		/// <param name="parentControl">相对位置的父控件，默认为上级控件</param>
		public static T KeepCenter<T>(this T control, Control parentControl = null) where T : Control
		{
			if (control == null || control.Parent == null)
			{
				return control;
			}
			parentControl = parentControl ?? control.Parent;

			parentControl.SizeChanged += (s, e) =>
			{
				if (parentControl == null)
					return;

				var location = new Point((parentControl.Width - control.Width) / 2, (parentControl.Height - control.Height) / 2);
				if (!(parentControl is Form))
					location.Offset(parentControl.Location);
				control.Location = location;
			};
			var loc = new Point((parentControl.Width - control.Width) / 2, (parentControl.Height - control.Height) / 2);
			if (!(parentControl is Form))
				loc.Offset(parentControl.Location);
			control.Location = loc;

			return control;
		}

		#region Navigation

		/// <summary>
		/// 向上级不断地查找到指定类型的控件
		/// </summary>
		/// <typeparam name="T">要查找的控件类型</typeparam>
		/// <param name="control">当前控件</param>
		/// <returns></returns>
		public static T Closest<T>(this Control control) where T : Control
		{
			if (control == null)
				return null;

			if (control is T)
				return control as T;

			do
			{
				control = control.Parent;
			}
			while (control != null && !(control is T));

			return control as T;
		}

		#endregion

		#region CheckedListBox

		/// <summary>
		/// 取消选择所有选项
		/// </summary>
		/// <param name="clb">控件</param>
		public static void UnCheckAll(this CheckedListBox clb)
		{
			Enumerable.Range(0, clb.Items.Count).ForEach(s => clb.SetItemChecked(s, false));
		}

		/// <summary>
		/// 全选所有选项
		/// </summary>
		/// <param name="clb">控件</param>
		public static void CheckAll(this CheckedListBox clb)
		{
			Enumerable.Range(0, clb.Items.Count).ForEach(s => clb.SetItemChecked(s, true));
		}

		#endregion

		#region TreeView

		/// <summary>
		/// 获得当前节点的一级节点
		/// </summary>
		/// <param name="node">当前节点</param>
		/// <returns><see cref="T:System.Windows.Forms.TreeNode"/></returns>
		public static TreeNode GetTopNode(this TreeNode node)
		{
			while (node.Parent != null)
			{
				node = node.Parent;
			}

			return node;
		}

		/// <summary>
		/// 获得所有节点列表
		/// </summary>
		/// <param name="view">当前列表</param>
		/// <param name="parentNode">父节点</param>
		/// <param name="includeFolderNode">是否包含文件夹节点</param>
		/// <returns></returns>
		public static List<TreeNode> GetAllNodes(this TreeView view, TreeNode parentNode, bool includeFolderNode)
		{
			var nodes = parentNode == null ? view.Nodes : parentNode.Nodes;
			var list = new List<TreeNode>(0x400);
			AddNodesToList(list, nodes, includeFolderNode);

			return list;
		}

		static void AddNodesToList(List<TreeNode> list, TreeNodeCollection nodes, bool addFolder)
		{
			foreach (TreeNode node in nodes)
			{
				if (node.Nodes.Count > 0)
				{
					if (addFolder) list.Add(node);
					AddNodesToList(list, node.Nodes, addFolder);
				}
				else
					list.Add(node);
			}
		}

		#endregion

		#region ListView

		/// <summary>
		/// 获得当前选中的项
		/// </summary>
		/// <param name="view">对应的控件</param>
		/// <returns></returns>
		public static ListViewItem GetCurrentViewItem(this ListView view)
		{
			if (view == null) return null;

			var items = view.SelectedItems;
			return items.Count == 0 ? null : items[0];
		}

		/// <summary>
		/// 获得当前选中的项
		/// </summary>
		/// <param name="view">对应的控件</param>
		/// <returns></returns>
		public static T GetCurrentViewItem<T>(this ListView view) where T : ListViewItem
		{
			return view.GetCurrentViewItem() as T;
		}

		#endregion

		#region Component

		/// <summary>
		/// 确定指定的组件是否处于设计模式
		/// </summary>
		/// <param name="target">组件</param>
		/// <returns><c>true</c> 表示正在设计模式，<c>false</c> 表示不是</returns>
		public static bool IsInDesignMode(this IComponent target)
		{
			if (target == null) throw new ArgumentNullException("target");

			if (target.Site == null) return false;

			return target.Site.DesignMode;
		}

		/// <summary>
		/// 确定指定的组件是否处于运行模式
		/// </summary>
		/// <param name="target">组件</param>
		/// <returns><c>true</c> 表示正在运行模式，<c>false</c> 表示不是</returns>
		public static bool IsInRuntimeMode(this IComponent target)
		{
			if (target == null) throw new ArgumentNullException("target");

			if (target.Site == null) return true;

			return !target.Site.DesignMode;
		}

		#endregion

		#region 文件扩展

		/// <summary>
		/// 判断拖放列表是否有任何一个符合要求的拖放项
		/// </summary>
		/// <param name="data">拖放数据</param>
		/// <param name="enableFolder">是否允许文件夹</param>
		/// <param name="enableFile">是否允许文件</param>
		/// <param name="fileExtensions">允许的文件类型</param>
		/// <returns>如果至少一个符合要求，则返回true，否则返回false</returns>
		public static bool HasAvailableFileItem(this DataObject data, bool enableFolder, bool enableFile, params string[] fileExtensions)
		{
			if (!data.ContainsFileDropList()) return false;

			return data.GetFileDropList().Cast<string>().Any(
				s =>
					(enableFolder && System.IO.Directory.Exists(s))
					||
					(enableFile && System.IO.File.Exists(s) && (fileExtensions.IsEmpty() || fileExtensions.Contains(System.IO.Path.GetExtension(s).Trim('.'))))
					);
		}


		/// <summary>
		/// 获得拖放列表中符合要求的拖放项
		/// </summary>
		/// <param name="data">拖放数据</param>
		/// <param name="enableFolder">是否允许文件夹</param>
		/// <param name="enableFile">是否允许文件</param>
		/// <param name="fileExtensions">允许的文件类型</param>
		/// <returns>返回符合要求的文件列表</returns>
		public static IEnumerable<string> GetAvailableFileItem(this DataObject data, bool enableFolder, bool enableFile, params string[] fileExtensions)
		{
			if (!data.ContainsFileDropList()) return null;

			return data.GetFileDropList().Cast<string>().Where(
				s =>
					(enableFolder && System.IO.Directory.Exists(s))
					||
					(enableFile && System.IO.File.Exists(s) && (fileExtensions.IsEmpty() || fileExtensions.Contains(System.IO.Path.GetExtension(s).Trim('.'))))
					);
		}


		#endregion

		#region QueryContinueDragEventArgs

		/// <summary>
		/// 判断ALT键是否按下
		/// </summary>
		/// <param name="e">事件数据</param>
		/// <returns>true/false</returns>
		public static bool IsAltPressed(this QueryContinueDragEventArgs e)
		{
			return e != null && (e.KeyState & 32) > 0;
		}

		/// <summary>
		/// 判断CTRL键是否按下
		/// </summary>
		/// <param name="e">事件数据</param>
		/// <returns>true/false</returns>
		public static bool IsCtrlPressed(this QueryContinueDragEventArgs e)
		{
			return e != null && (e.KeyState & 8) > 0;
		}

		/// <summary>
		/// 判断SHIFT键是否按下
		/// </summary>
		/// <param name="e">事件数据</param>
		/// <returns>true/false</returns>
		public static bool IsShiftPressed(this QueryContinueDragEventArgs e)
		{
			return e != null && (e.KeyState & 4) > 0;
		}

		/// <summary>
		/// 判断ALT键是否按下
		/// </summary>
		/// <param name="e">事件数据</param>
		/// <returns>true/false</returns>
		public static bool IsMouseLeftButtonPressed(this QueryContinueDragEventArgs e)
		{
			return e != null && (e.KeyState & 1) > 0;
		}

		/// <summary>
		/// 判断ALT键是否按下
		/// </summary>
		/// <param name="e">事件数据</param>
		/// <returns>true/false</returns>
		public static bool IsMouseRightButtonPressed(this QueryContinueDragEventArgs e)
		{
			return e != null && (e.KeyState & 2) > 0;
		}

		/// <summary>
		/// 判断ALT键是否按下
		/// </summary>
		/// <param name="e">事件数据</param>
		/// <returns>true/false</returns>
		public static bool IsMouseCenterButtonPressed(this QueryContinueDragEventArgs e)
		{
			return e != null && (e.KeyState & 16) > 0;
		}

		#endregion

		#region DragEventArgs

		/// <summary>
		/// 判断ALT键是否按下
		/// </summary>
		/// <param name="e">事件数据</param>
		/// <returns>true/false</returns>
		public static bool IsAltPressed(this DragEventArgs e)
		{
			return e != null && (e.KeyState & 32) > 0;
		}

		/// <summary>
		/// 判断CTRL键是否按下
		/// </summary>
		/// <param name="e">事件数据</param>
		/// <returns>true/false</returns>
		public static bool IsCtrlPressed(this DragEventArgs e)
		{
			return e != null && (e.KeyState & 8) > 0;
		}

		/// <summary>
		/// 判断SHIFT键是否按下
		/// </summary>
		/// <param name="e">事件数据</param>
		/// <returns>true/false</returns>
		public static bool IsShiftPressed(this DragEventArgs e)
		{
			return e != null && (e.KeyState & 4) > 0;
		}

		/// <summary>
		/// 判断ALT键是否按下
		/// </summary>
		/// <param name="e">事件数据</param>
		/// <returns>true/false</returns>
		public static bool IsMouseLeftButtonPressed(this DragEventArgs e)
		{
			return e != null && (e.KeyState & 1) > 0;
		}

		/// <summary>
		/// 判断ALT键是否按下
		/// </summary>
		/// <param name="e">事件数据</param>
		/// <returns>true/false</returns>
		public static bool IsMouseRightButtonPressed(this DragEventArgs e)
		{
			return e != null && (e.KeyState & 2) > 0;
		}

		/// <summary>
		/// 判断ALT键是否按下
		/// </summary>
		/// <param name="e">事件数据</param>
		/// <returns>true/false</returns>
		public static bool IsMouseCenterButtonPressed(this DragEventArgs e)
		{
			return e != null && (e.KeyState & 16) > 0;
		}

		#endregion

		#region DrawItemState

		/// <summary>
		/// 判断指定的状态是否有焦点
		/// </summary>
		/// <param name="state"></param>
		/// <returns></returns>
		public static bool IsFocused(this DrawItemState state)
		{
			return (state & DrawItemState.Focus) == DrawItemState.Focus;
		}

		/// <summary>
		/// 判断指定的状态是否已选中
		/// </summary>
		/// <param name="state"></param>
		/// <returns></returns>
		public static bool IsSelected(this DrawItemState state)
		{
			return (state & DrawItemState.Selected) == DrawItemState.Selected;
		}

		#endregion

		#region LinkLabel

		/// <summary>
		/// 设置链接的格式化文本，并附上链接。 &lt;!NAME&gt;LINK TEXT&lt;/!&gt;
		/// </summary>
		/// <param name="link"></param>
		/// <param name="text"></param>
		public static void SetLink(this LinkLabel link, string text)
		{
			if (link == null || text == null)
				return;

			//分析链接
			var matches = Regex.Matches(text, @"<(/?)!([^>]*)>", RegexOptions.IgnoreCase);

			if (matches.Count == 0)
			{
				link.Text = text;
				link.LinkArea = new LinkArea(0, text.Length);
			}
			else
			{
				//分析
				var links = new List<LinkLabel.Link>(matches.Count / 2);
				LinkLabel.Link tlink = null;
				var sb = new StringBuilder(text.Length);
				var lastIndex = 0;

				for (int i = 0; i < matches.Count; i++)
				{
					var m = matches[i];
					var tag = m.Groups[1].Value;

					if (m.Index > lastIndex)
					{
						sb.Append(text.Substring(lastIndex, m.Index - lastIndex));
					}

					if (tag == "/")
					{
						if (tlink != null)
						{
							tlink.Length = sb.Length - tlink.Start;
							links.Add(tlink);
							tlink = null;
						}
					}
					else
					{
						if (tlink == null)
						{
							tlink = new LinkLabel.Link()
								{
									Start = sb.Length,
									Name = m.Groups[2].Value ?? ""
								};
						}
					}
					lastIndex = m.Index + m.Length;
				}
				if (lastIndex < text.Length)
				{
					sb.Append(text.Substring(lastIndex, text.Length - lastIndex));
				}

				link.Text = sb.ToString();
				link.Links.Clear();
				links.ForEach(_ => link.Links.Add(_));
			}

		}

		#endregion

		#region TextBox

		/// <summary>
		/// 获得指定的文本框是不是没有输入内容
		/// </summary>
		/// <param name="txt"></param>
		/// <returns></returns>
		public static bool IsValueEmpty(this TextBox txt)
		{
			return txt == null || txt.Text.IsNullOrEmpty();
		}

		#endregion

		#region 数据绑定

		/// <summary>
		/// 添加一个数据源绑定
		/// </summary>
		/// <typeparam name="TControl"></typeparam>
		/// <typeparam name="TSource"></typeparam>
		/// <typeparam name="TValue"></typeparam>
		/// <param name="control"></param>
		/// <param name="source"></param>
		/// <param name="controlExpression"></param>
		/// <param name="propertyExpression"></param>
		public static void AddDataBinding<TControl, TSource, TValue>(this TControl control, TSource source, Expression<Func<TControl, TValue>> controlExpression, Expression<Func<TSource, TValue>> propertyExpression) where TControl : Control
		{
			if (control == null || controlExpression == null || propertyExpression == null)
				return;

			var controlPropertyName = controlExpression.GetExpressionAccessedMemberName();
			var sourcePropertyName = propertyExpression.GetExpressionAccessedMemberName();
			if (string.IsNullOrEmpty(sourcePropertyName) || string.IsNullOrEmpty(controlPropertyName))
				return;

			control.DataBindings.Add(controlPropertyName, source, sourcePropertyName);
		}

		/// <summary>
		/// 增加文本的验证，并在指定的图片框中显示状态。
		/// </summary>
		/// <param name="txt"></param>
		/// <param name="pbTarget"></param>
		/// <param name="validationFunc"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public static void AddTextValidationStatus([NotNull] this TextBox txt, [NotNull] PictureBox pbTarget, [NotNull] Func<TextBox, bool> validationFunc)
		{
			if (txt == null)
				throw new ArgumentNullException("txt");
			if (pbTarget == null)
				throw new ArgumentNullException("pbTarget");
			if (validationFunc == null)
				throw new ArgumentNullException("validationFunc");
			var callback = new Action(() =>
			{
				pbTarget.Image = validationFunc(txt) ? System.Properties.Resources.tick_16 : System.Properties.Resources.block_16;
			});
			callback();
			txt.TextChanged += (s, e) => callback();
		}

		#endregion

		#region 跨线程回调

		/// <summary>
		/// 判断指定的控件是否正在销毁中
		/// </summary>
		/// <param name="control"></param>
		/// <returns></returns>
		public static bool IsHandleAvailable(this Control control)
		{
			return control != null && !(control.IsDisposed || control.Disposing);
		}

		/// <summary>
		/// 在UI线程上回调指定委托
		/// </summary>
		/// <param name="action">委托</param>
		public static void Invoke(this Control control, Action action)
		{
			if (control.InvokeRequired && control.IsHandleAvailable())
				control.Invoke(action);
			else action();
		}

		/// <summary>
		/// 在UI线程上回调指定委托
		/// </summary>
		/// <param name="action">委托</param>
		/// <param name="p">参数1</param>
		/// <typeparam name="T">参数类型2</typeparam>
		public static void Invoke<T>(this Control control, Action<T> action, T p)
		{
			if (control.InvokeRequired && control.IsHandleAvailable())
			{
				control.Invoke(action, p);
			}
			else action(p);
		}

		/// <summary>
		/// 在UI线程上回调指定委托
		/// </summary>
		/// <param name="action">委托</param>
		/// <param name="p1">参数1</param>
		/// <param name="p2">参数2</param>
		/// <typeparam name="T1">参数类型1</typeparam>
		/// <typeparam name="T2">参数类型2</typeparam>
		public static void Invoke<T1, T2>(this Control control, Action<T1, T2> action, T1 p1, T2 p2)
		{
			if (control.InvokeRequired && control.IsHandleAvailable())
			{
				control.Invoke(action, p1, p2);
			}
			else action(p1, p2);
		}

		/// <summary>
		/// 在UI线程上回调指定委托
		/// </summary>
		/// <param name="action">委托</param>
		/// <param name="p1">参数1</param>
		/// <param name="p2">参数2</param>
		/// <param name="p3">参数3</param>
		/// <typeparam name="T1">参数类型1</typeparam>
		/// <typeparam name="T2">参数类型2</typeparam>
		/// <typeparam name="T3">参数类型3</typeparam>
		public static void Invoke<T1, T2, T3>(this Control control, Action<T1, T2, T3> action, T1 p1, T2 p2, T3 p3)
		{
			if (control.InvokeRequired && control.IsHandleAvailable())
			{
				control.Invoke(action, p1, p2, p3);
			}
			else action(p1, p2, p3);
		}

		/// <summary>
		/// 在UI线程上回调指定委托
		/// </summary>
		/// <param name="action">委托</param>
		/// <param name="p1">参数1</param>
		/// <param name="p2">参数2</param>
		/// <param name="p3">参数3</param>
		/// <param name="p4">参数4</param>
		/// <typeparam name="T1">参数类型1</typeparam>
		/// <typeparam name="T2">参数类型2</typeparam>
		/// <typeparam name="T3">参数类型3</typeparam>
		/// <typeparam name="T4">参数类型4</typeparam>
		public static void Invoke<T1, T2, T3, T4>(this Control control, Action<T1, T2, T3, T4> action, T1 p1, T2 p2, T3 p3, T4 p4)
		{
			if (control.InvokeRequired && control.IsHandleAvailable())
			{
				control.Invoke(action, p1, p2, p3, p4);
			}
			else action(p1, p2, p3, p4);
		}

		/// <summary>
		/// 在UI上执行操作
		/// </summary>
		public static TR Invoke<T1, TR>(this Control control, Func<T1, TR> action, T1 p1)
		{
			if (control.InvokeRequired && control.IsHandleAvailable())
				return (TR)control.Invoke(action, p1);
			return action(p1);
		}

		/// <summary>
		/// 在UI上执行操作
		/// </summary>
		public static TR Invoke<T1, T2, TR>(this Control control, Func<T1, T2, TR> action, T1 p1, T2 p2)
		{
			if (control.InvokeRequired && control.IsHandleAvailable())
				return (TR)control.Invoke(action, p1, p2);
			return action(p1, p2);
		}

		/// <summary>
		/// 在UI上执行操作
		/// </summary>
		public static TR Invoke<T1, T2, T3, TR>(this Control control, Func<T1, T2, T3, TR> action, T1 p1, T2 p2, T3 p3)
		{
			if (control.InvokeRequired && control.IsHandleAvailable())
				return (TR)control.Invoke(action, p1, p2, p3);
			return action(p1, p2, p3);
		}

		/// <summary>
		/// 在UI上执行操作
		/// </summary>
		public static TR Invoke<T1, T2, T3, T4, TR>(this Control control, Func<T1, T2, T3, T4, TR> action, T1 p1, T2 p2, T3 p3, T4 p4)
		{
			if (control.InvokeRequired && control.IsHandleAvailable())
				return (TR)control.Invoke(action, p1, p2, p3, p4);
			return action(p1, p2, p3, p4);
		}


		#endregion


	}
}

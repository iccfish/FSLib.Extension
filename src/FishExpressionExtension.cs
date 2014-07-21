using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace System
{
	/// <summary>
	/// 反射或表达式用的扩展方法
	/// </summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static class FishExpressionExtension
	{
		/// <summary>
		/// 获得属性访问表达式（MemberExpression）中访问的属性名
		/// </summary>
		/// <typeparam name="TSource"></typeparam>
		/// <typeparam name="TValue"></typeparam>
		/// <param name="expression"></param>
		/// <returns></returns>
		public static string GetExpressionAccessedMemberName<TSource, TValue>(this Expression<Func<TSource, TValue>> expression)
		{
			if (expression == null)
				return null;

			if (expression.Body.NodeType != ExpressionType.MemberAccess || !((expression.Body as MemberExpression).Member is PropertyInfo))
				return null;

			return ((expression.Body as MemberExpression).Member as PropertyInfo).Name;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Xml
{
	using ComponentModel;

	[EditorBrowsable(EditorBrowsableState.Never)]
	public static class FishXmlExtensionMethods
	{
		/// <summary>
		/// 获得对应名称属性的值
		/// </summary>
		/// <param name="node">当前的节点</param>
		/// <param name="attName">属性名</param>
		/// <returns>属性值</returns>
		public static string GetAttributeValue(this XmlNode node, string attName)
		{
			if (node == null)
				return null;

			return node.Attributes[attName].SelectValue(s => s.Value);
		}
	}
}

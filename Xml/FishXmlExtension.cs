using System.ComponentModel;

namespace System.Xml
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static class FishXmlExtension
	{
		/// <summary>
		/// 获得属性值
		/// </summary>
		/// <param name="node">当前节点</param>
		/// <param name="attName">属性名</param>
		/// <returns>属性值</returns>
		public static string GetAttributeValue(this XmlNode node, string attName)
		{
			if (node == null || attName.IsNullOrEmpty()) return null;

			var att = node.Attributes[attName];
			if (att == null) return null;

			return att.Value;
		}
	}
}

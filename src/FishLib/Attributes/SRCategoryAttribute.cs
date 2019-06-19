using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSLib.Extension.Attributes
{
	using System.ComponentModel;

	/// <summary>
	/// 支持资源的类别属性
	/// </summary>
	[AttributeUsage(AttributeTargets.All)]
	public class SRCategoryAttribute : CategoryAttribute
	{
		public SRCategoryAttribute(Type resourceType, string category)
				: base(category)
		{
			_resourceType = resourceType;
		}

		Type _resourceType;


		/// <summary>
		/// 查阅指定类别的本地化名称。
		/// </summary>
		/// <returns>
		/// 类别的本地化名称；如果本地化名称不存在，则为 null。
		/// </returns>
		/// <param name="value">要查阅的类别的标识符。</param>
		protected override string GetLocalizedString(string value)
		{
			var str = LocalizeContext.GetResourceFromResourceManager(_resourceType, value);
			if (!string.IsNullOrEmpty(str))
				return str;

            return base.GetLocalizedString(value);
		}
	}
}

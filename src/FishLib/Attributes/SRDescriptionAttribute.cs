namespace FSLib.Extension.Attributes
{
	using System;
	using System.Linq;

	/// <summary>
	/// 支持使用资源文件进行本地化的描述属性
	/// </summary>
	[AttributeUsage(AttributeTargets.All)]
	public class SRDescriptionAttribute : System.ComponentModel.DescriptionAttribute
	{


		bool replaced = false;

		public SRDescriptionAttribute(Type resourceType, string description)
				: base(description)
		{
			_resourceType = resourceType;
		}

		Type _resourceType;


		/// <inheritdoc />
		public override string Description
		{
			get
			{
				if (!replaced)
				{
					replaced = true;
					var val = LocalizeContext.GetResourceFromResourceManager(_resourceType, base.DescriptionValue);
					if (!string.IsNullOrEmpty(val)) base.DescriptionValue = val;
				}
				return base.Description;
			}
		}
	}
}

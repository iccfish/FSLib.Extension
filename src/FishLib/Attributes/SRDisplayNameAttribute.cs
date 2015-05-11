namespace System.FishLib.Attributes
{
	using System;
	using System.Linq;

	[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
	[AttributeUsage(AttributeTargets.All)]
	public class SRDisplayNameAttribute : System.ComponentModel.DisplayNameAttribute
	{
		bool replaced = false;

		public SRDisplayNameAttribute(Type resourceType,string displayName)
			: base(displayName)
		{
			_resourceType = resourceType;
		}

		Type _resourceType;



		/// <inheritdoc />
		public override string DisplayName
		{
			get
			{
				if (!replaced)
				{
					replaced = true;

					var val = LocalizeContext.GetResourceFromResourceManager(_resourceType, base.DisplayName);
					if (!string.IsNullOrEmpty(val))
						base.DisplayNameValue = val;
				}
				return base.DisplayName;
			}
		}
	}
}

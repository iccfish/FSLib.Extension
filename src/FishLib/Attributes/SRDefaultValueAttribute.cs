namespace FSLib.Extension.Attributes
{
	using System;
	using System.Linq;

	/// <summary>
	/// 支持使用资源文件进行本地化的默认值属性
	/// </summary>
	[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
	public class SRDefaultValueAttribute : System.ComponentModel.DefaultValueAttribute
	{
		/// <summary>
		/// 建立  <see cref="SRDefaultValueAttribute"/> 的新实例
		/// </summary>
		/// <param name="type"></param>
		/// <param name="value"></param>
		public SRDefaultValueAttribute(Type type, Type resourceManagerType, string resourceKey)
			: base(null)
		{
			this._value = System.ComponentModel.TypeDescriptor.GetConverter(type).ConvertFrom(LocalizeContext.GetResourceFromResourceManager(resourceManagerType, resourceKey) ?? resourceKey);
		}

		public SRDefaultValueAttribute(char value)
			: base(value)
		{

		}
		public SRDefaultValueAttribute(byte value)
			: base(value)
		{

		}
		public SRDefaultValueAttribute(short value)
			: base(value)
		{

		}
		public SRDefaultValueAttribute(int value)
			: base(value)
		{

		}
		public SRDefaultValueAttribute(long value)
			: base(value)
		{

		}
		public SRDefaultValueAttribute(float value)
			: base(value)
		{

		}
		public SRDefaultValueAttribute(double value)
			: base(value)
		{

		}
		public SRDefaultValueAttribute(bool value)
			: base(value)
		{

		}
		public SRDefaultValueAttribute(string value)
			: base(value)
		{
		}

		/// <summary>
		/// 建立  <see cref="SRDefaultValueAttribute"/> 的新实例
		/// </summary>
		public SRDefaultValueAttribute(Type resourceManagerType, string resourceKey)
			: base(null)
		{
			this._value = LocalizeContext.GetResourceFromResourceManager(resourceManagerType, resourceKey) ?? resourceKey;
		}

		public SRDefaultValueAttribute(object value)
			: base(value)
		{

		}

		object _value;

		public override object Value
		{
			get
			{
				return _value ?? base.Value;
			}
		}
	}
}

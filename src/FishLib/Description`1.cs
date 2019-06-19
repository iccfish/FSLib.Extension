namespace FSLib.Extension
{
	using System.Linq;
	using System.Reflection;

	using SmartAssembly.Attributes;

	/// <summary>
	/// 泛型的描述项目
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class Description<T> : Description
	{
		/// <summary>
		/// 创建 <see cref="Description"></see> 的新实例
		/// </summary>
		[DoNotPrune]
		public Description(string displayName, string descriptionText, object value, FieldInfo field)
			: base(displayName, descriptionText, value, field)
		{

		}
		/// <summary>
		/// 获得或设置泛型值
		/// </summary>
		public new T Value
		{
			get
			{
				return (T)base.Value;
			}
			set
			{
				base.Value = value;
			}
		}
	}
}

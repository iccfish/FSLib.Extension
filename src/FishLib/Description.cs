namespace FSLib.Extension.FishLib
{
	using System.Linq;
	using System.Reflection;

	using SmartAssembly.Attributes;

	/// <summary>
	/// 描述项目
	/// </summary>
	public class Description
	{
		/// <summary>
		/// 显示名称
		/// </summary>
		public string DisplayName { get; private set; }

		/// <summary>
		/// 描述
		/// </summary>
		public string DescriptionText { get; private set; }

		/// <summary>
		/// 值
		/// </summary>
		public virtual object Value { get; protected set; }

		/// <summary>
		/// 是否是标记位
		/// </summary>
		public bool IsFlag { get; internal set; }

		/// <summary>
		/// 获得相关联的类型
		/// </summary>
		public FieldInfo Field { get; private set; }

		/// <summary>
		/// 创建 <see cref="Description" /> 的新实例
		/// </summary>
		public Description(string displayName, string descriptionText, object value, FieldInfo field)
		{
			DisplayName = displayName;
			DescriptionText = descriptionText;
			Value = value;
			Field = field;
		}

		public override string ToString()
		{
			return this.DisplayName;
		}

		/// <summary>
		/// 获得值的表达式形式
		/// </summary>
		/// <returns></returns>
		public virtual string GetValueString()
		{
			return Value.ToString();
		}
	}

	/// <summary>
	/// 描述项目
	/// </summary>
	[DoNotPruneType]
	public class DescriptionGeneric<T> : Description
	{
		/// <summary>
		/// 创建 <see cref="DescriptionGeneric"></see> 的新实例
		/// </summary>
		public DescriptionGeneric(string displayName, string descriptionText, object value, FieldInfo field)
			: base(displayName, descriptionText, value, field)
		{

		}

		/// <summary>
		/// 值
		/// </summary>
		public new T Value
		{
			get
			{
				return (T)base.Value;
			}
			protected set
			{
				base.Value = value;
			}
		}

		/// <summary>
		/// 获得值的表达式形式
		/// </summary>
		/// <returns></returns>
		public override string GetValueString()
		{
			return Value.ToString();
		}
	}
}

namespace FSLib.Extension
{
	using System;
	using System.Linq;
	using System.Reflection;

	using Attributes;

	/// <summary>
	/// 类型描述基类
	/// </summary>
	public class MemberDescriptorBase
	{
		/// <summary>
		/// 创建 <see cref="MemberDescriptorBase" /> 的新实例
		/// </summary>
		public MemberDescriptorBase(MemberInfo type)
		{
			Member = type;
		}

		/// <summary>
		/// 获得或设置包含的基类
		/// </summary>
		public MemberInfo Member { get; private set; }

		/// <summary>
		/// 当前的类名
		/// </summary>
		public string Name { get { return Member.Name; } }



		private System.Attribute[] _attributes;
		/// <summary>
		/// 获得行为的自定义属性
		/// </summary>
		public System.Attribute[] Attributes
		{
			get
			{
				return _attributes ?? (_attributes = this.Member.GetCustomAttributes(true).Cast<System.Attribute>().ToArray());
			}
		}


		#region 隐式转换

		/// <summary>
		/// 隐式转换为 <see cref="System.Type"/>
		/// </summary>
		/// <param name="db">要转换的对象</param>
		/// <returns>返回 <see cref="MemberDescriptorBase"/> 所表示的 <see cref="System.Type"/> 对象</returns>
		public static implicit operator MemberInfo(MemberDescriptorBase db)
		{
			return db.Member;
		}

		/// <summary>
		/// 隐式转换为 <see cref="MemberDescriptorBase"/>
		/// </summary>
		/// <param name="type">要转换的对象</param>
		/// <returns>返回包含着 <see cref="System.Type"/> 类型的 <see cref="MemberDescriptorBase"/> 对象</returns>
		public static implicit operator MemberDescriptorBase(MemberInfo type)
		{
			return new MemberDescriptorBase(type);
		}


		#endregion
		/// <summary>
		/// 根据类型获得属性
		/// </summary>
		/// <typeparam name="T">属性类型</typeparam>
		/// <returns></returns>
		public T[] FindAttributes<T>()
				where T : class
		{
			return Attributes.Where(s => s is T).Cast<T>().ToArray();
		}
		/// <summary>
		/// 根据类型获得属性
		/// </summary>
		/// <typeparam name="T">属性类型</typeparam>
		/// <returns></returns>
		public T FindAttribute<T>()
				where T : class
		{
			Attribute first = null;
			foreach (var s in Attributes)
			{
				if (s is T)
				{
					first = s;
					break;
				}
			}
			return first as T;
		}

		/// <summary>
		/// 获得显示的描述
		/// </summary>
		public virtual string Description
		{
			get
			{
				var dt = FindAttribute<System.ComponentModel.DescriptionAttribute>();

				return dt == null ? "" : dt.Description;
			}
		}
		/// <summary>
		/// 获得显示名称
		/// </summary>
		public virtual string DisplayName
		{
			get
			{
				var dnat = FindAttribute<System.ComponentModel.DisplayNameAttribute>() ?? FindAttribute<DisplayNameWAttribute>();
				HasAliasName = dnat != null;
				return dnat == null ? this.Name : dnat.DisplayName;
			}
		}

		/// <summary>
		/// 获得或设置当前的名称是否是通过别名获得的
		/// </summary>
		public bool HasAliasName
		{
			get;
			private set;
		}
	}
}

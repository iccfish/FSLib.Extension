using System.Collections.Generic;
using System.ComponentModel;

// ReSharper disable once CheckNamespace
namespace System
{
	using FishLib;
	using System.Reflection;

	/// <summary>
	/// 枚举的扩展
	/// </summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static class FsLibEnumExtension
	{
		static readonly Dictionary<Type, List<Description>> EnumDescriptionCache = new Dictionary<Type, List<Description>>();
		static readonly object LockObject = new object();


		/// <summary>
		/// 获得描述
		/// </summary>
		/// <param name="type">枚举类型参数</param>
		/// <returns></returns>
		public static List<Description> GetEnumDescription(this Type type)
		{
#if NET_CORE
			var typeInfo = type?.GetTypeInfo();
#else
			var typeInfo = type;
#endif
			if (type == null || typeInfo.BaseType != typeof(Enum)) throw new InvalidOperationException();

			var result = EnumDescriptionCache.GetValue(type);
			if (result == null)
			{
				lock (LockObject)
				{
					if (result == null)
					{
						var fields = typeInfo.GetFields();
						result = new List<Description>(fields.Length);

						foreach (var f in fields)
						{
							if (f.IsSpecialName || !f.IsPublic) continue;

							var value = f.GetRawConstantValue();
							var typeWrapper = (MemberDescriptorBase)f;

							//创建泛型类
							var m = typeof(DescriptionGeneric<>).MakeGenericType(type);
							var desc = (Description)Activator.CreateInstance(m, typeWrapper.DisplayName, typeWrapper.Description, value, f);

							result.Add(desc);
						}

						EnumDescriptionCache.Add(type, result);
					}
				}
			}

			return result;
		}

		/// <summary>
		/// 获得描述
		/// </summary>
		/// <returns></returns>
		public static List<DescriptionGeneric<T>> GetEnumDescription<T>()
		{
			var type = FishObjectExtension.GetTypeInfo(typeof(T));

			if (type.BaseType != typeof(Enum))
				throw new InvalidOperationException();

			var fields = type.GetFields();
			var list = new List<DescriptionGeneric<T>>(fields.Length);

			foreach (var f in fields)
			{
				if (f.IsSpecialName || !f.IsPublic) continue;

				var value = f.GetRawConstantValue();
				var typeWrapper = (MemberDescriptorBase)f;

				list.Add(new DescriptionGeneric<T>(typeWrapper.DisplayName, typeWrapper.Description, value, f));
			}

			return list;
		}
	}
}

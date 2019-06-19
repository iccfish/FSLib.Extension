namespace FSLib.Extension
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Reflection;
	using System.Linq;

	/// <summary>
	/// 本地化上下文环境
	/// </summary>
	internal static class LocalizeContext
	{
		static Dictionary<Type, System.Resources.ResourceManager> _resourceDictionary;

		static LocalizeContext()
		{
			_resourceDictionary = new Dictionary<Type, System.Resources.ResourceManager>();
		}

		/// <summary>
		/// 获得指定类型的资源管理类
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static System.Resources.ResourceManager GetAttributeResourceManager(Type type)
		{
			if (type == null) return null;


			var rm = _resourceDictionary.GetValue(type);
			if (rm == null)
			{
#if NETSTANDARD1_6_1 || NETSTANDARD2_0 || NETSTANDARD3_0
				var prop = type.GetTypeInfo().GetProperty("ResourceManager", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
#else
				var prop = type.GetProperty("ResourceManager", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
#endif
				rm = (System.Resources.ResourceManager)prop.GetValue(null, null);
				_resourceDictionary.Add(type, rm);
			}

			return rm;
		}

		/// <summary>
		/// 从指定的强类型资源类中加载指定的字符串
		/// </summary>
		/// <param name="type"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public static string GetResourceFromResourceManager(Type type, string key)
		{
			var rm = GetAttributeResourceManager(type);
			return rm == null ? null : rm.GetString(key);
		}

#if !NETSTANDARD1_6_1 && !NETSTANDARD2_0 && !NETSTANDARD3_0

		/// <summary>
		/// 从指定的强类型资源类中加载指定的字符串
		/// </summary>
		/// <param name="type"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public static Stream GetResourceStreamFromResourceManager(Type type, string key)
		{
			var rm = GetAttributeResourceManager(type);
			return rm == null ? null : rm.GetStream(key);
		}
#endif
	}
}

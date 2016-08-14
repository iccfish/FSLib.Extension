namespace System.FishLib
{
	using System;
	using System.Linq;
	using System.Reflection;

	using Globalization;

	using Linq.Expressions;

	/// <summary>
	/// 反射辅助类
	/// </summary>
	public static class Reflection
	{
#if !NET_CORE
		/// <summary>
		/// 尝试加载程序集，如果加载失败，则返回NULL
		/// </summary>
		/// <param name="filePath">要加载的文件路径</param>
		/// <returns><see cref="T:System.Reflection.Assembly"/></returns>
		public static Assembly TryLoadAssemblyFrom(string filePath)
		{
			try
			{
				return System.Reflection.Assembly.LoadFrom(filePath);
			}
			catch (Exception)
			{
				return null;
			}
		}

#endif

#if NET40
		/// <summary>
		/// 动态构造委托
		/// </summary>
		/// <param name="methodInfo">方法元数据</param>
		/// <returns>委托</returns>
		public static Delegate BuildDynamicDelegate(this MethodInfo methodInfo)
		{
			if (methodInfo == null)
				throw new ArgumentNullException("methodInfo");

			var paramExpressions = methodInfo.GetParameters().Select((p, i) =>
			{
				var name = "param" + (i + 1).ToString(CultureInfo.InvariantCulture);
				return Expression.Parameter(p.ParameterType, name);
			}).ToList();

			MethodCallExpression callExpression;
			if (methodInfo.IsStatic)
			{
				//Call(params....)
				callExpression = Expression.Call(methodInfo, paramExpressions);
			}
			else
			{
				var instanceExpression = Expression.Parameter(methodInfo.ReflectedType, "instance");
				//insatnce.Call(params….)
				callExpression = Expression.Call(instanceExpression, methodInfo, paramExpressions);
				paramExpressions.Insert(0, instanceExpression);
			}
			var lambdaExpression = Expression.Lambda(callExpression, paramExpressions);
			return lambdaExpression.Compile();
		}

		/// <summary>
		/// 创建赋值委托
		/// </summary>
		/// <typeparam name="TInstance"></typeparam>
		/// <typeparam name="TProperty"></typeparam>
		/// <param name="propertyInfo"></param>
		/// <returns></returns>
		public static Action<TInstance, TProperty> BuildSetPropertyAction<TInstance, TProperty>(this PropertyInfo propertyInfo)
		{
			var instanceParam = Expression.Parameter(typeof(TInstance), "instance");
			var valueParam = Expression.Parameter(typeof(TProperty), "value");
			//instance.Property
			var propertyProperty = Expression.Property(instanceParam, propertyInfo);
			//instance.Property = value
			var assignExpression = Expression.Assign(propertyProperty, valueParam);
			var lambdaExpression = Expression.Lambda<Action<TInstance, TProperty>>(assignExpression, instanceParam, valueParam);
			return lambdaExpression.Compile();
		}

#endif

	}
}

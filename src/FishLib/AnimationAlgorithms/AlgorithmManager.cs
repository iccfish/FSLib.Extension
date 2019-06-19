using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSLib.Extension.AnimationAlgorithms
{
	using System.Reflection;

	/// <summary>
	/// 算法管理器
	/// </summary>
	public static class AlgorithmManager
	{
		/// <summary>
		/// 获得当前可用的算法列表
		/// </summary>
		public static Dictionary<string, ITweenPositionAlgorithm> CurrentAlgorithmManager { get; private set; }

		static AlgorithmManager()
		{
			CurrentAlgorithmManager = typeof(AlgorithmManager).GetTypeInfo().Assembly
				.GetExportedTypes().Where(s =>
				{
					var ti = s.GetTypeInfo();
					return !ti.IsAbstract && ti.GetInterface(typeof(ITweenPositionAlgorithm).FullName) != null;
				}).Select(s => Activator.CreateInstance(s) as ITweenPositionAlgorithm).ToDictionary(s => s.GetType().Name.Replace("TweenPositionAlgorithm", ""), s => s);
		}
	}
}

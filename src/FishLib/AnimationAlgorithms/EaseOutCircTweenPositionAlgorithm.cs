using System;

namespace FSLib.Extension.AnimationAlgorithms
{
	/// <summary>
	/// EaseOutCirc
	/// </summary>
	public class EaseOutCircTweenPositionAlgorithm : TweenPositionAlgorithm
	{
		#region Overrides of TweenPositionAlgorithm

		/// <summary>
		/// 获得在指定的时间刻度上应该处于的位置
		/// </summary>
		/// <param name="pos"></param>
		/// <returns></returns>
		public override double GetPosition(double pos)
		{
			return Math.Sqrt(1 - Math.Pow((pos - 1), 2));
		}

		#endregion
	}
}
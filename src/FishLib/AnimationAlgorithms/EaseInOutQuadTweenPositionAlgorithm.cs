using System;

namespace System.FishLib.AnimationAlgorithms
{
	/// <summary>
	/// EaseInOutQuad 算法
	/// </summary>
	public class EaseInOutQuadTweenPositionAlgorithm : TweenPositionAlgorithm
	{
		#region Overrides of TweenPositionAlgorithm

		/// <summary>
		/// 获得在指定的时间刻度上应该处于的位置
		/// </summary>
		/// <param name="pos"></param>
		/// <returns></returns>
		public override double GetPosition(double pos)
		{
			if ((pos /= 0.5) < 1)
				return 0.5 * Math.Pow(pos, 2);
			return -0.5 * ((pos -= 2) * pos - 2);

		}

		#endregion
	}
}
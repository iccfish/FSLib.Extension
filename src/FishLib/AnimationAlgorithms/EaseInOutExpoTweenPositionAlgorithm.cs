using System;

namespace FSLib.Extension.AnimationAlgorithms
{
	/// <summary>
	/// EaseInOutExpo
	/// </summary>
	public class EaseInOutExpoTweenPositionAlgorithm : TweenPositionAlgorithm
	{
		#region Overrides of TweenPositionAlgorithm

		/// <summary>
		/// 获得在指定的时间刻度上应该处于的位置
		/// </summary>
		/// <param name="pos"></param>
		/// <returns></returns>
		public override double GetPosition(double pos)
		{
			if (pos < 0.005)
				return 0;
			if (pos > 0.995)
				return 1;
			if ((pos /= 0.5) < 1)
				return 0.5 * Math.Pow(2, 10 * (pos - 1));
			return 0.5 * (-Math.Pow(2, -10 * --pos) + 2);
		}

		#endregion
	}
}
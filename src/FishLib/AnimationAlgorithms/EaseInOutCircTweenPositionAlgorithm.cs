using System;

namespace FSLib.Extension.FishLib.AnimationAlgorithms
{
	/// <summary>
	/// EaseInOutCirc
	/// </summary>
	public class EaseInOutCircTweenPositionAlgorithm : TweenPositionAlgorithm
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
				return -0.5 * (Math.Sqrt(1 - pos * pos) - 1);
			return 0.5 * (Math.Sqrt(1 - (pos -= 2) * pos) + 1);

		}

		#endregion
	}
}
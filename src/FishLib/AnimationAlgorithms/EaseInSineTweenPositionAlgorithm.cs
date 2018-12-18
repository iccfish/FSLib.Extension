using System;

namespace FSLib.Extension.FishLib.AnimationAlgorithms
{
	/// <summary>
	/// EaseInSine
	/// </summary>
	public class EaseInSineTweenPositionAlgorithm : TweenPositionAlgorithm
	{
		#region Overrides of TweenPositionAlgorithm

		/// <summary>
		/// 获得在指定的时间刻度上应该处于的位置
		/// </summary>
		/// <param name="pos"></param>
		/// <returns></returns>
		public override double GetPosition(double pos)
		{
			return -Math.Cos(pos * (Math.PI / 2)) + 1;
		}

		#endregion
	}
}
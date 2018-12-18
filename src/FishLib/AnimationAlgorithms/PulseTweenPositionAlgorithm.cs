using System;

namespace System.FishLib.AnimationAlgorithms
{
	/// <summary>
	/// Pulse
	/// </summary>
	public class PulseTweenPositionAlgorithm : TweenPositionAlgorithm
	{
		#region Overrides of TweenPositionAlgorithm

		/// <summary>
		/// 获得在指定的时间刻度上应该处于的位置
		/// </summary>
		/// <param name="pos"></param>
		/// <returns></returns>
		public override double GetPosition(double pos)
		{
			return (-Math.Cos((pos * (5 - .5) * 2) * Math.PI) / 2) + .5;
		}

		#endregion
	}
}
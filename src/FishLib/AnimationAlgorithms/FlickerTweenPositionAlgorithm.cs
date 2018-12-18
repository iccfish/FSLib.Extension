using System;

namespace FSLib.Extension.FishLib.AnimationAlgorithms
{
	/// <summary>
	/// Flicker
	/// </summary>
	public class FlickerTweenPositionAlgorithm : TweenPositionAlgorithm
	{
		SinusoidalTweenPositionAlgorithm _st = new SinusoidalTweenPositionAlgorithm();
		Random _ran = new Random();

		#region Overrides of TweenPositionAlgorithm

		/// <summary>
		/// 获得在指定的时间刻度上应该处于的位置
		/// </summary>
		/// <param name="pos"></param>
		/// <returns></returns>
		public override double GetPosition(double pos)
		{
			pos = pos + (_ran.NextDouble() - 0.5) / 5.0;
			return _st.GetPosition(pos < 0 ? 0 : pos > 1 ? 1 : pos);

		}

		#endregion
	}
}
namespace FSLib.Extension.FishLib.AnimationAlgorithms
{
	/// <summary>
	/// EaseOutBounce
	/// </summary>
	public class EaseOutBounceTweenPositionAlgorithm : TweenPositionAlgorithm
	{
		#region Overrides of TweenPositionAlgorithm

		/// <summary>
		/// 获得在指定的时间刻度上应该处于的位置
		/// </summary>
		/// <param name="pos"></param>
		/// <returns></returns>
		public override double GetPosition(double pos)
		{
			if ((pos) < (1 / 2.75))
			{
				return (7.5625 * pos * pos);
			}
			if (pos < (2 / 2.75))
			{
				return (7.5625 * (pos -= (1.5 / 2.75)) * pos + .75);
			}
			if (pos < (2.5 / 2.75))
			{
				return (7.5625 * (pos -= (2.25 / 2.75)) * pos + .9375);
			}
			return (7.5625 * (pos -= (2.625 / 2.75)) * pos + .984375);
		}

		#endregion
	}
}
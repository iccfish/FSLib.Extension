namespace FSLib.Extension.AnimationAlgorithms
{
	/// <summary>
	/// Mirror
	/// </summary>
	public class MirrorTweenPositionAlgorithm : TweenPositionAlgorithm
	{
		SinusoidalTweenPositionAlgorithm _st = new SinusoidalTweenPositionAlgorithm();

		#region Overrides of TweenPositionAlgorithm

		/// <summary>
		/// 获得在指定的时间刻度上应该处于的位置
		/// </summary>
		/// <param name="pos"></param>
		/// <returns></returns>
		public override double GetPosition(double pos)
		{
			if (pos < 0.5)
				return _st.GetPosition(pos * 2);
			return _st.GetPosition(1 - (pos - 0.5) * 2);
		}

		#endregion
	}
}
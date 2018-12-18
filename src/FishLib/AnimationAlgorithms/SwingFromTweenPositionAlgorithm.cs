namespace FSLib.Extension.FishLib.AnimationAlgorithms
{
	/// <summary>
	/// SwingFrom
	/// </summary>
	public class SwingFromTweenPositionAlgorithm : TweenPositionAlgorithm
	{
		#region Overrides of TweenPositionAlgorithm

		/// <summary>
		/// 获得在指定的时间刻度上应该处于的位置
		/// </summary>
		/// <param name="pos"></param>
		/// <returns></returns>
		public override double GetPosition(double pos)
		{
			var s = 1.70158;
			return pos * pos * ((s + 1) * pos - s);

		}

		#endregion
	}
}
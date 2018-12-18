namespace System.FishLib.AnimationAlgorithms
{
	/// <summary>
	/// Reverse
	/// </summary>
	public class ReverseTweenPositionAlgorithm : TweenPositionAlgorithm
	{
		#region Overrides of TweenPositionAlgorithm

		/// <summary>
		/// 获得在指定的时间刻度上应该处于的位置
		/// </summary>
		/// <param name="pos"></param>
		/// <returns></returns>
		public override double GetPosition(double pos)
		{
			return 1 - pos;
		}

		#endregion
	}
}
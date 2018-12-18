namespace System.FishLib.AnimationAlgorithms
{
	/// <summary>
	/// 位置算法接口
	/// </summary>
	public interface ITweenPositionAlgorithm
	{
		/// <summary>
		/// 获得在指定的时间刻度上应该处于的位置
		/// </summary>
		/// <param name="pos"></param>
		/// <returns></returns>
		double GetPosition(double pos);

		/// <summary>
		/// 计算在指定的时间刻度上应该处于的位置
		/// </summary>
		/// <param name="initialPos">初始化位置</param>
		/// <param name="pos">当前位置</param>
		/// <returns>应该处于的位置</returns>
		int GetPosition(int initialPos, double pos);

		/// <summary>
		/// 计算在指定的时间刻度上应该处于的位置
		/// </summary>
		/// <param name="initialPos">初始化位置</param>
		/// <param name="pos">当前相对位置</param>
		/// <param name="totalLength">位置变化总长度单位</param>
		/// <returns>应该处于的位置</returns>
		int GetPosition(int initialPos, double pos, double totalLength);
	}
}
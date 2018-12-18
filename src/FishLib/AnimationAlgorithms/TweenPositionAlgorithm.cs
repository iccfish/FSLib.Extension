using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSLib.Extension.FishLib.AnimationAlgorithms
{
	/// <summary>
	/// 位置算法基类
	/// </summary>
	public abstract class TweenPositionAlgorithm : ITweenPositionAlgorithm
	{
		#region Implementation of ITweenPositionAlgorithm

		/// <summary>
		/// 获得在指定的时间刻度上应该处于的位置
		/// </summary>
		/// <param name="pos"></param>
		/// <returns></returns>
		public abstract double GetPosition(double pos);

		/// <summary>
		/// 计算在指定的时间刻度上应该处于的位置
		/// </summary>
		/// <param name="initialPos">初始化位置</param>
		/// <param name="pos">当前位置</param>
		/// <returns>应该处于的位置</returns>
		public int GetPosition(int initialPos, double pos)
		{
			return initialPos + (int)GetPosition(pos);
		}

		/// <summary>
		/// 计算在指定的时间刻度上应该处于的位置
		/// </summary>
		/// <param name="initialPos">初始化位置</param>
		/// <param name="pos">当前相对位置</param>
		/// <param name="totalLength">位置变化总长度单位</param>
		/// <returns>应该处于的位置</returns>
		public int GetPosition(int initialPos, double pos, double totalLength)
		{
			return initialPos + (int)GetPosition(pos / totalLength);
		}


		#endregion
	}
}

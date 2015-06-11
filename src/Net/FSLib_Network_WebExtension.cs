using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Net
{
	/// <summary>
	/// WebException的辅助类
	/// </summary>
	public static class FSLib_Network_WebExtension
	{
		#region WebException 扩展

		/// <summary>
		/// 判断当前的请求是不是 <see cref="HttpWebResponse"/>
		/// </summary>
		/// <param name="e">包含异常的事件数据</param>
		/// <returns>如果是 <see cref="HttpWebResponse"/> ，则返回 true</returns>
		public static bool IsHttpResponse(this WebException e)
		{
			return e.Response != null && e.Response is HttpWebResponse;
		}

		/// <summary>
		/// 获得 <see cref="WebException"/> 所对应的 <see cref="HttpWebResponse"/>
		/// </summary>
		/// <param name="e"></param>
		/// <returns></returns>
		public static HttpWebResponse AsHttpWebResponse(this WebException e)
		{
			if (!e.IsHttpResponse()) return null;
			else return e.Response as HttpWebResponse;
		}

		#endregion

		#region HttpWebResponse扩展

		/// <summary>
		/// 判断当前的 <see cref="HttpWebResponse"/> 是不是重定向的请求
		/// </summary>
		/// <param name="response"></param>
		/// <returns></returns>
		public static bool IsRedirectHttpWebResponse(this HttpWebResponse response)
		{
			return response.StatusCode == HttpStatusCode.Ambiguous || // 300 
					response.StatusCode == HttpStatusCode.Moved || // 301 
					response.StatusCode == HttpStatusCode.Redirect || // 302
					response.StatusCode == HttpStatusCode.RedirectMethod || // 303 
					response.StatusCode == HttpStatusCode.RedirectKeepVerb;// 307
		}


		#endregion

		#region 标头扩展


		#endregion
	}
}

#if NET20 || NET35 || NET40 || NET45 || NET46 || NET47
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Linq;
using System.ComponentModel;

namespace System.Data
{
	using Common;

	using Configuration;

	using FSLib.Extension;

	/// <summary>
	/// 通用数据库的扩展方法
	/// </summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static class FSLibCommonExtension
	{
		#region 连接

		/// <summary>
		/// 清除指定数据表中的数据
		/// </summary>
		/// <param name="connection">数据库连接</param>
		/// <param name="tableName">数据表名</param>
		public static void ClearTable(this IDbConnection connection, string tableName)
		{
			using (IDbCommand command = connection.CreateCommand())
			{
				command.CommandText = string.Format("DELETE FROM [{0}]", tableName);
				command.CommandType = CommandType.Text;
				bool isClosed = connection.State == ConnectionState.Closed;

				try
				{
					if (isClosed) connection.Open();
					command.ExecuteNonQuery();
				}
				finally
				{
					if (isClosed && connection.State == ConnectionState.Open) connection.Close();
				}
			}
		}

		/// <summary>
		/// 判断一个连接是否正处于打开状态
		/// </summary>
		/// <param name="connection"></param>
		/// <returns></returns>
		public static bool IsOpen(this IDbConnection connection)
		{
			return connection != null && connection.State == ConnectionState.Open;
		}

		/// <summary>
		/// 通过指定的数据库连接配置创建一个 <see cref="DbConnection"/>
		/// </summary>
		/// <param name="setting">连接字符串</param>
		/// <returns></returns>
		[CanBeNull]
		public static DbConnection CreateDbConnection([NotNull] this ConnectionStringSettings setting)
		{
			var dbFactory = DbProviderFactories.GetFactory(setting.ProviderName);
			var conn = dbFactory.CreateConnection();
			if (conn != null)
				conn.ConnectionString = setting.ConnectionString;
			return conn;
		}

		/// <summary>
		/// 通过指定的数据库连接配置获得对应的 <see cref="DbProviderFactory"/>
		/// </summary>
		/// <param name="setting">连接字符串</param>
		/// <returns></returns>
		[NotNull]
		public static DbProviderFactory GetDbProviderFactory([NotNull] this ConnectionStringSettings setting)
		{
			return DbProviderFactories.GetFactory(setting.ProviderName);
		}

		#endregion

		#region Command


		/// <summary>
		/// 创建数据库命令
		/// </summary>
		/// <param name="connectionString">连接字符串</param>
		/// <param name="sql">要执行的语句</param>
		/// <param name="isProcedure">是否是存储过程</param>
		/// <returns>创建的结果</returns>
		public static IDbCommand CreateCommand(this IDbConnection connection, string sql, bool isProcedure)
		{
			IDbCommand command = connection.CreateCommand();
			command.CommandText = sql;
			command.CommandType = isProcedure ? CommandType.StoredProcedure : CommandType.Text;
			return command;
		}

		/// <summary>
		/// 运行命令-NonQuery方式
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		public static int RunCommandNonQuery(this IDbCommand command)
		{
			return command.RunCommandNonQuery(null);
		}

		/// <summary>
		/// 运行命令-NonQuery方式
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		public static int RunCommandNonQuery(this IDbCommand command, bool? closeConnection)
		{
			var alreadyOpen = command.Connection.State == ConnectionState.Open;

			try
			{
				if (!alreadyOpen) command.Connection.Open();
				return command.ExecuteNonQuery();
			}
			catch (Exception ex)
			{
				if (command.Connection.State != ConnectionState.Closed)
				{
					if (closeConnection.HasValue) command.Connection.Close();
					else if (!alreadyOpen) command.Connection.Close();
				}
				throw ex;
			}
			finally
			{
				if (command.Connection.State != ConnectionState.Closed)
				{
					if (closeConnection.HasValue) command.Connection.Close();
					else if (!alreadyOpen) command.Connection.Close();
				}

			}
		}

		/// <summary>
		/// 运行命令-Scalar方式
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		public static object RunCommandScalar(this IDbCommand command)
		{
			return command.RunCommandScalar(null);
		}

		/// <summary>
		/// 运行命令-Scalar方式
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		public static object RunCommandScalar(this IDbCommand command, bool? closeConnection)
		{
			var alreadyOpen = command.Connection.IsOpen();
			object r = null;
			Exception e = null;
			try
			{
				if (!alreadyOpen) command.Connection.Open();

				r = command.ExecuteScalar();
			}
			catch (Exception ex)
			{
				e = ex;
			}
			finally
			{

				if (closeConnection.HasValue && closeConnection.Value) command.Connection.Close();
				else if (!alreadyOpen) command.Connection.Close();
			}
			if (e != null)
			{
				throw e;
			}
			if (r == DBNull.Value)
				r = null;

			return r;
		}


		/// <summary>
		/// 运行命令-Scalar方式
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		public static T RunCommandScalar<T>(this IDbCommand command)
		{
			return (T)command.RunCommandScalar<T>(null);
		}

		/// <summary>
		/// 运行命令-Scalar方式
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		public static T RunCommandScalar<T>(this IDbCommand command, bool? closeConnection)
		{
			return (T)command.RunCommandScalar(closeConnection);
		}

		/// <summary>
		/// 运行命令-DataReader方式
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		public static IDataReader RunCommandReader(this IDbCommand command)
		{
			return command.RunCommandReader(true);
		}

		/// <summary>
		/// 运行命令-DataReader方式
		/// </summary>
		/// <param name="command">命令</param>
		/// <param name="closeConnection">是否加入随DataReader关闭连接选项</param>
		/// <returns></returns>
		public static IDataReader RunCommandReader(this IDbCommand command, bool? closeConnection)
		{
			var alreadyOpen = command.Connection.IsOpen();
			if (!alreadyOpen) command.Connection.Open();

			IDataReader result = null;
			try
			{
				result = command.ExecuteReader((closeConnection.HasValue && closeConnection.Value) || !alreadyOpen ? CommandBehavior.CloseConnection : CommandBehavior.Default);
			}
			catch (Exception ex)
			{
				if (!alreadyOpen && command.Connection.State != ConnectionState.Closed)
				{
					command.Connection.Close();
				}
				throw ex;
			}
			return result;
		}

		/// <summary>
		/// 运行命令-DataReader方式
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		public static IEnumerable<T> RunCommandReader<T>(this IDbCommand command, Func<IDataReader, T> convertor)
		{
			using (var dr = command.RunCommandReader())
			{
				while (dr.Read())
				{
					yield return convertor(dr);
				}
				dr.Close();
			}
		}

		/// <summary>
		/// 运行命令-DataReader方式
		/// </summary>
		/// <param name="command">命令</param>
		/// <param name="closeConnection">是否加入随DataReader关闭连接选项</param>
		/// <returns></returns>
		public static IEnumerable<T> RunCommandReader<T>(this IDbCommand command, Func<IDataReader, T> convertor, bool? closeConnection)
		{
			using (var dr = command.RunCommandReader(closeConnection))
			{
				while (dr.Read())
				{
					yield return convertor(dr);
				}
				dr.Close();
			}
		}

		/// <summary>
		/// 运行命令,并返回结果
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="command"></param>
		/// <param name="convertor"></param>
		/// <param name="closeConnection"></param>
		/// <returns></returns>
		public static List<T> ToList<T>(this IDbCommand command, Func<IDataReader, T> convertor, bool? closeConnection)
		{
			return command.RunCommandReader<T>(convertor, closeConnection).ToList();
		}


		/// <summary>
		/// 运行命令,并返回结果
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="command"></param>
		/// <param name="convertor"></param>
		/// <param name="closeConnection"></param>
		/// <returns></returns>
		public static List<T> ToList<T>(this IDbCommand command, Func<IDataReader, T> convertor)
		{
			return command.RunCommandReader<T>(convertor).ToList();
		}


		#endregion

	}
}
#endif
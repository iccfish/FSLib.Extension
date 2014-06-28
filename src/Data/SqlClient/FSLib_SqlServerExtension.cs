using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.ComponentModel;
using FSLib.Data;

namespace System.Data.SqlClient
{
	/// <summary>
	/// 针对Sql Server的扩展类
	/// </summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static class FSLib_SqlServerExtension
	{

		#region 数据连接

		/// <summary>
		/// 创建数据库连接
		/// </summary>
		/// <returns></returns>
		public static SqlConnection CreateConnection(this string conn)
		{
			return new SqlConnection(conn);
		}


		/// <summary>
		/// 创建数据库命令
		/// </summary>
		/// <param name="connection"></param>
		/// <param name="sql"></param>
		/// <returns></returns>
		public static SqlCommand CreateQueryCommand(this SqlConnection connection, string sql)
		{
			return new SqlCommand(sql, connection)
			{
				CommandType = CommandType.Text
			};
		}

		/// <summary>
		/// 创建数据库命令
		/// </summary>
		/// <param name="connection"></param>
		/// <param name="sql"></param>
		/// <param name="isProcedure"></param>
		/// <returns></returns>
		public static SqlCommand CreateProcedureCommand(this SqlConnection connection, string sql)
		{
			return new SqlCommand(sql, connection)
			{
				CommandType = CommandType.StoredProcedure
			};
		}


		/// <summary>
		/// 更改主键列的自增值开始数据
		/// </summary>
		/// <param name="connection">数据库连接</param>
		/// <param name="tableName">数据表名</param>
		/// <param name="idFieldName">自增列名称</param>
		/// <param name="seed">开始数据</param>
		/// <returns>开始数据</returns>
		public static int ChangeIdEntity(this SqlConnection connection, string tableName, string idFieldName, int seed)
		{
			if (tableName == string.Empty)
			{
				throw new Exception("要修改自增字段的数据库表名不能为空!");
			}

			using (SqlCommand command = connection.CreateCommand())
			{
				command.CommandText = string.Format("DBCC CHECKIDENT ('{0}', RESEED, {1});", tableName, seed - 1);
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

			return seed;
		}


		#endregion

		#region Command


		/// <summary>
		/// 运行命令-NonQuery方式
		/// <para>这个函数隐式带有了执行后关闭数据库链接的参数</para>
		/// </summary>
		/// <param name="command">数据库命令</param>
		/// <returns>返回的结果(影响行数)</returns>
		public static int RunCommandNonQuery(this SqlCommand command)
		{
			return command.RunCommandNonQuery(true);
		}

		/// <summary>
		/// 运行命令-NonQuery方式
		/// </summary>
		/// <param name="command">当前命令</param>
		/// <param name="closeConnection">如果为 true, 则在执行后会自动关闭数据库链接</param>
		/// <returns>返回的结果(影响行数)</returns>
		public static int RunCommandNonQuery(this SqlCommand command, bool closeConnection)
		{
			command.Connection.Open();
			int i = command.ExecuteNonQuery();
			if (closeConnection) command.Connection.Close();
			return i;
		}

		/// <summary>
		/// 运行命令-Scalar方式
		/// <para>这个函数隐式带有了执行后关闭数据库链接的参数</para>
		/// </summary>
		/// <param name="command">当前命令</param>
		/// <returns>返回的第一行第一列结果</returns>
		public static object RunCommandScalar(this SqlCommand command)
		{
			return command.RunCommandScalar(true);
		}

		/// <summary>
		/// 运行命令-Scalar方式
		/// </summary>
		/// <param name="command">当前命令</param>
		/// <param name="closeConnection">如果为 true, 则在执行后会自动关闭数据库链接</param>
		/// <returns>返回的第一行第一列结果</returns>
		public static object RunCommandScalar(this SqlCommand command, bool closeConnection)
		{
			command.Connection.Open();
			object r = command.ExecuteScalar();
			if (closeConnection) command.Connection.Close();
			return r;
		}


		/// <summary>
		/// 运行命令-Scalar方式并返回指定类型的结果
		/// <para>这个函数隐式带有了执行后关闭数据库链接的参数</para>
		/// </summary>
		/// <param name="command">当前命令</param>
		/// <returns>返回执行的结果</returns>
		public static T RunCommandScalar<T>(this SqlCommand command)
		{
			return command.RunCommandScalar<T>(true);
		}

		/// <summary>
		/// 运行命令-Scalar方式
		/// </summary>
		/// <param name="command">当前命令</param>
		/// <param name="closeConnection">如果为 true, 则在执行后会自动关闭数据库链接</param>
		/// <returns>返回执行的结果</returns>
		public static T RunCommandScalar<T>(this SqlCommand command, bool closeConnection)
		{
			command.Connection.Open();
			object r = command.ExecuteScalar();
			if (closeConnection) command.Connection.Close();
			return (T)r;
		}

		/// <summary>
		/// 运行命令-DataReader方式
		/// <para>这个函数隐式带有了执行后关闭数据库链接的参数</para>
		/// </summary>
		/// <param name="command">当前命令</param>
		/// <returns>返回包含执行结果的  <see cref="T:System.Data.SqlClient.SqlDataReader"/></returns>
		public static SqlDataReader RunCommandReader(this SqlCommand command)
		{
			return RunCommandReader(command, true);
		}

		/// <summary>
		/// 运行命令-DataReader方式
		/// </summary>
		/// <param name="command">当前命令</param>
		/// <param name="closeConnection">如果为 true, 则在执行后会自动关闭数据库链接</param>
		/// <returns>返回包含执行结果的  <see cref="T:System.Data.SqlClient.SqlDataReader"/></returns>
		public static SqlDataReader RunCommandReader(this SqlCommand command, bool closeConnection)
		{
			command.Connection.Open();
			return command.ExecuteReader(closeConnection ? CommandBehavior.CloseConnection : CommandBehavior.Default);
		}


		/// <summary>
		/// 为当前的命令添加参数
		/// </summary>
		/// <param name="command">当前命令</param>
		/// <param name="name">参数名，@符号是可选的</param>
		/// <param name="value">要添加的值</param>
		/// <returns>返回传入的  <see cref="T:System.Data.SqlClient.SqlCommand"/></returns>
		public static SqlCommand AddParameter(this SqlCommand command, string name, object value)
		{
			name = name.StartsWith("@") ? name : "@" + name;

			command.Parameters.Add(new SqlParameter(name, value));
			return command;
		}

		/// <summary>
		/// 为当前的命令添加参数
		/// </summary>
		/// <param name="command">当前命令</param>
		/// <param name="name">参数名，@符号是可选的</param>
		/// <param name="type">参数的类型</param>
		/// <param name="value">要添加的值</param>
		/// <returns>返回传入的  <see cref="T:System.Data.SqlClient.SqlCommand"/></returns>
		public static SqlCommand AddParameter(this SqlCommand command, string name, SqlDbType type, object value)
		{
			name = name.StartsWith("@") ? name : "@" + name;

			command.Parameters.Add(new SqlParameter(name, type) { Value = value });
			return command;
		}

		/// <summary>
		/// 为当前的命令添加参数
		/// </summary>
		/// <param name="command">当前命令</param>
		/// <param name="name">参数名，@符号是可选的</param>
		/// <param name="type">参数的类型</param>
		/// <param name="size">参数的长度</param>
		/// <param name="value">要添加的值</param>
		/// <returns>返回传入的  <see cref="T:System.Data.SqlClient.SqlCommand"/></returns>
		public static SqlCommand AddParameter(this SqlCommand command, string name, SqlDbType type, int size, object value)
		{
			name = name.StartsWith("@") ? name : "@" + name;

			command.Parameters.Add(new SqlParameter(name, type, size) { Value = value });
			return command;
		}

		/// <summary>
		/// 添加参数
		/// </summary>
		/// <param name="command">当前命令</param>
		/// <param name="name">参数名，@符号是可选的</param>
		/// <param name="value">参数值</param>
		/// <returns>返回参数命令</returns>
		public static SqlCommand AddParameter(this SqlCommand command, string name, int value)
		{
			return command.AddParameter(name, SqlDbType.Int, value);
		}

		/// <summary>
		/// 添加参数
		/// </summary>
		/// <param name="command">当前命令</param>
		/// <param name="name">参数名，@符号是可选的</param>
		/// <param name="value">参数值</param>
		/// <returns>返回参数命令</returns>
		public static SqlCommand AddParameter(this SqlCommand command, string name, bool value)
		{
			return command.AddParameter(name, SqlDbType.Bit, value);
		}


		/// <summary>
		/// 添加参数
		/// </summary>
		/// <param name="command">当前命令</param>
		/// <param name="name">参数名，@符号是可选的</param>
		/// <param name="value">参数值</param>
		/// <returns>返回参数命令</returns>
		public static SqlCommand AddParameter(this SqlCommand command, string name, short value)
		{
			return command.AddParameter(name, SqlDbType.SmallInt, value);
		}

		/// <summary>
		/// 添加参数
		/// </summary>
		/// <param name="command">当前命令</param>
		/// <param name="name">参数名，@符号是可选的</param>
		/// <param name="value">参数值</param>
		/// <returns>返回参数命令</returns>
		public static SqlCommand AddParameter(this SqlCommand command, string name, byte value)
		{
			return command.AddParameter(name, SqlDbType.TinyInt, value);
		}


		/// <summary>
		/// 添加参数
		/// </summary>
		/// <param name="command">当前命令</param>
		/// <param name="name">参数名，@符号是可选的</param>
		/// <param name="value">参数值</param>
		/// <returns>返回参数命令</returns>
		public static SqlCommand AddParameter(this SqlCommand command, string name, DateTime value)
		{
			return command.AddParameter(name, SqlDbType.DateTime, value);
		}

		/// <summary>
		/// 添加参数
		/// </summary>
		/// <param name="command">当前命令</param>
		/// <param name="name">参数名，@符号是可选的</param>
		/// <param name="value">参数值</param>
		/// <returns>返回参数命令</returns>
		public static SqlCommand AddParameter(this SqlCommand command, string name, string value, int fieldSize)
		{
			return command.AddParameter(name, SqlDbType.NVarChar, fieldSize, value);
		}

		#endregion

	}
}

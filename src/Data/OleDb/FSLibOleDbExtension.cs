using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
using System.ComponentModel;
using FSLib.Data;

namespace System.Data.OleDb
{
	/// <summary>
	/// OLEDB 的扩展类
	/// </summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static class FSLibOleDbExtension
	{
		#region 数据连接

		/// <summary>
		/// 创建数据库连接
		/// </summary>
		/// <returns></returns>
		public static OleDbConnection CreateConnection(this string conn)
		{
			return new OleDbConnection(conn);
		}


		/// <summary>
		/// 创建数据库命令
		/// </summary>
		/// <param name="connection"></param>
		/// <param name="sql"></param>
		/// <returns></returns>
		public static OleDbCommand CreateQueryCommand(this OleDbConnection connection, string sql)
		{
			return new OleDbCommand(sql, connection)
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
		public static OleDbCommand CreateProcedureCommand(this OleDbConnection connection, string sql)
		{
			return new OleDbCommand(sql, connection)
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
		public static int ChangeIdEntity(this OleDbConnection connection, string tableName, string idFieldName, int seed)
		{
			if (tableName == string.Empty)
			{
				throw new Exception("要修改自增字段的数据库表名不能为空!");
			}

			using (OleDbCommand command = connection.CreateCommand())
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
		public static int RunCommandNonQuery(this OleDbCommand command)
		{
			return command.RunCommandNonQuery(true);
		}

		/// <summary>
		/// 运行命令-NonQuery方式
		/// </summary>
		/// <param name="command">当前命令</param>
		/// <param name="closeConnection">如果为 true, 则在执行后会自动关闭数据库链接</param>
		/// <returns>返回的结果(影响行数)</returns>
		public static int RunCommandNonQuery(this OleDbCommand command, bool closeConnection)
		{
			if (command.Connection.State == ConnectionState.Closed) command.Connection.Open();
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
		public static object RunCommandScalar(this OleDbCommand command)
		{
			return command.RunCommandScalar(true);
		}

		/// <summary>
		/// 运行命令-Scalar方式
		/// </summary>
		/// <param name="command">当前命令</param>
		/// <param name="closeConnection">如果为 true, 则在执行后会自动关闭数据库链接</param>
		/// <returns>返回的第一行第一列结果</returns>
		public static object RunCommandScalar(this OleDbCommand command, bool closeConnection)
		{
			if (command.Connection.State == ConnectionState.Closed) command.Connection.Open();
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
		public static T RunCommandScalar<T>(this OleDbCommand command)
		{
			return command.RunCommandScalar<T>(true);
		}

		/// <summary>
		/// 运行命令-Scalar方式
		/// </summary>
		/// <param name="command">当前命令</param>
		/// <param name="closeConnection">如果为 true, 则在执行后会自动关闭数据库链接</param>
		/// <returns>返回执行的结果</returns>
		public static T RunCommandScalar<T>(this OleDbCommand command, bool closeConnection)
		{
			if (command.Connection.State == ConnectionState.Closed) command.Connection.Open();
			object r = command.ExecuteScalar();
			if (closeConnection) command.Connection.Close();
			return (T)r;
		}

		/// <summary>
		/// 运行命令-DataReader方式
		/// <para>这个函数隐式带有了执行后关闭数据库链接的参数</para>
		/// </summary>
		/// <param name="command">当前命令</param>
		/// <returns>返回包含执行结果的  <see cref="T:System.Data.SqlClient.OleDbDataReader"/></returns>
		public static OleDbDataReader RunCommandReader(this OleDbCommand command)
		{
			return RunCommandReader(command, true);
		}

		/// <summary>
		/// 运行命令-DataReader方式
		/// </summary>
		/// <param name="command">当前命令</param>
		/// <param name="closeConnection">如果为 true, 则在执行后会自动关闭数据库链接</param>
		/// <returns>返回包含执行结果的  <see cref="T:System.Data.SqlClient.OleDbDataReader"/></returns>
		public static OleDbDataReader RunCommandReader(this OleDbCommand command, bool closeConnection)
		{
			if (command.Connection.State == ConnectionState.Closed) command.Connection.Open();
			return command.ExecuteReader(closeConnection ? CommandBehavior.CloseConnection : CommandBehavior.Default);
		}


		/// <summary>
		/// 为当前的命令添加参数
		/// </summary>
		/// <param name="command">当前命令</param>
		/// <param name="name">参数名，@符号是可选的</param>
		/// <param name="value">要添加的值</param>
		/// <returns>返回传入的  <see cref="T:System.Data.SqlClient.OleDbCommand"/></returns>
		public static OleDbCommand AddParameter(this OleDbCommand command, string name, object value)
		{
			name = name.StartsWith("@") ? name : "@" + name;

			command.Parameters.Add(new OleDbParameter(name, value));
			return command;
		}

		/// <summary>
		/// 为当前的命令添加参数
		/// </summary>
		/// <param name="command">当前命令</param>
		/// <param name="name">参数名，@符号是可选的</param>
		/// <param name="type">参数的类型</param>
		/// <param name="value">要添加的值</param>
		/// <returns>返回传入的  <see cref="T:System.Data.SqlClient.OleDbCommand"/></returns>
		public static OleDbCommand AddParameter(this OleDbCommand command, string name, OleDbType type, object value)
		{
			name = name.StartsWith("@") ? name : "@" + name;

			command.Parameters.Add(new OleDbParameter(name, type) { Value = value });
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
		/// <returns>返回传入的  <see cref="T:System.Data.SqlClient.OleDbCommand"/></returns>
		public static OleDbCommand AddParameter(this OleDbCommand command, string name, OleDbType type, int size, object value)
		{
			name = name.StartsWith("@") ? name : "@" + name;

			command.Parameters.Add(new OleDbParameter(name, type, size) { Value = value });
			return command;
		}

		/// <summary>
		/// 添加参数
		/// </summary>
		/// <param name="command">当前命令</param>
		/// <param name="name">参数名，@符号是可选的</param>
		/// <param name="value">参数值</param>
		/// <returns>返回参数命令</returns>
		public static OleDbCommand AddParameter(this OleDbCommand command, string name, int value)
		{
			return command.AddParameter(name, OleDbType.Integer, value);
		}

		/// <summary>
		/// 添加参数
		/// </summary>
		/// <param name="command">当前命令</param>
		/// <param name="name">参数名，@符号是可选的</param>
		/// <param name="value">参数值</param>
		/// <returns>返回参数命令</returns>
		public static OleDbCommand AddParameter(this OleDbCommand command, string name, bool value)
		{
			return command.AddParameter(name, OleDbType.Boolean, value);
		}


		/// <summary>
		/// 添加参数
		/// </summary>
		/// <param name="command">当前命令</param>
		/// <param name="name">参数名，@符号是可选的</param>
		/// <param name="value">参数值</param>
		/// <returns>返回参数命令</returns>
		public static OleDbCommand AddParameter(this OleDbCommand command, string name, short value)
		{
			return command.AddParameter(name, OleDbType.SmallInt, value);
		}

		/// <summary>
		/// 添加参数
		/// </summary>
		/// <param name="command">当前命令</param>
		/// <param name="name">参数名，@符号是可选的</param>
		/// <param name="value">参数值</param>
		/// <returns>返回参数命令</returns>
		public static OleDbCommand AddParameter(this OleDbCommand command, string name, byte value)
		{
			return command.AddParameter(name, OleDbType.TinyInt, value);
		}


		/// <summary>
		/// 添加参数
		/// </summary>
		/// <param name="command">当前命令</param>
		/// <param name="name">参数名，@符号是可选的</param>
		/// <param name="value">参数值</param>
		/// <returns>返回参数命令</returns>
		public static OleDbCommand AddParameter(this OleDbCommand command, string name, DateTime value)
		{
			return command.AddParameter(name, OleDbType.Date, value);
		}

		/// <summary>
		/// 添加参数
		/// </summary>
		/// <param name="command">当前命令</param>
		/// <param name="name">参数名，@符号是可选的</param>
		/// <param name="value">参数值</param>
		/// <returns>返回参数命令</returns>
		public static OleDbCommand AddParameter(this OleDbCommand command, string name, string value, int fieldSize)
		{
			return command.AddParameter(name, OleDbType.VarWChar, fieldSize, value);
		}

		#endregion

		#region 辅助操作

		/// <summary>
		/// 对字符串进行转义
		/// </summary>
		/// <param name="str">要转义的字符串</param>
		/// <returns>转义后的字符串</returns>
		public static string Quote(this string str)
		{
			if (string.IsNullOrEmpty(str)) return str;

			return str.Replace("'", "''");
		}

		/// <summary>
		/// 将指定 <see cref="OleDbDataReader"/> 的首行记录转换为类型为 <typeparamref name="T"/> 的实体。如果没有数据，则返回默认值。
		/// </summary>
		/// <typeparam name="T">实体类型</typeparam>
		/// <param name="dr">类型为 <see cref="OleDbDataReader"/> 的数据集阅读器</param>
		/// <param name="adapter">转换实体数据的方法</param>
		/// <returns>类型为 <typeparamref name="T"/> 的实体</returns>
		public static T ToModel<T>(this OleDbDataReader dr, Func<OleDbDataReader, T> adapter)
		{
			if (!dr.Read()) return default(T);
			else return adapter(dr);
		}

		/// <summary>
		/// 将指定 <see cref="OleDbDataReader"/> 的记录转换为类型为 <typeparamref name="T"/> 的实体泛型列表。
		/// </summary>
		/// <typeparam name="T">实体类型</typeparam>
		/// <param name="dr">类型为 <see cref="OleDbDataReader"/> 的数据集阅读器</param>
		/// <param name="adapter">转换实体数据的方法</param>
		/// <returns>类型为 <typeparamref name="T"/> 的实体</returns>
		public static List<T> ToModelList<T>(this OleDbDataReader dr, Func<OleDbDataReader, T> adapter)
		{
			var list = new List<T>(20);
			while (dr.Read())
			{
				list.Add(adapter(dr));
			}

			return list;
		}

		#endregion

	}
}

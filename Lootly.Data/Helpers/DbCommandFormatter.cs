using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Lootly.Data.Helpers
{
	 public class DbCommandFormatter
	 {
		  public static string ToSqlString(IDbCommand cmd)
		  {
				string sql;
				if (cmd.Parameters.Count > 0)
				{
					 var parameters = cmd.Parameters.Cast<IDataParameter>().Select(FormatParameter);
					 sql = string.Format("{0}\n\n{1}", string.Join("\n", parameters), cmd.CommandText);
				}
				else
				{
					 sql = cmd.CommandText;
				}
				return string.Format("\n{0};", sql);
		  }

		  private static string FormatParameter(IDataParameter p)
		  {
				return string.Format("DECLARE {0} {1} = {2};", p.ParameterName, p.DbType, FormatValue(p.Value));
		  }

		  private static string FormatValue(object value)
		  {
				return IsNumeric(value) ? value.ToString() : string.Format("'{0}'", value);
		  }

		  private static bool IsNumeric(object value)
		  {
				decimal parseResult;
				return decimal.TryParse(value.ToString(), out parseResult);
		  }
	 }
}

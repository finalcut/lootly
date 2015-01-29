using System;
using System.Data;
using System.Diagnostics;
using NPoco;
using Lootly.Data.Helpers;


namespace Lootly.Data
{
	 public class CustomDatabase : Database
	 {
		  public CustomDatabase(string connectionStringName) : base(connectionStringName) { }

		  protected override void OnException(Exception e)
		  {
				base.OnException(e);
				e.Data["LastSQL"] = this.LastSQL;
				e.Data["LastArgs"] = this.LastArgs;
				e.Data["LastCommand"] = this.LastCommand;

				AbortTransaction();  // Rollback transaction on error
		  }

#if DEBUG
		  protected override void OnExecutingCommand(IDbCommand cmd)
		  {
				base.OnExecutingCommand(cmd);
				var sql = DbCommandFormatter.ToSqlString(cmd);
				Trace.WriteLine(string.Format("\n{0}\n{1}", sql, new string('-', 80)), "SQL");
		  }
#endif
	 }
}

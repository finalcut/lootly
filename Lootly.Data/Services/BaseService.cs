using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Lootly.Data.Services
{
	 public abstract class BaseService
	 {
		  public CustomDatabase Database { get; set; }
	 }

	 public abstract class BaseService<TGetModel, TCreateModel, TUpdateModel>
		  : BaseService, IService<TGetModel, TCreateModel, TUpdateModel>
	 {
		  public TGetModel Get(object id)
		  {
				return Database.SingleOrDefaultById<TGetModel>(id);
		  }

		  public TGetModel GetWhere( Expression<Func<TGetModel, bool>> expression)
		  {
				return Database.Query<TGetModel>().SingleOrDefault(expression);
		  }

		  public IEnumerable<TGetModel> GetAll()
		  {
				return Database.Fetch<TGetModel>();
		  }

		  public IEnumerable<TGetModel> GetAllWhere( Expression<Func<TGetModel, bool>> expression)
		  {
				return Database.FetchWhere(expression);
		  }

		  public int Create( TCreateModel poco)
		  {
				var obj = Database.Insert(poco);
				return Convert.ToInt32(obj);
		  }

		  public int Update( object id, TUpdateModel poco, IEnumerable<string> propertyNames)
		  {
				var pocoData = Database.PocoDataFactory.ForType(typeof(TUpdateModel));

				var columnNames = pocoData.Columns
						  .Select(kvp => kvp.Value)
						  .Where(pc => propertyNames.Contains(pc.MemberInfo.Name, StringComparer.OrdinalIgnoreCase))
						  .Where(pc => !pc.MemberInfo.Name.Equals(pocoData.TableInfo.PrimaryKey, StringComparison.OrdinalIgnoreCase)) // Don't attempt to update the table's primary key column
						  .Select(pc => pc.ColumnName);

				return Database.Update(poco, id, columnNames);
		  }

		  public int Delete( object id)
		  {
				return Database.Delete<TCreateModel>(id);
		  }
	 }

	 public abstract class BaseRepository<TGetModel, TCreateModel, TUpdateModel, TFilterModel>
		  : BaseService<TGetModel, TCreateModel, TUpdateModel>, IService<TGetModel, TCreateModel, TUpdateModel, TFilterModel>
		  where TFilterModel : class, new()
	 {
		  public abstract IEnumerable<TGetModel> GetAll(TFilterModel filter);
	 }
}
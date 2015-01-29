using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Lootly.Data.Services
{
	 public abstract class BaseService
	 {
		  public CustomDatabase Database { get; set; }
	 }

	 public abstract class BaseService<TModel>
		  : BaseService, IService<TModel>
	 {
		  public TModel Get(object id)
		  {
				return Database.SingleOrDefaultById<TModel>(id);
		  }

		  public TModel GetWhere(Expression<Func<TModel, bool>> expression)
		  {
				return Database.Query<TModel>().SingleOrDefault(expression);
		  }

		  public IEnumerable<TModel> GetAll()
		  {
				return Database.Fetch<TModel>();
		  }

		  public IEnumerable<TModel> GetAllWhere(Expression<Func<TModel, bool>> expression)
		  {
				return Database.FetchWhere(expression);
		  }

		  public int Create(TModel poco)
		  {
				var obj = Database.Insert(poco);
				return Convert.ToInt32(obj);
		  }

		  public int Update(object id, TModel poco, IEnumerable<string> propertyNames)
		  {
				var pocoData = Database.PocoDataFactory.ForType(typeof(TModel));

				var columnNames = pocoData.Columns
						  .Select(kvp => kvp.Value)
						  .Where(pc => propertyNames.Contains(pc.MemberInfo.Name, StringComparer.OrdinalIgnoreCase))
						  .Where(pc => !pc.MemberInfo.Name.Equals(pocoData.TableInfo.PrimaryKey, StringComparison.OrdinalIgnoreCase)) // Don't attempt to update the table's primary key column
						  .Select(pc => pc.ColumnName);

				return Database.Update(poco, id, columnNames);
		  }

		  public int Delete( object id)
		  {
				return Database.Delete<TModel>(id);
		  }
	 }

	 public abstract class BaseRepository<TModel, TFilterModel>
		  : BaseService<TModel>, IService<TModel, TFilterModel>
		  where TFilterModel : class, new()
	 {
		  public abstract IEnumerable<TModel> GetAll(TFilterModel filter);
	 }
}
using System.Collections.Generic;

namespace Lootly.Data.Services
{
	 public interface IService<TModel>
	 {
		  TModel Get(object id);
		  IEnumerable<TModel> GetAll();
		  int Create(TModel poco);
		  int Update(object id, TModel poco, IEnumerable<string> propertyNames);
		  int Delete(object id);
		  CustomDatabase Database { get; set; }
	 }

	 public interface IService<TModel, TFilterModel>
		  : IService<TModel>
		  where TFilterModel : class, new()
	 {
		  IEnumerable<TModel> GetAll(TFilterModel filter);
	 }
}

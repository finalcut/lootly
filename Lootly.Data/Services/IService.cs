using System.Collections.Generic;

namespace Lootly.Data.Services
{
	 public interface IService<TGetModel, TCreateModel, TUpdateModel>
	 {
		  TGetModel Get(object id);
		  IEnumerable<TGetModel> GetAll();
		  int Create(TCreateModel poco);
		  int Update(object id, TUpdateModel poco, IEnumerable<string> propertyNames);
		  int Delete(object id);
		  CustomDatabase Database { get; set; }
	 }

	 public interface IService<TGetModel, TCreateModel, TUpdateModel, TFilterModel>
		  : IService<TGetModel, TCreateModel, TUpdateModel>
		  where TFilterModel : class, new()
	 {
		  IEnumerable<TGetModel> GetAll(TFilterModel filter);
	 }
}

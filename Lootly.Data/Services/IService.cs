using System.Collections.Generic;

namespace Lootly.Data.Services
{
	 public interface IService<TGetModel, TCreateModel, TUpdateModel>
	 {
		  TGetModel Get(int version, object id);
		  IEnumerable<TGetModel> GetAll(int version);
		  int Create(int version, TCreateModel poco);
		  int Update(int version, object id, TUpdateModel poco, IEnumerable<string> propertyNames);
		  int Delete(int version, object id);
		  CustomDatabase Database { get; set; }
	 }

	 public interface IService<TGetModel, TCreateModel, TUpdateModel, TFilterModel>
		  : IService<TGetModel, TCreateModel, TUpdateModel>
		  where TFilterModel : class, new()
	 {
		  IEnumerable<TGetModel> GetAll(int version, TFilterModel filter);
	 }
}

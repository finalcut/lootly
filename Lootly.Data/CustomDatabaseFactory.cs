using System;
using System.Linq;
using System.Reflection;
using NPoco;
using NPoco.FluentMappings;

namespace Lootly.Data
{
	 public static class CustomDatabaseFactory
	 {
		  public static DatabaseFactory Setup(string connectionStringName)
		  {
				var fluentConfig = FluentMappingConfiguration.Scan(scanner =>
				{
					 scanner.Assembly(Assembly.GetExecutingAssembly());
					 scanner.WithSmartConventions();
					 scanner.TablesNamed(GetTableName);
					 scanner.PrimaryKeysNamed(GetPrimaryKeyName);

					 scanner.Columns
						  .IgnoreWhere(mi => ColumnInfo.FromMemberInfo(mi).IgnoreColumn)
						  .ResultWhere(mi => ColumnInfo.FromMemberInfo(mi).ResultColumn)
						  .Named(mi =>
						  {
								if (mi.GetCustomAttributes<ColumnAttribute>().Any())
								{
									 return mi.GetCustomAttribute<ColumnAttribute>().Name;
								}
								/*
								else if (mi.GetMemberInfoType() == typeof(bool) && mi.Name.ToLower().StartsWith("is"))
								{
									 return mi.Name.Remove(0, 2).ToLower() + "_ind";
								}
								else if (mi.GetMemberInfoType() == typeof(bool) && mi.Name.ToLower().StartsWith("can"))
								{
									 return Inflector.AddUnderscores(mi.Name).ToLower() + "_ind";
								}
								*/
								else
								{
									 return mi.Name;
								}

						  })
						  .Aliased(mi =>
						  {
								if (mi.GetCustomAttributes<AliasAttribute>().Any())
								{
									 return mi.GetCustomAttribute<AliasAttribute>().Alias;
								}

								return null;
						  })
						  .DbColumnTypeAs(mi =>
						  {
								if (mi.GetCustomAttributes<ColumnTypeAttribute>().Any())
								{
									 return mi.GetCustomAttribute<ColumnTypeAttribute>().Type;
								}
								return null;
						  });
				});

				var dbFactory = DatabaseFactory.Config(x =>
					 x.UsingDatabase(() => new CustomDatabase(connectionStringName))
					 .WithMapper(new Mapper())
					 .WithFluentConfig(fluentConfig));
				return dbFactory;
		  }

		  private static string GetTableName(Type t)
		  {
				if (t.GetCustomAttributes<TableNameAttribute>().Any())
				{
					 return t.GetCustomAttribute<TableNameAttribute>().Value;
				}
				else
				{
					 string tableName = Inflector.MakePlural(t.Name);

					 return tableName;

				}
		  }

		  private static string GetPrimaryKeyName(Type t)
		  {
				if (t.GetCustomAttributes<PrimaryKeyAttribute>().Any())
				{
					 return t.GetCustomAttribute<PrimaryKeyAttribute>().Value;
				}
				else
				{
					 return "Id";
				}
		  }

		  public class Mapper : DefaultMapper
		  {
				public override Func<object, object> GetToDbConverter(Type destType, Type sourceType)
				{
					 if (sourceType.IsEnum && destType == typeof(string))
					 {
						  return x => x.ToString();
					 }

					 return base.GetToDbConverter(destType, sourceType);
				}
		  }
	 }
}

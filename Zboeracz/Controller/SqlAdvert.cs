using Dapper;
using System;
using System.Configuration;
using System.Data.SqlClient;


namespace Zbieracz
{
    public static class SqlAdvert
    {
        private static SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString);
        public static string InsertAdvert(Advert advert, int searchid)
        {
            string query = @"insert into dbo.Advert(Adw_ForeignId, Adw_Name, Adw_Category, Adw_Location, Adw_Date, Adw_Price, Adw_Url, Adw_IsPromoted, Adw_SeLId)
values (@Id, @Name, @Category, @Location, @Date, @Price, @Url, @IsPromoted, @SearchId)
select top 1 'Poprawnie dodano ogłoszenie ' + Adw_ForeignId + ': ' + Adw_Name as Info from dbo.Advert where Adw_Id = SCOPE_IDENTITY()";

            DynamicParameters param = new DynamicParameters();
            param.Add("Id", advert.Id);
            param.Add("Name", advert.Name);
            param.Add("Category", advert.Category);
            param.Add("Location", advert.Location);
            param.Add("Date", advert.Date);
            param.Add("Price", advert.Price);
            param.Add("Url", advert.Url);
            param.Add("IsPromoted", advert.IsPromoted);
            param.Add("SearchId", searchid);

            return sqlConnection.ExecuteScalar<string>(query, param);
        }

        internal static string GetAdvertUrlById(int latestAdvId)
        {
            string query = @"select top 1 Adw_Url from dbo.Advert where adw_id = @Id";

            DynamicParameters param = new DynamicParameters();
            param.Add("Id", latestAdvId);

            return sqlConnection.ExecuteScalar<string>(query, param);
        }

        internal static int GetLatestAdvertId(int searchingId)
        {
            int i = 0;
            string query = @"select top 1 adw_id from dbo.Advert
where not exists(select 1 from dbo.Describe where Des_AdwId = Adw_Id)
order by Adw_Id";

            i = sqlConnection.ExecuteScalar<int>(query);

            return i;
        }

        public static bool IsAdvertExists(Advert advert)
        {
            string query = @"select iif(exists(select 1 from dbo.Advert where Adw_ForeignId = @Id),1,0) IsAdwExists";

            DynamicParameters param = new DynamicParameters();
            param.Add("Id", advert.Id);

            return sqlConnection.ExecuteScalar<bool>(query, param);
        }

        internal static string InsertAdvertDescribe(AdvertDescribe advert)
        {
            string query = @"insert into dbo.Describe(Des_AdwId, Des_Describe, Des_Exposer, Des_Tel)
values (@Id, @Describe, @Exposer,@Tel)
select top 1 'Poprawnie dodano informacje o ogloszeniu ' + Adw_ForeignId + ': ' + Adw_Name as Info 
from dbo.Describe inner join dbo.Advert on Des_AdwId = Adw_Id where Des_Id = SCOPE_IDENTITY()";

            DynamicParameters param = new DynamicParameters();
            param.Add("Id", advert.AdvertId);
            param.Add("Describe", advert.Describe);
            param.Add("Exposer", advert.AdvertExposer);
            param.Add("Tel", null);

            return sqlConnection.ExecuteScalar<string>(query, param);
        }

        internal static string InsertAdvertDetail(Details detail, int id)
        {
            string query = @"insert into dbo.Attributes(Atr_AdwId, Atr_Name, Atr_Value)
values (@Id, @Name, @Value)
select top 1 'Poprawnie dodano atrybuty ogłoszenia ' + Adw_ForeignId + ': ' + Atr_Name + ': ' + Atr_Value as Info 
from dbo.Attributes inner join dbo.Advert on Atr_AdwId = Adw_Id where Atr_Id = SCOPE_IDENTITY()";

            DynamicParameters param = new DynamicParameters();
            param.Add("Id", id);
            param.Add("Name", detail.DetailName);
            param.Add("Value", detail.DetailValue);

            return sqlConnection.ExecuteScalar<string>(query, param);
        }

        public static string GetSearchingUrl(int id)
        {
            string query = @"select Sel_Url from dbo.SearchLink  where Sel_Id = @Id";

            DynamicParameters param = new DynamicParameters();
            param.Add("Id", id);

            return sqlConnection.ExecuteScalar<string>(query, param);
        }
    }
}

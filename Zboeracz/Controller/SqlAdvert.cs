using Dapper;
using System.Configuration;
using System.Data.SqlClient;


namespace Zbieracz
{
    public static class SqlAdvert
    {
        private static SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString);
        public static string InsertAdvert(Advert advert)
        {
            string query = @"insert into dbo.Advert(Adw_ForeignId, Adw_Name, Adw_Category, Adw_Location, Adw_Date, Adw_Price, Adw_Url, Adw_IsPromoted)
values (@Id, @Name, @Category, @Location, @Date, @Price, @Url, @IsPromoted)
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

            return sqlConnection.ExecuteScalar<string>(query, param);
        }

        public static bool IsAdvertExists(Advert advert)
        {
            string query = @"select iif(exists(select 1 from dbo.Advert where Adw_ForeignId = @Id),1,0) IsAdwExists";

            DynamicParameters param = new DynamicParameters();
            param.Add("Id", advert.Id);

            return sqlConnection.ExecuteScalar<bool>(query, param);
        }

    }
}

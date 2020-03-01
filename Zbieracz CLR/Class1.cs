using System.Data.SqlTypes;
using System.IO;
using Microsoft.SqlServer.Server;


namespace Zbieracz_CLR
{
    public class Main
    {
        [SqlProcedure]
        public static SqlInt32 GetAdvertsFromService(int searchingId = 0)
        {
            string filename = Path.Combine(@"C:\Zbieracz", "Zbieracz.exe");
            var proc = System.Diagnostics.Process.Start(filename, $"lista {searchingId}");
            
            proc.Close();
            return 1;
        }

        [SqlProcedure]
        public static SqlInt32 GetAdvertDetails(int searchingId = 0)
        {
            string filename = Path.Combine(@"C:\Zbieracz", "Zbieracz.exe");
            var proc = System.Diagnostics.Process.Start(filename, $"detal {searchingId}");

            proc.Close();
            return 1;
        }
    }
}

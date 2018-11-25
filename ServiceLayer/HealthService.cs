using System.Threading.Tasks;
using API.Models;
using API.DataAccess;

namespace API.ServiceLayer
{
    public class HealthService
    {
        public async Task<SystemStatus> GetWaitTime()
        {
            SystemStatus result = new SystemStatus
            {
                SystemIsUp = true,
                DbIsUp = false
            };
            
            DBConnector test = new DBConnector();
            if (test.OpenConnection())
            {
                result.DbIsUp = true;
                test.CloseConnection();
            }

            return await Task.FromResult(result);
        }
    }
}
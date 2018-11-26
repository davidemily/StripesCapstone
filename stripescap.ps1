param (
    [string]$name = "stripesBackend"
 )
mkdir $name
Set-Location $name
Write-Output "Creating api.csproj.."
$API = '
<Project Sdk="Microsoft.NET.Sdk.Web">

  <PropertyGroup>
    <TargetFramework>netcoreapp2.1</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <Folder Include="wwwroot\" />
  </ItemGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.AspNetCore.App" />
    <PackageReference Include="MySql.Data" Version="8.0.12" />
    <PackageReference Include="SwashBuckle.AspNetCore.MicrosoftExtensions" Version="0.4.0" />
    <PackageReference Include="Swashbuckle.AspNetCore.SwaggerGen" Version="3.0.0" />
    <PackageReference Include="Swashbuckle.AspNetCore.SwaggerUI" Version="3.0.0" />
  </ItemGroup>

</Project>'
$API >> 'API.csproj'
Write-Output "API.csproj created!"
Write-Output "Creating Program.cs.."
$Program = 'using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace API
{
    public class Program
    {
/*        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
*/	
		static void Main(string[] args)
		{
   			 var config = new ConfigurationBuilder()
        			.SetBasePath(Directory.GetCurrentDirectory())
			        .AddJsonFile("hosting.json", optional: true)
			        .Build();
 
			    var host = new WebHostBuilder()
			        .UseKestrel()
			        .UseConfiguration(config)
			        .UseContentRoot(Directory.GetCurrentDirectory())
			        .UseStartup<Startup>()
			        .Build();
			    host.Run();
		}
	}
}'
$Program >> Program.cs
Write-Output "Program.cs Created!"


Write-Output "Creating Startup.cs.."
$Startup = 'using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;
using Swashbuckle.AspNetCore.SwaggerGen;


namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            
            //trying to get swagger to work
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            
            app.UseMvc();
        }
    }
}
'
$Startup >> "Startup.cs"
Write-Output "Startup Created!"

Write-Output "Appsettings.Development.json.."
$AppSettings = '{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "System": "Information",
      "Microsoft": "Information"
    }
  }
}'
$AppSettings >> "appsettings.Development.json"
Write-Output "Appsettings created!"


Write-Output "Creating appsettings.json.."
$AppSettings = '{
  "Logging": {
    "LogLevel": {
      "Default": "Warning"
    }
  },
  "ConnectionStrings": "SERVER=localhost;uid=stripessql;password=stripessqldatabase;database=stripes;sslmode=none;",
  "AllowedHosts": "*"
}
'
$AppSettings >> "appsettings.json"
Write-Output "Appsettings created!"

Write-output "Creating hosting.json..."
$Hosting = '
{
	"server.urls": "http://localhost:5123"
}'
$Hosting >> "hosting.json"
Write-Output "hosting created!"

mkdir Controllers
Set-Location Controllers
Write-Output "Creating cars controller.."
$Cars = 'using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Models;
using API.DataAccess;
using API.ServiceLayer;

using System.Collections.Generic;
using System;

namespace API.Controllers
{
    [Route("api/[controller]")] 
    [ApiController] 
    public class CarsController : ControllerBase
    {
        [HttpPost]
        public ActionResult Post([FromBody] CarRequest request)
        {
            CarsService carsService = new CarsService();
            return carsService.Post(request);
            // if (request.CarId == null || request.CarNumber == null || request.PhoneNumber == null ||
            //     request.IsActive == null)
            // {
            //     return BadRequest();
            // }

            // var convertedCarRequest = new CarRequest()
            // {
            //     CarId = request.CarId,
            //     CarNumber = request.CarNumber,
            //     PhoneNumber = request.PhoneNumber,
            //     IsActive = request.IsActive
            // };
            
            // try
            // {
            //     DBConnector db = new DBConnector();
            //     db.InsertNewCar(convertedCarRequest);
            // }
            // catch (Exception ex)
            // {
            //     Console.WriteLine(ex);
            //     return StatusCode(500);
            // }
            // return Ok();

        }
    }
}'
$Cars >> "CarsController.cs"
Write-Output "Cars Controller created!"



Write-Output "Creating Health Controller.."
$Health='using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Models;
using API.DataAccess;
using API.ServiceLayer;

using System.Collections.Generic;
using System;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public async Task<SystemStatus> Get()
        {
            HealthService healthService = new HealthService();
            return await healthService.GetWaitTime();
        //     SystemStatus result = new SystemStatus
        //     {
        //         SystemIsUp = true,
        //         DbIsUp = false
        //     };
            
        //     DBConnector test = new DBConnector();
        //     if (test.OpenConnection())
        //     {
        //         result.DbIsUp = true;
        //         test.CloseConnection();
        //     }

        //     return await Task.FromResult(result);
        }
    }
}'
$Health >> 'HealthController.cs'
Write-Output "healthController created!"


Write-Output "creating comments controller"
$Comment = 'using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Models;
using API.DataAccess;

using System.Collections.Generic;
using System;
using API.ServiceLayer;

using System.Text;
using System.Net.Mail;
using System.Net.Mime;

namespace API.Controllers
{
    [Route("api/[controller]")] 
    [ApiController] 
    public class LeaveCommentController : ControllerBase
    {
        [HttpPost]
        public void Post([FromBody] CommentRequest commentToEmail)
        {
            LeaveCommentService leaveCommentService = new LeaveCommentService();
            leaveCommentService.Post(commentToEmail);
        //     try
        //     {
        //         MailMessage mailMsg = new MailMessage();

        //         // To
        //         mailMsg.To.Add(new MailAddress("STRIPESComments@gmail.com"));

        //         // From
        //         mailMsg.From = new MailAddress("STRIPESComments@gmail.com");

        //         // Subject and multipart/alternative Body
        //         mailMsg.Subject = "Patron Comment";
        //         mailMsg.Body = commentToEmail.comment;

        //         // Init SmtpClient and send
        //         SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", Convert.ToInt32(587))
        //         {
        //             Port = 587, EnableSsl = true
        //         };
        //         var credentials = new System.Net.NetworkCredential("STRIPESComments@gmail.com", "1qaz@wsx#edc4");
        //         smtpClient.Credentials = credentials;
        //         smtpClient.Send(mailMsg);
        //     }
            
        //     catch (Exception ex)
        //     {
        //         Console.WriteLine(ex.Message + " " + ex.InnerException);
        //     }
        }
    }
}'
$Comment >> 'LeaveCommentController.cs'
Write-Output "Leave Comment Controller Created!"


Write-Output "Creating Night Controller.."
$Night = 'using System;
using API.DataAccess;
using Microsoft.AspNetCore.Mvc;
using API.ServiceLayer;

namespace API.Controllers
{
    [Route("api/[controller]")] 
    [ApiController] 
    public class NightController : ControllerBase
    {
        [HttpPost]
        public ActionResult Post()
        {
            NightService nightService = new NightService();
            return nightService.Post();
            // var dbConnector = new DBConnector();
            // //get hours 9 hours from utc or 3 hours before central time to cover open till 3
            // var todaysDate = DateTime.Today.ToString("yyyy-MM-dd");

            // try
            // {
            //     if (dbConnector.IsNightActive(todaysDate) == false)
            //     {
            //         Console.WriteLine("night does not exist");
            //         dbConnector.CreateNewNight(todaysDate);
            //     }
            // }
            // catch (Exception ex)
            // {
            //     Console.WriteLine(ex.Message + ex.InnerException);
            //     return BadRequest();
            // }

            // return Ok();
        }
    }
}'
$Night >> 'NightController.cs'
Write-Output "Night Controller created!"


Write-Output "Creating Rides Controller.."
$Rides = 'using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Models;
using API.DataAccess;
using API.ServiceLayer;

using System.Collections.Generic;
using System;

namespace API.Controllers
{
    [Route("api/[controller]")] 
    [ApiController] 
    public class RidesController : ControllerBase
    {
        [HttpPost]
        public ActionResult Post([FromBody] PhoneRideRequest request)
        {
            RidesService ridesService = new RidesService();
            return ridesService.Post(request);
            // if (request.dropoffs == null || request.passengers == null || request.firstName == null ||
            //     request.homeAddress == null || request.pickupLocation == null || request.cellPhoneNumber == null)
            // {
            //     return BadRequest();
            // }

            // var convertedRideRequest = new RideRequest()
            // {
            //     Pickup = request.pickupLocation,
            //     PhoneNumber = request.cellPhoneNumber,
            //     NumberOfPeople = request.passengers,
            //     Destination = request.homeAddress,
            //     PatronName = request.firstName
            // };   
            
            // try
            // {
            //     DBConnector db = new DBConnector();
            //     db.InsertRide(convertedRideRequest);
            // }
            // catch (Exception ex)
            // {
            //     Console.WriteLine(ex);
            //     return StatusCode(500);
            // }
            // return Ok();

        }
    }
}'

$Rides >> 'RidesController.cs'
Write-Output "Rides controller created!"

$WaitTimeC='using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Models;
using API.DataAccess;
using API.ServiceLayer;

using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Razor.Language.Extensions;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WaitTimeController : ControllerBase
    {
//         private int _estWaitTimeOfUnassigned = 20;
//         private int _estWaitTimeOfAssigned = 10;
//         private const int __estWaitTimeOfRiding = 5;
//         private readonly DBConnector _dbConnector = new DBConnector();

        [HttpGet]
        public async Task<WaitTime> Get()
        {
            WaitTimeService waitTimeService = new WaitTimeService();
            return await waitTimeService.GetWaitTimeAsync();
            // WaitTime result = new WaitTime
            // {
            //     status = getStatus(),
            //     waitTime = getWaitTime()
            // };

            // return await Task.FromResult(result);
        }

//         private string getStatus()
//         {   
//             var todaysDate = DateTime.Today.ToString("yyyy-MM-dd");
//             Console.WriteLine(todaysDate);
//             try
//             {
//                 if( _dbConnector.IsNightActive(todaysDate))
//                 {
//                     return "running";
//                 }
//             }
//             catch (Exception ex)
//             {
//                 return ex.ToString();
//             }
//             return "notRunning";
            
//         }
//   //Approx. wait time =
//   //[(Number of unassigned requested rides * Estimated wait time for unassigned ride in minutes)
//   //+
//   //(Number assigned rides * estimated wait time of assigned ride in minutes)] / Number of cars
//         private float getWaitTime()
//         {
//             int numOfUnassignedRides = 0;
//             int numOfAssignedRides = 0;
//             int numOfRidingRides = 0;
//             int numOfCarsRunning = 0;

//             numOfUnassignedRides = _dbConnector.GetUnassignedRides();
//             numOfAssignedRides = _dbConnector.GetAssignedRides();
//             numOfRidingRides = _dbConnector.GetRidingRides();
//             numOfCarsRunning = _dbConnector.GetNumberCarsRunning();

//             return ((((numOfAssignedRides * _estWaitTimeOfAssigned) +
//                              (numOfUnassignedRides * _estWaitTimeOfUnassigned)) +
//                                 (numOfRidingRides * __estWaitTimeOfRiding))
//                                     / numOfCarsRunning);
//         }
    }
}'

$WaitTimeC >> 'WaitTimeController.cs'
Write-Output "Wait Time Controller created!"

Set-Location ..

mkdir DataAccess
Set-Location DataAccess
Write-Output "Creating Data Access folder.."

$DBConnector = 'using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using API.Models;
using MySql.Data.MySqlClient;
using Swashbuckle.AspNetCore.Swagger;

namespace API.DataAccess
{
    public class DBConnector
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        // Constructor
        public DBConnector() 
        {
            Initialize();
        }

        // Initialize values
        private void Initialize() 
        {
            server = "localhost";
	        uid = "stripessql";
            database = "stripes";
            password = "stripessqldatabase";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "uid=" + uid + ";" + "password=" + password + ";" + "database=" + database + ";" + "sslmode=none;";
        
            connection = new MySqlConnection(connectionString);
        }

        // open connection to database
        public bool OpenConnection()
        {
            try 
            {
                connection.Open();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        // Close connection
        public bool CloseConnection()
        {
            try 
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        //Insert statement
        public void InsertRide(RideRequest request) 
        {
            if (OpenConnection())
            {
                MySqlCommand comm = connection.CreateCommand();

                comm.CommandText =
                    "INSERT INTO RIDES (RideId, PatronName, PhoneNumber, NumberOfPeople, Pickup, Destination, Status, TimeRequested, RequestSource,NIGHTS_NightId)" +
                    " VALUES " +
                    $"({request.RideId}, ''{request.PatronName}'', ''{request.PhoneNumber}'', ''{request.NumberOfPeople}'', ''{request.Pickup}'', ''{request.Destination}'', ''waiting'', NOW(), ''iphone'', 1)";
                if (comm.ExecuteNonQuery() != 1)
                {
                    Console.WriteLine("did not modify database");
                    throw new Exception("did not modify database");
                }
                CloseConnection();
            }
        }

        public bool IsNightActive(string todaysDate)
        {
            string query = $"Select * FROM NIGHTS WHERE Night=''{todaysDate}'';";
            var result = false;
            if (OpenConnection())
            {
                MySqlCommand comm = connection.CreateCommand();
                comm.CommandText = query;
                var queryResult = comm.ExecuteScalar();
                if(queryResult != null)
                {
                    result = true;
                }
                CloseConnection();
            }
            return result;
        }

        public void CreateNewNight(string todaysDate)
        {
            string query = $"INSERT INTO NIGHTS (NightId, Night, IsActive) VALUES (1, ''{todaysDate}'', 1);";

            if (OpenConnection())
            {
                MySqlCommand comm = connection.CreateCommand();
                comm.CommandText = query;
                comm.ExecuteNonQuery();
                CloseConnection();
            }
            return;
        }

        public int GetUnassignedRides()
        {
            string query = "SELECT COUNT(*) FROM RIDES WHERE Status = ''waiting'';";
            int result = 0;
            if (OpenConnection())
            {
                MySqlCommand comm = connection.CreateCommand();
                comm.CommandText = query;
                var result1 = comm.ExecuteScalar().ToString();
                result = int.Parse(result1);
                CloseConnection();
            }
            return result;
        }

        public int GetAssignedRides()
        {
            string query = "SELECT COUNT(*) FROM RIDES WHERE Status = ''assigned'';";
            int result = 0;
            if (OpenConnection())
            {
                MySqlCommand comm = connection.CreateCommand();
                comm.CommandText = query;
                var result1 = comm.ExecuteScalar().ToString();
                result = int.Parse(result1);
                CloseConnection();
            }
            return result;
        }

        public int GetRidingRides()
        {
            string query = "SELECT COUNT(*) FROM RIDES WHERE Status = ''riding'';";
            int result = 0;
            if (OpenConnection())
            {
                MySqlCommand comm = connection.CreateCommand();
                comm.CommandText = query;
                var result1 = comm.ExecuteScalar().ToString();
                result = int.Parse(result1);
                CloseConnection();
            }

            return result;
        }

        public int GetNumberCarsRunning()
        {
            string query = "SELECT COUNT(*) FROM CARS WHERE IsActive = ''1'';";
            int result = 0;
            if (OpenConnection())
            {
                MySqlCommand comm = connection.CreateCommand();
                comm.CommandText = query;
                var result1 = comm.ExecuteScalar().ToString();
                result = int.Parse(result1);
                CloseConnection();
            }
            return result;
        }

        public void InsertNewCar(CarRequest request)
        {
            //string query = $"INSERT INTO CARS (CarId, CarNumber, PhoneNumber, IsActive) VALUES (''{request.CarId}'', ''{request.CarNumber}'', ''{request.PhoneNumber}'', ''{request.IsActive}''";

            if (OpenConnection())
            {
                MySqlCommand comm = connection.CreateCommand();
                comm.CommandText = 
                    "INSERT INTO CARS (CarId, CarNumber, PhoneNumber, IsActive)" + 
                    " VALUES " +
                    $"({request.CarId}, ''{request.CarNumber}'', ''{request.PhoneNumber}'', ''{request.IsActive}'')"; 
                if (comm.ExecuteNonQuery() != 1)
                {
                    Console.WriteLine("Did not modify database");
                    throw new Exception("did not modify database");
                }
                CloseConnection();
            }
            return;
        }
    }
}'
$DBConnector >> 'DBConnector.cs'
Write-Output "Created Database connector"

Set-Location ..

Write-Output "Creating Models"
mkdir Models
Set-Location Models

Write-Output "Creating Car request model.."
$CarRequestM = 'using System;

namespace API.Models
{
    public class CarRequest
    {
        public int CarId { get; set; }
        public int CarNumber { get; set; }
        public string PhoneNumber { get; set; }
        public byte IsActive { get; set; }
    }
}
'
$CarRequestM >> "CarRequest.cs"
Write-Output "CarRequest created!"

Write-Output "Creating Comment Request model.."

$CommenRequestM = 'namespace API.Models
{
    public class CommentRequest
    {
        public string comment { get; set; }
    }
}'
$CommenRequestM >> "CommentRequest.cs"
Write-Output "created comment request!"


Write-Output "Creating PhoneRide Request model.."
$PhoneRideM = 'namespace API.Models
{
    public class PhoneRideRequest
    {
        public string firstName { get; set; }
        public string homeAddress { get; set; }
        public string cellPhoneNumber { get; set; }
        public string passengers { get; set; }
        public string dropoffs { get; set; }
        public string pickupLocation { get; set; }
    }
}'
$PhoneRideM >> "PhoneRideRequest.cs"
Write-Output "Phone Ride request Created!"

$RideRequestM = 'using System;

namespace API.Models
{
    public class RideRequest
    {
        public int RideId { get; set; }
        public string PatronName { get; set; }
        public string PhoneNumber { get; set; }
        public string NumberOfPeople { get; set; }
        public string Pickup { get; set; }
        public string PickupLat { get; set; }
        public string PickupLong { get; set; }
        public string Destination { get; set; }
        public string DestinationLat { get; set; }
        public string DestinationLong { get; set; }
        public string Description { get; set; }

        public enum Status
        {
            waiting,
            assigned,
            canceled,
            completed,
            riding
        }

        public DateTime TimeRequested { get; set; }
        public DateTime TimeDispatched { get; set; }
        public DateTime TimeFinished { get; set; }
        public DateTime TimePickedUp { get; set; }

        public enum RequestSource
        {
            iphone,
            android,
            caller
        }

        public int CARS_CarId { get; set; }
    }
}'
$RideRequestM >> "RideRequest.cs"
Write-Output "Ride Request Created!"

$SystemStatusM = 'namespace API.Models
{
    public class SystemStatus
    {
        public bool SystemIsUp{get;set;}
        public bool DbIsUp{get;set;}
    }
}'
$SystemStatusM >> "SystemStatus.cs"
Write-Output "System status Model Created!"

$WaitTimeM = 'namespace API.Models
{
    public class WaitTime
    {
        public string status{get;set;}
        public float waitTime{get;set;}
    }
}'
$WaitTimeM >> "WaitTime.cs"
Write-Output "Wait time model created!"
Write-Output "Models folder created!"
Set-Location ..



Write-Output "Creating Properties folder.."
mkdir Properties
Set-Location Properties
$LaunchSettings = '{
  "$schema": "http://json.schemastore.org/launchsettings.json",
  "iisSettings": {
    "windowsAuthentication": false, 
    "anonymousAuthentication": true, 
    "iisExpress": {
      "applicationUrl": "http://localhost:31147",
      "sslPort": 44364
    }
  },
  "profiles": {
    "IIS Express": {
      "commandName": "IISExpress",
      "launchBrowser": true,
      "launchUrl": "api/values",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },
    "API": {
      "commandName": "Project",
      "launchBrowser": true,
      "launchUrl": "api/values",
      "applicationUrl": "http://localhost:5000;http://104.248.54.97:5000",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}'
$LaunchSettings >> "launchSettings.json"
Write-Output "Launch settings created!!"
Write-Output "Properties complete!"
Set-Location ..

Write-Output "Creating Service Layer directory.."
mkdir ServiceLayer
Set-Location ServiceLayer

Write-Output "Creating Cars Service.."
$CarsS = 'using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Models;
using API.DataAccess;

using System.Collections.Generic;
using System;

namespace API.ServiceLayer
{    
    public class CarsService : ControllerBase
    {
        public ActionResult Post([FromBody] CarRequest request)
        {
            if (request.CarId == null || request.CarNumber == null || request.PhoneNumber == null ||
                request.IsActive == null)
            {
                return BadRequest();
            }

            var convertedCarRequest = new CarRequest()
            {
                CarId = request.CarId,
                CarNumber = request.CarNumber,
                PhoneNumber = request.PhoneNumber,
                IsActive = request.IsActive
            };
            
            try
            {
                DBConnector db = new DBConnector();
                db.InsertNewCar(convertedCarRequest);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500);
            }
            return Ok();

        }
    }
}'
$CarsS >> "CarsService.cs"
Write-Output "Cars Service created!"

$HealthS = 'using System.Threading.Tasks;
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
}'
$HealthS >> "HealthService.cs"
Write-Output "Health service created!"

$CommentS = 'using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Models;
using API.DataAccess;

using System.Collections.Generic;
using System;

using System.Text;
using System.Net.Mail;
using System.Net.Mime;

namespace API.ServiceLayer
{ 
    public class LeaveCommentService : ControllerBase
    {
        [HttpPost]
        public void Post([FromBody] CommentRequest commentToEmail)
        {
            try
            {
                MailMessage mailMsg = new MailMessage();

                // To
                mailMsg.To.Add(new MailAddress("STRIPESComments@gmail.com"));

                // From
                mailMsg.From = new MailAddress("STRIPESComments@gmail.com");

                // Subject and multipart/alternative Body
                mailMsg.Subject = "Patron Comment";
                mailMsg.Body = commentToEmail.comment;

                // Init SmtpClient and send
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", Convert.ToInt32(587))
                {
                    Port = 587, EnableSsl = true
                };
                var credentials = new System.Net.NetworkCredential("STRIPESComments@gmail.com", "1qaz@wsx#edc4");
                smtpClient.Credentials = credentials;
                smtpClient.Send(mailMsg);
            }
            
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " " + ex.InnerException);
            }
        }
    }
}'
$CommentS >> "LeaveCommentService.cs"
Write-Output "Leave comment service complete!"

$NightS = 'using System;
using API.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace API.ServiceLayer
{
    public class NightService : ControllerBase
    {
        [HttpPost]
        public ActionResult Post()
        {
            var dbConnector = new DBConnector();
            //get hours 9 hours from utc or 3 hours before central time to cover open till 3
            var todaysDate = DateTime.Today.ToString("yyyy-MM-dd");

            try
            {
                if (dbConnector.IsNightActive(todaysDate) == false)
                {
                    Console.WriteLine("night does not exist");
                    dbConnector.CreateNewNight(todaysDate);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.InnerException);
                return BadRequest();
            }

            return Ok();
        }
    }
}'
$NightS >> "NightService.cs"
Write-Output "Night service created!"

$RidesS = 'using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Models;
using API.DataAccess;

using System.Collections.Generic;
using System;

namespace API.ServiceLayer
{
    public class RidesService : ControllerBase
    {
        [HttpPost]
        public ActionResult Post([FromBody] PhoneRideRequest request)
        {
            if (request.dropoffs == null || request.passengers == null || request.firstName == null ||
                request.homeAddress == null || request.pickupLocation == null || request.cellPhoneNumber == null)
            {
                return BadRequest();
            }

            var convertedRideRequest = new RideRequest()
            {
                Pickup = request.pickupLocation,
                PhoneNumber = request.cellPhoneNumber,
                NumberOfPeople = request.passengers,
                Destination = request.homeAddress,
                PatronName = request.firstName
            };   
            
            try
            {
                DBConnector db = new DBConnector();
                db.InsertRide(convertedRideRequest);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500);
            }
            return Ok();

        }
    }
}'
$RidesS >> "RidesService.cs"
Write-Output "Rided Service created!"

$WaitTimeS = 'using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Models;
using API.DataAccess;

using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Razor.Language.Extensions;

namespace API.ServiceLayer
{
    public class WaitTimeService : ControllerBase
    {
        private int _estWaitTimeOfUnassigned = 20;
        private int _estWaitTimeOfAssigned = 10;
        private const int __estWaitTimeOfRiding = 5;
        private readonly DBConnector _dbConnector = new DBConnector();

        [HttpGet]
        public async Task<WaitTime> GetWaitTimeAsync()
        {
            WaitTime result = new WaitTime
            {
                status = getStatus(),
                waitTime = getWaitTime()
            };

            return await Task.FromResult(result);
        }

        private string getStatus()
        {   
            var todaysDate = DateTime.Today.ToString("yyyy-MM-dd");
            Console.WriteLine(todaysDate);
            try
            {
                if( _dbConnector.IsNightActive(todaysDate))
                {
                    return "running";
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return "notRunning";
            
        }
  //Approx. wait time =
  //[(Number of unassigned requested rides * Estimated wait time for unassigned ride in minutes)
  //+
  //(Number assigned rides * estimated wait time of assigned ride in minutes)] / Number of cars
        private float getWaitTime()
        {
            int numOfUnassignedRides = 0;
            int numOfAssignedRides = 0;
            int numOfRidingRides = 0;
            int numOfCarsRunning = 0;

            numOfUnassignedRides = _dbConnector.GetUnassignedRides();
            numOfAssignedRides = _dbConnector.GetAssignedRides();
            numOfRidingRides = _dbConnector.GetRidingRides();
            numOfCarsRunning = _dbConnector.GetNumberCarsRunning();

            return ((((numOfAssignedRides * _estWaitTimeOfAssigned) +
                             (numOfUnassignedRides * _estWaitTimeOfUnassigned)) +
                                (numOfRidingRides * __estWaitTimeOfRiding))
                                    / numOfCarsRunning);
        }
    }
}'
$WaitTimeS >> "WaitTimeService.cs"
Write-Output "Wait Time Service Created!"
Write-Output "Service Layer Directory Complete!"
Set-Location ..

Write-Output "Restoring all Nuget packages.."
dotnet restore
Start-Sleep -s 5

Write-Output "Building project.."
dotnet build
Start-Sleep -s 5

Write-Output "
                         __,,,,_
          _ __..-;''`--/'/ /.',-`-.
      (`/' ` |  \ \ \\ / / / / .-'/`,_
     /'`\ \   |  \ | \| // // / -.,/_,'-,
    /<7' ;  \ \  | ; ||/ /| | \/    |`-/,/-.,_,/')
   /  _.-, `,-\,__|  _-| / \ \/|_/  |    '-/.;.\'
   `-`  f/ ;      / __/ \__ `/ |__/ |
        `-'      |  -| =|\_  \  |-' |
              __/   /_..-' `  ),'  //
          fL ((__.-'((___..-'' \__.'"

Write-Output "M - I - Z!!"

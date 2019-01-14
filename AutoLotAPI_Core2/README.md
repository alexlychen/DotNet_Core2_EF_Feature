ASP.NET Core Service Applications

1. ASP.NET Core Web Service using ASP.NET Core Web API template to create RESTful services.
2. ASP.NET Core service application can be run using IIS, Kestrel, and CLI. All of the options are controlled by the settings in the launchsettings.json.
3. Format for returned json: 
   using Newtonsoft.Json.Serialization;

4. Add connection string to appsettings.Development.json

"ConnectionStrings": {
	"AutoLot": "server=(LocalDb)\\MSSQLLocalDB;database=AutoLotCore2;integrated
	security=True;MultipleActiveResultSets=True;App=EntityFramework;"
}
   
5. Change the Startup constructor with adding environment variable 

	private readonly IHostingEnvironment _env;
	public Startup(IConfiguration configuration, IHostingEnvironment env)
	{
		_env = env;
		Configuration = configuration;
	} 

6. Configure the AutoLotContext and InventoryRepo for DI support

	services.AddDbContextPool<AutoLotContext>(
		options => options.UseSqlServer(Configuration.GetConnectionString("AutoLot"),
			o => o.EnableRetryOnFailure())
		.ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.
	QueryClientEvaluationWarning)));

7. Add application-wide exception Handling

    using Microsoft.AspNetCore.Mvc.Filters;
	
	public class AutoLotExceptionFilter:IExceptionFilter
	{
		public void OnException(ExceptionContext context){}
	}
	
8. Use AutoLotMVC_Core2 in AutoLotAPI_Core2 

8.1 Update the setting file of AutoMVC_Core2
	Delte the "connectionString" and replaced with "ServiceAddress". The serviceAddress url points to AutoAPI_Core2 
    {
		{	
			...
		},
		"ServiceAddress":"http://localhost:51714/api/Inventory"
	}
	
8.2 create a static constructor for InventoryController as follow, because the Mapper can be only initialized once for The Instance
	static InventoryController()
	{
		Mapper.Initialize(cfg => {
			cfg.CreateMap<Inventory, Inventory>().ForMember(x => x.Orders, opt => opt.Ignore());
		});
	}
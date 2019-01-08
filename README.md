"DotNet_Core2_EF_Feature" 

1.Comparing Feature Sets
	EF Core supports only the Code First paradigm, but you can still scaffold all of your EF code from an existing database or create database from your code.

1.1 Changes from EF 6
     DbContext creation and configuration are based on dependency injection, and not simple inheritance;
	 Fewer data annotations, more reliance on the Fluent API;
	 Changes to the DbSet<T> and Database APIs, such as adding the Update() and FromSql() methods;
	 Major performance improvements through cleaner code as well as query improvements, such as batching;

1.2 New Features in EF Core
     Properties that are not defined on the model but can be tracked and updated by EF Core. These are commonly used for foreign keys;
	 Provides non-primary key targets for foreign keys;
	 Allows for key values to be generated on the client side;
	 Allows for global query filters, such as soft-delete or tenant ID;
	 Backing fields/field mapping
	 Mixed client-server evaluation
	 Eager loading with Include() and ThenInclude() to make cleaner LINQ statements.
	 Raw SQL queries with LINQ, return tracked entities with FromSQL();
	 Batching of statements, reducing with database interactions for higher performance;
	 Maps scalar function to C# function for use in query statements;
	 Add SQL Server LIKE capabilities in LINQ statements;
	 DbContexting pooling, with ASP.NET Core, pools DBContext instances for higher performance;
	 Maps LINQ query to C# function for precompilation and higher performance.

1.3 Usage Scenarios
	 EF Core is built on top of .NET Core, EF Core can be used with ASP.NET MVC5, Web API2.2 and even WPF
	 EF6 and EF Core can live happily i nthe same application, but you need to place the EF6 context and the EF Core context into different assemblies, but they can share the same models.
	
2. Project Code description

2.1 Adding the NuGet Packages

	Microsoft.EntityFrameworkCore
	Microsoft.EntityFrameworkCore.Design
	Microsoft.EntityFrameworkCore.Relational
	Microsoft.EntityFrameworkCore.SqlServer
	Microsoft.EntityFrameworkCore.Tools (optional; this enables using Package Manager Console for EF migrations)

2.2 Mixed client and server evaluation
     Change state from "return Table.FirstOrDefault(x => x.TheGuid.ToString() == g)" to the follwoing two statements:
		Guid g = Guid.Parse(guid);
		return Table.FirstOrDefault(x => x.Guid == g);
	
	 Avoid mixed evaluation, using throw an exception when mixed-mode evaluation occurs.
		.ConfigureWarnings(warnings=>warnings.Throw(RelationalEventId.QueryClientEvaluationWarning));

2.3 Addming the parameterless constructor
     New DbContext pooling ASP.NET Core feature to be used, there can be only one constructor on your context class, and that’s the one that takes a DbContextOptions instance. To satisfy both of these needs, create an internal parameterless constructor.
	 
	  internal AutoLotContext()
	 {
	 }
	
	To use this constructor and leverage the OnConfiguring() event handler, you need to expose the internal items in AutoLotDAL_Core2 to the AutoLotDAL_Core2.TestDriver project. Put the following statement in the AssemblyInfor.cs of AutoLotDAL_Core2 project:
		
		using System.Runtime.CompilerServices;
		[assembly: InternalsVisibleTo("AutoLotDAL_Core2.TestDriver")]

2.4 Adding the Design-Time Context Factory
	Since a public parameterless constructor prevents using DbContext pooling, you are going to need to add a class that implements the IDesign DbTimeContextFactory<TContext> interface.
	
	 public class AutoLotContextFactory : IDesignTimeDbContextFactory<AutoLotContext>
	{
	}

2.5 Using the Fluent API to Finish the Models
	The Fluent API is anothere way to shape the models and the resulting database tables and columns. You must use the Fluent API to create multicolumn indices.
	
2.6 Adding the GetTableName() Function
	This method returns the SQL Server table name for a DbSet<T>, the class and table names don't have to match.
	public string GetTableName(Type type)
	{
		return Model.FindEntityType(type).SqlServer().TableName;
	}
	
3.Creating the Database with Migrations

	EF Core has completely removed the hash from the process. All that is stored in the database is the name of the migration and the EF version number. The database definition is stored in a C# file named <contextname>ModelSnapshot.cs. Then developers can use a standard diff tool to work out any conflicting changes.

3.1 Using the EF .NET CLI Commands
	adding the following item into the project file for the AutoLotDAL_Core2 project file:
	
		<ItemGroup>
			<DotNetCliToolReference Include="Microsoft.EntityFrameworkCore.Tools.DotNet" Version="2.0.0" />
		</ItemGroup>
		
	To add the migration to create your database, make sure to save all of the files, build the project (for safe measure), and then execute the following:
		
		dotnet ef migrations add Initial --context AutoLotDAL_Core2.EF.AutoLotContext -o EF\Migrations
	
	This creates the timestamped files (similar to EF 6 migrations) that are used to update the database and creates the AutoLotContextModelSnapshot.cs file, which stores the complete database model.
	To apply this migration, simply execute the following command:
	
		dotnet ef database update

3.2 Seeding the Database
    Start by creating a new folder named DataInitialization in the AutoLotDAL_Core2 project. In this folder, add a class named MyDataInitializer.cs. Add the static InitializeData() as follows:
		
	public static class MyDataInitializer
	{
		public static void InitializeData(AutoLotContext context)
		{
			......
		}
	}
	
3.3 Raw SQL Queries

	The following two statements are quivalent if the value of tableName is CreditRisk:
	
		context.Database.ExecuteSqlCommand($"SET IDENTITY_INSERT dbo.{tableName} ON;");
		context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.CreditRisk ON;");
	
	In EF Core 2, the first query gets translated into a parameterized query, like this:

		SET IDENTITY_INSERT dbo.@p0 ON
	
3.4 Clearing the Database and Resetting Sequences
    The Database helpers for dropping and creating database:
	
	EnsureDeleted 	Deletes the database if it exists; does nothing if it doesn’t.
	EnsureCreated 	Creates the database based on the <context>ModelSnapshot code if it doesn’t exist; does nothing if it exists. 
					This is mutually exclusive of Migrate().
	Migrate 		Creates the database and executes all migrations if it doesn’t exist; applies any missing migrations if it exists. 
					This is mutually exclusive with EnsureCreated().
					
4. Adding Repositories for Code Resuse

4.1 Adding the IRepo Inteface
    public interface IRepo<T>
	{
		int Add(T entity);
		int Add(IList<T> entities);
		int Update(T entity);
		int Update(IList<T> entities);
		int Delete(int id, byte[] timeStamp);
		int Delete(T entity);
		T GetOne(int? id);
		List<T> GetSome(Expression<Func<T, bool>> where);
		List<T> GetAll();
		List<T> GetAll<TSortField>(Expression<Func<T, TSortField>> orderBy, bool ascending);
		List<T> ExecuteQuery(string sql);
		List<T> ExecuteQuery(string sql, object[] sqlParametersObjects);
	}
	
	In this version, the Add()/AddRange() and Update()/UpdateRange() methods have been converted to just Add() and Update(), respectively, with overloads.
	There is a new method, GetSome(), that takes an expression for the where clause, and there is an overload of GetAll() that takes an expression for the sort expression.

4.2 Adding the BaseRepo
	
	public class BaseRepo<T> : IDisposable,IRepo<T> where T:EntityBase, new()
	{
		private readonly DbSet<T> _table;
		private readonly AutoLotContext _db;
		protected AutoLotContext Context => _db;
		
		public BaseRepo() : this(new AutoLotContext())
		{
		}
		public BaseRepo(AutoLotContext context)
		{
			_db = context;
			_table = _db.Set<T>();
		}
		
		public void Dispose()
		{
			_db?.Dispose(); //if not null then dispose it.
		}

		public int Update(T entity)
		{
			_table.Update(entity);
			return SaveChanges();
		}
		
		public int Update(IList<T> entities)
		{
			_table.UpdateRange(entities);
			return SaveChanges();
		}
		
		public int Delete(int id, byte[] timeStamp)
		{
			_db.Entry(new T() {Id = id, Timestamp = timeStamp}).State = EntityState.Deleted;
			return SaveChanges();
		}
		
		public int Delete(T entity)
		{
			_db.Entry(entity).State = EntityState.Deleted;
			return SaveChanges();
		}
		
		public T GetOne(int? id) => _table.Find(id);
		public virtual List<T> GetAll() => _table.ToList();
	}

4.3 Retrieving Records with FromSql()

	One of the great features of FromSql() is that you can add LINQ queries on top of the call. For example, if you wanted to return all Inventory records and the related Order and Customer records, you could write the following:

    return Context.Cars.FromSql("SELECT * FROM Inventory").Include(x => x.Orders).ThenInclude(x => x.Customer).ToList();

4.4 Implementing the SaveChanges() Helper Methods
	
	The exception handlers for the SaveChanges() method on the DbContext are simply stubbed out. In a production application, you would need to handle any exceptions accordingly

	internal int SaveChanges()
	{
		try
		{
			return _db.SaveChanges();
		}
		catch (DbUpdateConcurrencyException ex)
		{
			//Thrown when there is a concurrency error
			//for now, just rethrow the exception
			throw;
		}
		catch (RetryLimitExceededException ex)
		{
			//Thrown when max retries have been hit
			//Examine the inner exception(s) for additional details
			//for now, just rethrow the exception
			throw;
		}
		catch (DbUpdateException ex)
		{
			//Thrown when database update fails
			//Examine the inner exception(s) for additional
			//details and affected objects
			//for now, just rethrow the exception
			throw;
		}
		catch (Exception ex)
		{
			//some other exception happened and should be handled
			throw;
		}
	}
	
4.5 Creating the InventoryRepo

	This handles specific work on the Inventory table that isn’t covered by the base repo. You will also need to create an IInventoryRepo interface to set you up for ASP.NET Core dependency injection.
	
	Add a constructor that takes an AutoLotContext as the parameter and chain it to the base constructor. This will be used by the DI framework in ASP.NET Core to create the repository using one of the pooled AutoLotContext instances.
	
	public class InventoryRepo : BaseRepo<Inventory>, IInventoryRepo
	{
		public InventoryRepo(AutoLotContext context) : base(context)
		{
		}
	}

4.6 Executing a SQL LIKE Query

	public List<Inventory> Search(string searchString)=> Context.Cars.Where(c => Functions.Like(c.PetName, $"%{searchString}%")).ToList();
	
	this exactly like the SQL statement as follow:
	
		SELECT * FROM Inventory WHERE PetName like '%searchString%'

5. Test-Driving AutoLotDAL_Core2
	You need to decide whether you plan on using full dependency injection in your tests or whether you will just create a new context using the parameterless constructor and use the fallback configuration in the OnConfiguring() method.
	
	Running this code first drops and creates the database (using the migrations), populates the data, and then retrieves the records, first using AutoLotContext and then using InventoryRepo.
	
	static void Main(string[] args)
	{
		Console.WriteLine("***** Fun with ADO.NET EF Core 2 *****\n");
		using (var context = new AutoLotContext())
		{
			MyDataInitializer.RecreateDatabase(context);
			MyDataInitializer.InitializeData(context);
			foreach (Inventory c in context.Cars)
			{
				Console.WriteLine(c);
			}
		}
		Console.WriteLine("***** Using a Repository *****\n");
		using (var repo = new InventoryRepo())
		{
			foreach (Inventory c in repo.GetAll())
			{
				Console.WriteLine(c);
			}
		}
		Console.ReadLine();
	}
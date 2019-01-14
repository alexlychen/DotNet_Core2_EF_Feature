ASP.NET Core Project Organization

1. The wwwroot folder contains client-side content, such as CSS, JavaScript, images and any other non-programmatic content.
2. Programming content (such as the models, views, controllers, etc) lives outside the wwwroot directory and is structured as an MVC application.
3. Running ASP.NET Core Applications:
•    From Visual Studio, using IIS Express
•	 From Visual Studio, using Kestrel
•	 From the Package Manager Console with the .NET CLI, using Kestrel
•	 From a command prompt with the .NET CLI, using Kestrel 

4. Deploying ASP.NET Core Applications
   ASP.NET Core can be deployed to multiple operating systems in multiple ways, including outside of a web server. The high-level options are as follows:
•	 On a Windows server (including Azure) using IIS
•	 On a Windows server (including Azure app services) outside of IIS
•	 On a Linux server using Apache
•	 On a Linux server using NGINX
•	 On Windows or Linux in a Docker container

5. What's New in ASP.NET Core
•	 Unified story for building web applications and RESTful services
•	 Built-in dependency injection
•	 Cloud-ready, environment-based configuration system
•	 Can run on .NET Core or the full .NET Framework
•	 Lightweight, high-performance, and modular HTTP request pipeline
•	 Integration of modern client-side frameworks and development workflows
•	 Introduction of tag helpers
•	 Introduction of view components

5.1 Built-in Dependency Injection
	Dependency injection (DI) is a mechanism to support loose coupling between objects.The ASP.NET Core DI container is typically configured in the ConfigureServices methodo the Startup class using the instance of IServiceCollection that itself is injected in.
	There are four lifetime options:
	• Transient 	Created each time they are needed.
	• Scoped 		Created once for each request. Recommended for Entity Framework DbContext objects.
	• Singleton 	Created once on first request and then reused for the lifetime of the object. This is the recommended approach versus implementing your class as a Singleton.
	• Instance 		Similar to Singleton but created when Instance is called versus on first request.

5.2 Could-Ready Environment-Based Configuration System
	Determining the Runtime Environment: ASP.NET Core applications look for an environment variable named ASPNETCORE_ENVIRONMENT to determine the current running environment. There are three different environments as Development, Staging, and Production.
	
5.3 Running on .NET Core or the Full .NET Framework
    ASP.NET Core and ASP.NET MVC cannot coexist in the same application.
	
5.4 Lightweight and Modular HTTP Request Pipeline

5.5 Integeration of Client-Side Frameworks
	Tag helpers encapsulate server-side code that shapes the attached element. It's recommended that tag helpers be used for input controls in ASP.NET Core applications.
	
	The following is example of HTML helper:
	@Html.Label("FullName", "Full Name:", new (@class="customer"})
	
	The following is example of tag helper:
	<label class="customer" asp-for="FullName">Full Name:</label>
	
	The Form Tag Helper
	The form tag helper replaces the Html.BeginForm and Html.BeginRouteForm HTML helpers. The antiforgery token is included by default with all form tag helpers.
	
	The Anchor Tag Helper
	The anchor tag helper replaces the Html.ActionLink HTML helper.
	<a asp-controller="Inventory" asp-action="Edit" asp-route-id="@Model.Id">Edit</a>
	
	The Input Tag Helper
	In addition to autogenerating the HTML id and name attributes, as well as any HTML5 data-val validation attributes, the tag helper builds the appropriate HTML markup base on the data type of the target property.
	
	Enabling Tag Helpers
	The tag helpers, like any other code, must be made visible to any code that wants to use them. The _ViewImports.html file contains the following line:
	@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
	
5.6 Building the Server-side Code

	View components are composed of a server-side class that derives from ViewComponent and uses the naming covention <customname>ViewComponent.
	
	The partical view to be rendered must be one of these two directories:
	Views/<parent_controller_name_calling_view >/Components/<view_component_name>/<view_name>
	Views/Shared/Components/<view_component_name>/<view_name>
	
	Invoking View Components
	View components are typically invoked from a view. The syntax is as follow:
	@await Component.InvokeAsync("Inventory")
	
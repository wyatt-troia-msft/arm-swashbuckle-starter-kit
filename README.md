# arm-swashbuckle-starter-kit
A starter kit for auto-generating Swagger in accordance with the Azure Resource Manager resource provider [requirements](https://github.com/Azure/azure-rest-api-specs/blob/main/documentation/openapi-authoring-automated-guidelines.md#R1001), using [Swashbuckle](https://github.com/domaindrivendev/Swashbuckle.AspNetCore).

Swashbuckle generates a Swagger file from C# code by inspecting .NET controllers and attributes. You can customize the generation through the use of "filters." This kit includes filters, along with other configurations, that modify the Swagger generation to conform to ARM requirements for RP Swagger specs.

## Instant Demo

1. Clone this repo.
2. From the repo directory, run `dotnet build`. This will auto-generate a Swagger file, `swagger.json`, in the "ArmSwashbuckleStarterKit" project directory (sample [here](./ArmSwashbuckleStarterKit/swagger.json)). Inspect the file's last-modified time to confirm it is newly updated.

## Integrating with Your Own Repo

1. Copy the kit's [Attributes](./ArmSwashbuckleStarterKit/Attributes) and [Swagger](./ArmSwashbuckleStarterKit/Swagger) directories to your repo.
2. Familiarize yourself with the attributes you've copied over by reading the accompanying comments, then apply them throughout your repo as appropriate. You can see sample usage of some of the attributes in the controller and models in this repo.
3. Copy the kit's [`ResourceListResultModel.cs`](./ArmSwashbuckleStarterKit/Models/ResourceListResultModel.cs) file to your repo and update any controller methods that return lists to return this type.
4. Copy the kit's [Exceptions](./ArmSwashbuckleStarterKit/Exceptions) directory to your repo and update all error responses from your API to be of type [`ArmErrorResponse`](./ArmSwashbuckleStarterKit/Exceptions/ArmErrorResponse.cs). One approach is to use [middleware](./ArmSwashbuckleStarterKit/Middlewares/ExceptionHandlerMiddleware.cs).
5. Add a `[Consumes("application/json")]` attribute to all controller operations that accept a request body. This will restrict the operation's "consumes" types to "application/json".
6. For any delete operations in your controllers, add a `[Produces("application/json")]` attribute and ensure at least one `[ProducesResponseType()]` attribute is passed a `typeof` argument. Without the `typeof`, Swashbuckle will not add the "produces" property to the operation in the Swagger, which ARM requires even while delete operations are not supposed to return any response body. The [`DeleteProducesOperationFilter`](./ArmSwashbuckleStarterKit/Swagger/DeleteProducesOperationFilter.cs) will remove the inaccurate response schema.
7. The kit's [`Startup.cs`](./ArmSwashbuckleStarterKit/Startup.cs) file registers and configures Swashbuckle and registers our custom filters. Copy and paste all non-boilerplate code to your own `Startup.cs`. You will need to update the values that are specific to "ArmSwashbuckleStarterKit" to values appropriate for your own repo.
8. Copy the commented sections of the kit' [`.csproj`](./ArmSwashbuckleStarterKit/ArmSwashbuckleStarterKit.csproj) and paste in your `Startup.cs`. You must use the same version of all the Swashbuckle Nuget packages you use; otherwise, you may run into errors. 
9. [Install Swashbuckle.AspNetCore.Cli as a local tool in your project](https://github.com/domaindrivendev/Swashbuckle.AspNetCore#using-the-tool-with-the-net-core-30-sdk-or-later). This is the tool that will be invoked by the `swagger tofile` post-build command, which you can find in the [`.csproj`](./ArmSwashbuckleStarterKit/ArmSwashbuckleStarterKit.csproj) file.
10. Run a `dotnet build` (however you normally do), and check our your freshly-generated Swagger file!

## Resources

- [Get started with Swashbuckle and ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-5.0&tabs=visual-studio)
- [ARM OpenAPI Specifications Authoring - Automated Guidelines](https://github.com/Azure/azure-rest-api-specs/blob/main/documentation/openapi-authoring-automated-guidelines.md)
## Disclaimer

This toolkit is not polished or thoroughly tested. It is one RP's attempt to create a generic toolkit out of the solutions it developed to ARM-compliantly autogenerate its Swagger spec. If you find a bug or want to build on it, we welcome PRs.

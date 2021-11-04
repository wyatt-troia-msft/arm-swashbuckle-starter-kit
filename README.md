# arm-swashbuckle-starter-kit
A starter kit for auto-generating Swagger in accordance with Azure Resource Manager resource provider requirements, using Swashbuckle.

Swashbuckle generates a Swagger file by inspecting .NET controllers and attributes. You can customize the generation through the use of "filters." This kit includes filters, along with other configurations, that modify the Swagger generation to conform to ARM requirements for RP Swagger specs.

## Instant Demo

1. Clone this repo
2. From the repo directory, run `dotnet build`. This will auto-generate a Swagger file, `swagger.json`, in the "ArmSwashbuckleStarterKit" project directory (sample [here](./ArmSwashbuckleStarterKit/swagger.json)). Inspect the file's last-modified time to confirm it is newly updated.

## Integrating with Your Own Repo

1. Copy the kit's [Attributes](./ArmSwashbuckleStarterKit/Attributes) and [Swagger](./ArmSwashbuckleStarterKit/Swagger) directories to your repo.
2. Familiarize yourself with the attributes you've copied over, then apply them throughout your repo as appropriate.
3. Copy the kit's [`ResourceListResultModel.cs`](./ArmSwashbuckleStarterKit/ResourceListResultModel.cs) file to your repo and update any controller methods that return lists to return this type.
4. Copy over to your project's corresponding files everything they are missing from the kit's [`.csproj`](./ArmSwashbuckleStarterKit/ArmSwashbuckleStarterKit.csproj) and [`Startup.cs`](./ArmSwashbuckleStarterKit/Startup.cs) files. You will need to update the values that are specific to "ArmSwashbuckleStarterKit" to values appropriate for your own repo.
5. If you didn't copy over the package references in the last step, install the same version of the [Swashbuckle.AspNetCore](https://www.nuget.org/packages/Swashbuckle.AspNetCore/), [Swashbuckle.AspNetCore.Annotations](https://www.nuget.org/packages/Swashbuckle.AspNetCore.Annotations/), and (optionally, if using Newtonsoft.Json as a serializer) [Swashbuckle.AspNetCore.Newtonsoft](https://www.nuget.org/packages/Swashbuckle.AspNetCore.Newtonsoft/) NuGet packages in your project.
6. [Install Swashbuckle.AspNetCore.Cli as a local tool in your project](https://github.com/domaindrivendev/Swashbuckle.AspNetCore#using-the-tool-with-the-net-core-30-sdk-or-later)
7. Run a `dotnet build` (however you normally do), and check our your freshly-generated Swagger file!

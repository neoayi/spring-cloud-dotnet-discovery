# .NET Service Discovery & Registration
A Service Registry provides a database which applications can use in implementing the Service Discovery pattern; one of the key tenets of a microservice-based architecture. Trying to hand-configure each client of a service or adopt some form of access convention can be difficult and prove to be brittle in production. Instead, your applications can use a Service Registry to dynamically discover and call registered services.

There are several popular options for Service Registries. Netflix built and then open-sourced their own service registry, Eureka. Another relatively new, but increasingly popular option is Consul. 

This repo contains various packages for interacting with Service Registries.  The [Pivotal.Discovery.Client](https://github.com/pivotal-cf/spring-cloud-dotnet-discovery/tree/master/src/Pivotal.Discovery.Client) provides a configurable generalized interface to Service Discovery and Registration.  Currently you can use the client to work with the [Spring Cloud Services Eureka Server](http://docs.pivotal.io/spring-cloud-services/service-registry/) as a Service Registry. In the near future support will be added for Consul.

Windows Master:  [![AppVeyor Master](https://ci.appveyor.com/api/projects/status/84oangqh3o4fyt7b/branch/master?svg=true)](https://ci.appveyor.com/project/steeltoe/spring-cloud-dotnet-discovery/branch/master)

Windows Dev:  [![AppVeyor Dev](https://ci.appveyor.com/api/projects/status/84oangqh3o4fyt7b/branch/dev?svg=true)](https://ci.appveyor.com/project/steeltoe/spring-cloud-dotnet-discovery/branch/dev)

Linux/OS X Master: [![Travis Master](https://travis-ci.org/pivotal-cf/spring-cloud-dotnet-discovery.svg?branch=master)](https://travis-ci.org/pivotal-cf/spring-cloud-dotnet-discovery)

Linux/OSX Dev: [![Travis Dev](https://travis-ci.org/pivotal-cf/spring-cloud-dotnet-discovery.svg?branch=dev)](https://travis-ci.org/pivotal-cf/spring-cloud-dotnet-discovery)

# .NET Runtime & Framework Support
The packages are intended to support both .NET 4.5.1+ and .NET Core (CoreCLR/CoreFX) runtimes.  They are built and unit tested on Windows, Linux and OSX.

While the primary usage of the providers is intended to be with ASP.NET Core applications, they should also work fine with UWP, Console and ASP.NET 4.x apps.
 
Currently all of the code and samples have been tested on .NET Core 1.0.0-RC2/SDK Preview 1, .NET 4.5.1, and on ASP.NET Core 1.0.0-RC2-final.

# Usage
See the Readme for each enclosed project for more details on how to make use of it in an application.

# Nuget Feeds
All new development is done on the dev branch. More stable versions of the packages can be found on the master branch. The latest prebuilt packages from each branch can be found on one of two MyGet feeds. Released version can be found on nuget.org.

[Development feed (Less Stable)](https://www.myget.org/gallery/steeltoedev) - https://www.myget.org/gallery/steeltoedev

[Master feed (Stable)](https://www.myget.org/gallery/steeltoemaster) - https://www.myget.org/gallery/steeltoemaster

[Release or Release Candidate feed](https://www.nuget.org/) - https://www.nuget.org/. 

# Building Packages & Running Tests - Windows
To build the packages on windows:

1. git clone ...
2. cd <clone directory>
3. Install .NET Core SDK
4. dotnet restore src
5. cd src\<project> (e.g. cd src\Pivotal.Discovery.Client)
6. dotnet pack --configuration <Release or Debug>  

The resulting artifacts can be found in the bin folder under the corresponding project. (e.g. src/Pivotal.Discovery.Client/bin

To run the unit tests:

1. git clone ...
2. cd <clone directory>
3. Install .NET Core SDK 
4. dotnet restore test
5. cd test\<test project> (e.g. cd test\Pivotal.Discovery.Client.Test)
6. dotnet test

# Building Packages & Running Tests - Linux/OSX
To build the packages on Linux/OSX:

1. git clone ...
2. cd <clone directory>
3. Install .NET Core SDK
4. dotnet restore src
5. cd src/<project> (e.g.. cd src/Pivotal.Discovery.Client)
6. dotnet pack --configuration <Release or Debug> 

The resulting artifacts can be found in the bin folder under the corresponding project. (e.g. src/Pivotal.Discovery.Client/bin

To run the unit tests:

1. git clone ...
2. cd <clone directory>
3. Install .NET Core SDK 
4. dotnet restore test
5. cd test\<test project> (e.g. cd test/Pivotal.Discovery.Client.Test)
6. dotnet test --framework netcoreapp1.0

# Sample Applications
See the [Samples](https://github.com/SteelToeOSS/Samples) repo for examples of how to use these packages.

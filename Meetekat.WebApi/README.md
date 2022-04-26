# Meetekat.WebApi

Meet-e-Kat WebApi project.


## Project structure

### Overview

The project consists of the following sections:
1. [Entry Point](#entry-point)
2. [Application Configuration](#application-configuration)
3. [Domain Model](#domain-model)
4. [Seedwork or Micro-Framework](#seedwork-or-micro-framework)
5. [Features or Use Cases](#features-or-use-cases)
6. [Infrastructure modules](#infrastructure-modules)

### Entry Point

The `Program.cs` file is the entry point for the application. It contains DI
and Middleware configurations. This is the "heart" of the application, since
it's responsible for configuring a working application version

### Application Configuration

Application configuration is split into several parts. Web-server configuration
is located under the `Properties` folder in the `launchSettings.json` file.

After the application is run, it scans environment for configuration parameters.
Instead of configuring environment variables on the local machine for
Development purposes You can use the `appsettings.json` and
`appsettings.Development.json` files. The `appsettings.json` file provides a
full list of supported configuration parameters along with the default values
for them. If You want to override some of these values (e.g. `Password` under
the `Persistence` section), You can copy this file and rename the copy to the
`appsettings.Development.json`. This way You wouldn't need to change the content
of the `appsettings.json` file, which is quite dangerous (it only takes one
incorrectly resolved configuration file merge conflict to bring the
configuration into a terrible state). So please, put Your local development
configuration inside the `appsettings.Development.json` file. You can also
delete all untouched configuration parameters (including whole sections), since
application loads the root `appsettings.json` file first and only then overrides
it with `appsettings.Development.json`.

Action point: You might want to refactor application configuration a bit and put
`appsettings*.json` files under the `Properties` directory as well.




### Domain Model

Domain model is represented by the anemic POCO entities located under the
`Entities` namespace. It was decided not to use Aggregates for now, but since
the application is currently in the active development stage, this decision
might change quite a lot.

### Seedwork or Micro-Framework

This is all framework-like stuff designed specifically to fulfill this
application needs. All kinds of things are here: configuration utilities,
migration templates, custom validation attributes and so on. In the future we
might want to ship this Seedwork as a NuGet package for all adjacent
applications (or microservices), but for now it's placed under the `Seedwork`
namespace.

### Features or Use Cases

This is the cornerstone of the entire application. The `Features` namespace
contains all application logic grouped by sections. Each feature is
**self-sufficient** and must **not** depend on any other features. Try to reuse
the code between different features **as little as possible** (yes, this is a
serious violation of the DRY principle and this is **the most important** aspect
of the chosen application architecture).

### Infrastructure modules

If You take a look at the project structure You will notice, that project also
has namespaces like `Auth`, `Persistence`, `Swagger` and maybe others, that
weren't described jut yet. These namespaces contain configuration specific to
the certain infrastructure units or modules. Try as hard as You can to push all
infrastructure-specific configuration, rules, services and other stuff in these
infrastructure-modules namespaces. 

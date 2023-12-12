# AspNetIdentity
**Introducing to the ASP.NET Identity with a simple example**

### Road Map
- [Dependencies](#dependencies)
  - [EntityFrameworkCore](#entityframeworkcore)
  - [Identity](#identity)
  - [JWT Security](#jwt-security)

<br>

### Dependencies
- #### [EntityFrameworkCore]
  - [Microsoft.EntityFrameworkCore](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore)
  - [Microsoft.EntityFrameworkCore.InMemory](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.InMemory) <- In Memory Database Provider
  - [Microsoft.EntityFrameworkCore.Tools](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Tools) <- For Migration Operations
  - [Microsoft.EntityFrameworkCore.Design](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Design) <- For Persistence Layer Working on Migrations
- #### [Identity]
  - [Microsoft.AspNetCore.Identity](https://www.nuget.org/packages/Microsoft.AspNetCore.Identity)
  - [Microsoft.AspNetCore.Identity.EntityFrameworkCore](https://www.nuget.org/packages/Microsoft.AspNetCore.Identity.EntityFrameworkCore) <- For Data Layer
- #### [JWT Security]
  - [Microsoft.AspNetCore.Authentication.JwtBearer](https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.JwtBearer)

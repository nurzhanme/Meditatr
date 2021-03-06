# Meditatr

[![NuGet Badge](https://buildstats.info/nuget/Meditatr)](https://www.nuget.org/packages/Meditatr/)

Meditatr is a dotnet CLI tool for scaffolding Mediator Patterns (Command and Queries and their Handlers)

## Installation

To install use dotnet cli:

```
dotnet tool install --global Meditatr
```

To uninstall:

```
dotnet tool uninstall Meditatr --global
```

## How to Use

Show help information:

```
med -h
```
### Commands

Show help for commands:

```
med Command -h
```

Example creating command and its handler:

```
med Command -m Product -a Add -r long -p MyProject.Dtos -H MyProject.Handlers
```
as a result two classes have been created:

* Command class named 'AddProductCommand':

```c#
using MediatR;

namespace MyProject.Dtos.Commands.ProductCommands
{
    public class AddProductCommand : IRequest<long>
    {
    }
}
```

* and its handler named 'AddProductCommandHandler'

```c#
using MediatR;
using MyProject.Dtos.Commands.ProductCommands;
using System.Threading;
using System.Threading.Tasks;

namespace MyProject.Handlers.Commands.ProductCommands
{
    public class AddProductCommandHandler : IRequestHandler<AddProductCommand, long>
    {
        public Task<long> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
```

### Queries

Show help for queries:

```
med Query -h
```

Example creating query and its handler:

```
med Query -m Product -a Get -r ProductDto -p MyProject.Dtos -H MyProject.Handlers
```
as a result two classes have been created:

* Query class named 'GetProductQuery':

```c#
using MediatR;

namespace MyProject.Dtos.Queries.ProductQueries
{
    public class GetProductQuery : IRequest<ProductDto>
    {
    }
}
```

* and its handler named 'GetProductQueryHandler'

```c#
using MediatR;
using MyProject.Dtos.Queries.ProductQueries;
using System.Threading;
using System.Threading.Tasks;

namespace MyProject.Handlers.Queries.ProductQueries
{
    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, ProductDto>
    {
        public Task<ProductDto> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
```

### Data Transfer Objects 

Show help for dtos:

```
med Dto -h
```

Example creating dto:

```
med Dto -m Product -p MyProject.Dtos
```
NOTE: Command MUST be executed where model class located

Let's assume there is a model class with following structure

```c#

namespace MyProject.Domain
{
    public class Product
    {
        public long Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; }
    }
}
```

after execution of the command 'ProductDto' class had been created

```c#

namespace MyProject.Dtos.Queries.ProductQueries
{
    public class ProductDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
```

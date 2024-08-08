# C# .NET GameEngine

## Standalone game engine written in .NET C# using a multiple project paradigm to enforce modularity

### General solution design:
This repository consists of 5 projects:
* GameEngine.Core:
    * includes the main communication protocol between the modules, and common services
* PhysicsEngine:
    * Keeps track of current forces working upon objects and deals with collision calculations
* InputEngine:
    * Provides a way to check for user inputs
* SoundEngine:
    * Responsible for spatial and static sound playing
* GameEngine:
    * Incorporates all of the projects above into 1 product

### Aims:
* Educational:
    * The main aim behind this project is to learn about game engines and the cooperation of multiple projects into 1 working product.
* Modularity over efficiency:
    * The aim with this project wasn't making an optimized game engine that could run the latest features and whatnot, it was intended more to learn about design patterns and project incorporation between a few modules, so I've made the design choice of abstracting every project to a specific interface, for example, the GraphicsEngine project currently has to implement IGraphicsEngine in it's DI Container for the GameEngine to work, the same applies for the other projects.
    * This approach is anti-efficiency because it makes the renderer for instance stick to it's interface (which doesn't give it a lot of freedom), so specific render features must be implemented for every render api using shader code only and not specific features supplied by that api.

### Design patterns:
* Dependency injection:
    * The projects are based on dependency injection, where every little thing is a service of its own, each project has a respective ServiceRegisterer class that encapsulates the service registration process for that specific project, it is located inside the "Services" folder in each of the projects, and it is quite straightforward to understand.
* Factory pattern:
    * Factories are used primarily in the GraphicsEngine project, from Texture factories up to Material factories

### Project structure
I tried making the project structure for each of the projects similar making it easy to navigate, the basic project structure is as follows:
* Components
    * Contains components for the specific project
* Extensions
    * Contains extension classes
* Services
    * Implementations
        * Contains service implementations
    * Interfaces
        * Contains service interfaces
    * ServiceRegisterer.cs
        * Defines the service registration for the project
* [ProjectName]Provider.cs
    * Used for building and getting the appropriate finalized interface from the project

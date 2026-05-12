# ZordNest.Navigator

A lightweight, viewmodel-first, and framework-agnostic navigation library for .NET desktop 
applications built entirely in C#.

## What It Solves

It treat screens or a certain portion of a screen as the function of the ViewModel given. You can split your larger desktop screens into different portions
and let the Viewmodels determine the current control to display.


ZordNest.Navigator introduces the **StackHost** concept — treating 
individual portions of a window as independent navigable containers, 
each with their own back-stack and lifecycle, rather than treating the 
entire window as one navigation host.

## Features

- **StackHost pattern** — multiple independent navigable containers per window
- **Key-based navigation** — navigate by string keys or web-like routes, not concrete types
- **NavigatorViewModelStore** — centralised ViewModel lifecycle management with scoped DI per instance
- **INavigableViewModel** — lifecycle hooks: `Created`, `Resumed`, `Paused`
- **NavigateTo** — root navigation that clears the back-stack
- **AddToStack** — stack navigation that preserves history
- **RouteBased** - Navigation is route-based using the web structure e.g. (posts/post/more-comments). Note: The route is purely string-based and static. To pass
  dynamic values like id e.g. (posts/post/{id}), you will use the AddToStack method and pass Dictionary query params like new Dictionary<string,object>{{"id", 5}};
- **Sub-route eviction** — evicting a parent route automatically evicts all child routes
- **Bounded back-stack** — configurable depth limit to prevent memory pressure
- **Source Generator** — zero reflection at runtime via `[Navigable("key")]` attribute auto-registration
- **Framework agnostic** — works with Community Toolkit MVVM, ReactiveUI, or plain viewmodels
- **Dependency Injection** - It uses the official Microsoft DI container to resolve scoped viewmodels instance, and provides extension methods on
  the IServiceCollection for configuring and kick-starting the library. 

## Quick Start

```csharp
// 1. Mark your ViewModels
[Navigable("dashboard")]
public class DashboardViewModel : INavigableViewModel { }

[Navigable("posts/post/comment")]
public class CommentsViewModel : INavigableViewModel { }

// 2. Register at startup — generated automatically, zero reflection


// 3. Navigate
navigator.NavigateTo("dashboard");           // root — clears stack
navigator.AddToStack("file-manager", null);  // push to stack
navigator.PopStack();                        // go back
```

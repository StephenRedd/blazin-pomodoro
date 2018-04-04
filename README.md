# Blazin' Pomodoro

An experimental task-list with pomodoro timer features, written for [Microsoft's Blazor](https://github.com/aspnet/Blazor) framework.

[![Shipping faster with ZenHub](https://cdn.rawgit.com/ZenHubIO/support/master/zenhub-badge.svg)](https://zenhub.com)

[![Build status](https://ci.appveyor.com/api/projects/status/5lhjs3c6sfq4h726?svg=true)](https://ci.appveyor.com/project/StephenRedd/blazin-pomodoro)

# Overview

Basically, this is the stock ASP.NET hosted blazor sample template, with the TODO list from the blazor [getting started walkthrough](https://blogs.msdn.microsoft.com/webdev/2018/03/22/get-started-building-net-web-apps-in-the-browser-with-blazor) combined with a custom pomodoro timer of my own. 

## Online Sample

An [online demo](https://blazinpomodorosample.azurewebsites.net) has been posted to Azure as an App Service.

## Running locally

You will need the latest dotnet SDK, VS 2017 preview, and extensions to work with this project locally. 

Follow the three simple ['Getting Started' steps from the Blazor README.md here](https://github.com/aspnet/Blazor#getting-started). 


## Implementation Notes:

- Unit test coverage is much thinner than I like, but with so many new pre-release technologies the time it would take to write and maintain proper test coverage was larger than my available time. I would especially liked to have included tests for the actual pomodoro timer, as this is where there is substantial logic to test. In the real-world, "it takes too much time" is never an acceptable reason not to write tests.

- I would also normally setup Swagger/OpenAPI documentation for any JSON API, but again -- time was limited.

- I would love to persist the timer to the DB, and retrieve it on load. That way, if a timer was running and the browse closed you could resume the timer where you left off. This capability is inherent in the design already, but I didn't have time to wire up the actual persistence API and logic for retrieving it onload.

- Logging is setup server side, but I didn't have the chance to see if MS logger extensions and serilog can be made to work on the client; while it's probably a bad idea, I am interested in seeing if I could log from browsers to a central SEQ server for example.

- Blazor itself if **really** raw. While it is amazing what it does, the actual framework is nowhere near as full-featured as proper JS frameworks; but it has massive potnetial. It does have quirks. For example, sometimes the delete button for tasks just doesn't work -- no error, just doesn't fire the event. The VS 2017 editor is also super-frustrating for Blazor files.

- As I am not particularly familiar with the Pomorodo techniques for time management, and it isn't a time management system I would every personally use, I make no guarantees about the sanity of the implemenation here. The 'business rules', such as they are, were based on about 10 minutes of online research on the technique.

- This is a single instance application; and would not function correctly in a multi-node deployment environment.

- LiteDB is used for simplicity, and to avoid an external dependency on an external data store. 

- It seems out-of-box Docker support is a bit wonky for this stack; I could probably work out the kinks with some time, but it hardly seemed worth the effort given the preview nature of this tech stack. Instead, I chose to use Appveyor for CI builds and target the deployments to a simple Azure App Services instance.
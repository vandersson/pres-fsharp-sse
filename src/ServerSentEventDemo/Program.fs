module ServerSentEventDemo.Program

open System.IO
open Json
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.FileProviders
open Microsoft.Extensions.DependencyInjection
open Giraffe
open Microsoft.Extensions.Hosting
open ServerSentEventDemo.ClientService
open ServerSentEventDemo.DataGen


type Services =
    {
        clientService : ClientService
        dummyDataService: DummyDataService
    }
let api (services: Services) =
    choose [
        GET >=>
            choose [
                route "/" >=> text "pong"
                route "/nowplaying" >=>
                    choose [
                        mustAccept ["application/json"] >=> NowPlayingHandler.nowPlayingJson services.dummyDataService
                        mustAccept ["text/event-stream"] >=> NowPlayingHandler.nowPlayingEventStream services.clientService 
                    ]
            ]
        POST >=>
            choose [
                routef "/nowplaying/generate/%s" (DummyDataHandler.generateNewItem services.clientService services.dummyDataService) 
            ]
    ]

let configServices () =
    let dummyDataService = DummyDataService()
    let clientService = ClientService(dummyDataService)
    {
        clientService = clientService
        dummyDataService = dummyDataService 
    }
    
    
[<EntryPoint>]
let main args =
    let builder = WebApplication.CreateBuilder(args)
    
    builder.Services.AddGiraffe() |> ignore
    builder.Services.AddSingleton<Json.ISerializer>(SystemTextJson.Serializer(jsonOptions)) |> ignore   
    
    
    let app = builder.Build()
    
    app.UseStaticFiles(StaticFileOptions(
        FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "static")),
        RequestPath = "/static")) |> ignore
    
    let services = configServices()

    let lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>()
    lifetime.ApplicationStopping.Register(fun () -> services.clientService.terminate()) |> ignore
    
    app.UseGiraffe(api services)
    app.Run("http://*:3000")

    0


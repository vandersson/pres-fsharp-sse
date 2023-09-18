module ServerSentEventDemo.NowPlayingHandler

open Microsoft.AspNetCore.Http
open ServerSentEventDemo.ClientConnection
open ServerSentEventDemo.ClientService
open Giraffe
open ServerSentEventDemo.DataGen

let nowPlayingJson (dummyDataService: DummyDataService): HttpHandler =
    handleContext(fun ctx ->
        task {
            return! ctx.WriteJsonAsync <| dummyDataService.NowPlaying().Values
        })


let nowPlayingEventStream (clientService: ClientService): HttpHandler =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        task {
            ctx.Response.ContentType <- "text/event-stream" 
            
            use conn = new ClientConnection(ctx.Response, ctx.RequestAborted)
            let clientId = clientService.registerClient conn 
            
            let _ = ctx.RequestAborted.WaitHandle.WaitOne()
            
            clientService.disconnectClient clientId
            return! next ctx
        }


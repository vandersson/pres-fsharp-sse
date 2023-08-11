module ServerSentEventDemo.DummyDataHandler

open Giraffe
open Microsoft.AspNetCore.Http
open ServerSentEventDemo.ClientService
open ServerSentEventDemo.DataGen

let generateNewItem (clientService: ClientService) (dummyData: DummyDataService) (channelInput: string) =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        task {
            match Channels.parseChannel channelInput with
            | None -> return! (setStatusCode 400 >=> text "Invalid channel") next ctx
            | Some channel ->
                let indexPoint = dummyData.GenerateIndexPoint channel
                clientService.sendNowPlaying indexPoint
                return! (json indexPoint) next ctx
        }
        


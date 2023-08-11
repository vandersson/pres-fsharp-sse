module ServerSentEventDemo.ClientService

open System
open System.Collections.Concurrent
open System.Text 
open ServerSentEventDemo.ClientConnection
open ServerSentEventDemo.DataGen
open ServerSentEventDemo.NowPlaying


type EventType =
    | NowPlaying
    
module EventType =
    let string = function | NowPlaying -> "nowplaying"

let createMsg (event: EventType) data =
    let event =
        $"event: {EventType.string event}\ndata: {Json.serialize data}\n\n"
     
    Encoding.UTF8.GetBytes event
    

type ClientService(dummyData: DummyDataService) =
    
    let connectedClients = ConcurrentDictionary<string, ClientConnection>()

    let rec addClient client =
        let id = Guid.NewGuid().ToString()
        if connectedClients.TryAdd (id, client) then
            id
        else 
            addClient client
            
            
    member this.registerClient client =
        let id = addClient client
        let nowPlaying = dummyData.NowPlaying()
        nowPlaying.Values |> Seq.iter (createMsg NowPlaying >> client.sendSSEMsg)
        id
            
    member this.disconnectClient (id: string) =
        connectedClients.TryRemove(id) |> ignore
    
    
    member this.sendNowPlaying (event: IndexPoint) =
        let msg = createMsg NowPlaying event
        connectedClients.Values |> Seq.iter (fun c -> c.sendSSEMsg msg)
    

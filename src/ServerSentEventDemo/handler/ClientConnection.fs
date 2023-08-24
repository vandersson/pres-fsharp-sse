module ServerSentEventDemo.ClientConnection

open System
open System.Threading
open Microsoft.AspNetCore.Http
    
type Msg =
    | Msg of byte[]
    | Close
    
type ClientConnection(response: HttpResponse, token: CancellationToken) =
    
    let mailbox = MailboxProcessor.Start(fun (inbox: MailboxProcessor<Msg>) ->
        let rec loop state =
            async {
                let! msg = inbox.Receive()
                match msg with
                | Msg body ->
                    do! response.Body.WriteAsync(body, 0, body.Length, token) |> Async.AwaitTask
                    return! loop state
                | Close ->
                    response.HttpContext.Abort()
                    return ()
            }
        loop ())        

    member this.sendSSEMsg (msg: byte[]) =
        mailbox.Post <| Msg msg
        

    member this.CloseConnection () =
        mailbox.Post Close
        
    interface IDisposable with
        member this.Dispose() = (mailbox :> IDisposable).Dispose()
        
    

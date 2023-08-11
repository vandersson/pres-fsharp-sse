module ServerSentEventDemo.ClientConnection

open System
open System.Threading
open Microsoft.AspNetCore.Http
    
type ClientConnection(response: HttpResponse, token: CancellationToken) = // todo bare response body inn?
    
    let mailbox = MailboxProcessor.Start(fun (inbox: MailboxProcessor<byte[]>) ->
        let rec loop state =
            async {
                let! msg = inbox.Receive()
                do! response.Body.WriteAsync(msg, 0, msg.Length, token) |> Async.AwaitTask
                return! loop state
            }
        loop ())        

    member this.sendSSEMsg (msg: byte[]) =
        mailbox.Post msg
        
        
    interface IDisposable with
        member this.Dispose() = (mailbox :> IDisposable).Dispose()
        
    

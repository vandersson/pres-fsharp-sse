module ServerSentEventDemo.Channels

open System

type RadioChannel =
    | P1
    | P2
    | P3
    | P13

module RadioChannel =
    let channels = [| P1; P2; P3; P13 |]
    
let parseChannel (s: string) =
    Some s
    |> Option.filter (not << String.IsNullOrWhiteSpace)
    |> Option.map (fun c -> c.Trim().ToLower())
    |> Option.bind (function
        | "p1" -> Some P1
        | "p2" -> Some P2
        | "p3" -> Some P3
        | "p13" -> Some P13
        | _ -> None)
        

module ServerSentEventDemo.DataGen

open System
open NowPlaying
open Channels


module Rand =
    let private r = Random()
    let arrayItem (l: 'a []) =
        l[r.Next(l.Length)]
        
    let fiftyFifty (a: unit -> 'a) (b: unit -> 'a) =
        if r.NextSingle() < 0.5f then
            a ()
        else
            b ()
        
let songs =
    [|
        { songTitle = "To the Mountains"; artist = "Bigbang" }
        { songTitle = "Cirrus"; artist = "Bonobo" }
        { songTitle = "Teardrop"; artist = "Massive Attack" }
        { songTitle = "Baltimore"; artist = "Nina Simone" }
        { songTitle = "Gimme Shelter"; artist = "The Rolling Stones" }
        { songTitle = "Good Feeling"; artist = "Violent Femmes" }
        { songTitle = "September"; artist = "Earth, Wind & Fire" }
    |]
    
let editorialItems =
    [|
        { text = "Siste nytt" }
        { text = "Gjett låta konkurranse" }
        { text = "Stem på din favoritt" }
        { text = "Send oss din mening" }
        { text = "Jens Stoltenberg er i studio" }
        { text = "Kommunevalget" }
    |]
let radioPrograms =
    Map([
        P1, [|
            { title = "Reiseradion" }
            { title = "Dagsnytt" }
        |]
        P2, [|
            { title = "Nyhetsmorgen" }
            { title = "Feriestemning" }
        |]
        P3, [|
            { title = "P3Musikk" }
            { title = "Hør'a!" }
            { title = "P3Morgen" }
        |]
        P13, [|
            { title = "Felbergs loft" }
            { title = "Transmission" }
            { title = "Gitar" }
        |]
    ])
    

type DummyDataService() =
    
    let generateIndexPoint (channel: RadioChannel) =
        {
            startTime = DateTimeOffset.Now
            radioProgram = radioPrograms[channel] |> Rand.arrayItem
            radioChannel = channel
            nowPlayingItem = Rand.fiftyFifty (fun () -> songs |> Rand.arrayItem |> Music) (fun () -> editorialItems |> Rand.arrayItem |> Editorial)
        }
    
    let mutable state: Map<RadioChannel, IndexPoint> =
        Map([
            for c in RadioChannel.channels do
                yield c,generateIndexPoint c
        ])
        
    member _.NowPlaying () = state
    
    member _.GenerateIndexPoint (channel: RadioChannel) =
        let item = generateIndexPoint channel
        state <- state |> Map.add channel item
        item

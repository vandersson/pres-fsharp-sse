module ServerSentEventDemo.NowPlaying

open System
open ServerSentEventDemo.Channels

type MusicItem =
    {
        songTitle: string
        artist: string
    }
    
type EditorialItem =
    {
        text: string
    }
    
type NowPlayingItem =
    | Music of MusicItem
    | Editorial of EditorialItem

type RadioProgram =
    {
        title: string
    }

type IndexPoint =
    {
        startTime: DateTimeOffset
        radioProgram: RadioProgram
        radioChannel: RadioChannel
        nowPlayingItem: NowPlayingItem
    }


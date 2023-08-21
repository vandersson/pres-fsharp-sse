

function nowPlayingItem(channelNowPlaying) {
    switch (channelNowPlaying.nowPlayingItem.type) {
        case "music":
            return p({}, 
                [ img("music.svg", "Music element")
                , text(channelNowPlaying.nowPlayingItem.artist)
                , text (" – ")
                , i({}, [text(channelNowPlaying.nowPlayingItem.songTitle)])
                ]);    
        case "editorial":
            return p({},
                [ img("editorial.svg", "Editorial element")
                    , text(channelNowPlaying.nowPlayingItem.text)
                ]);
    }
}

function channelCard(channelNowPlaying) {
    return article({id: "channel-"+channelNowPlaying.radioChannel}, 
        [ h2(channelNowPlaying.radioChannel.toUpperCase())
        , b({},[text(channelNowPlaying.radioProgram.title)])
        , nowPlayingItem(channelNowPlaying) 
        ]
    )
}

function handleNowPlayngEvent(event) {
    let channelListElem = document.getElementById("channel-list");
    let msg = JSON.parse(event.data);
    
        
    
    let newElem = channelCard(msg);
    let existingElem = document.getElementById("channel-"+msg.radioChannel);
    if (existingElem) {
        existingElem.replaceWith(newElem);
    } else {
        channelListElem.appendChild(newElem);
    }
    setTimeout(() => newElem.classList.add("highlighted"), 1000);
    console.log(msg);
}

const eventSource = new EventSource("/nowplaying");

eventSource.addEventListener("nowplaying", handleNowPlayngEvent);



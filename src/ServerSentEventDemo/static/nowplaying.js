

function nowPlayingItem(channelNowPlaying) {
    switch (channelNowPlaying.nowPlayingItem.type) {
        case "music":
            return div({}, 
                [ img("music.svg", "Music element")
                , text(channelNowPlaying.nowPlayingItem.artist)
                , text (" – ")
                , i({}, [text(channelNowPlaying.nowPlayingItem.songTitle)])
                ]);    
        case "editorial":
            return div({},
                [ img("editorial.svg", "Editorial element")
                    , text(channelNowPlaying.nowPlayingItem.text)
                ]);
    }
}

function channelCard(channelNowPlaying) {
    return article({id: "channel-"+channelNowPlaying.radioChannel}, 
        [ div({}, [h2(channelNowPlaying.radioChannel.toUpperCase())])
        , div({class: "nowplaying"}, 
            [ b({},[text(channelNowPlaying.radioProgram.title)])
            , nowPlayingItem(channelNowPlaying) 
            ])            
        ]
    )
}

function handleNowPlayingEvent(event) {
    let msg = JSON.parse(event.data);

    let newElem = channelCard(msg);
    let existingElem = document.getElementById("channel-"+msg.radioChannel);
    if (existingElem) {
        existingElem.replaceWith(newElem);
    } else {
        let channelListElem = document.getElementById("channel-list");
        channelListElem.appendChild(newElem);
    }
    setTimeout(() => newElem.classList.add("highlighted"), 1000);
    console.log(msg);
}



const eventSource = new EventSource("/nowplaying");

eventSource.addEventListener("nowplaying", handleNowPlayingEvent);

eventSource.onerror = (event) => console.error("Event Source error: ", event)

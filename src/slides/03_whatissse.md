# What are Server-Sent Event?

- _Reverses_ the HTTP message flow -> send messages from the server to the client
- Defined by its own content-type: text/event-stream

## Simple mechanism 
- Client opens HTTP connection and keeps it open, reading new data as it arrives
- Server sends _messages_ delimited by 2 newline characters `\n\n` 


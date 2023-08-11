# Content-Type

text/event-stream

# Content format

``` 
field: value\n
\n
```

| fields | desc                                                                                                                                | Required |
|--------|-------------------------------------------------------------------------------------------------------------------------------------|----------|
| event  | Event type. Used by browser to select event listener                                                                                | No       |
| data   | Event payload. For example json. Event may contain multiple data fields. Client with concatenate the values with a newline between  | Yes      |
| id     | Event id                                                                                                                            | No       |
| retry  | Client wait milliseconds before retry on lost connection. Positive integer                                                          | No       |

events are separated by a newline 

# Example

```
event: nowplaying
data: wowow
id: 120921e7-2707-471f-9862-1fd154398692
retry: 500

event: nowplaying
data: wowow
id: 82a3e08a-8ac4-4aa5-8808-83fabe241256
retry: 500

```

# Connecting to the server

## Javascript
```js
const eventSource = new EventSource("<host uri>");

eventSource.addEventListener("<event type>", (event) => console.log(event));
```

## F#
Http client and parse result

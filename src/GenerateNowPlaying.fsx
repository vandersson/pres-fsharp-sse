
open System.Net.Http

let httpClient = new HttpClient()
    
let generateForChannel c =
    let t =
        task {
            let! r = httpClient.PostAsync ($"http://localhost:3000/nowplaying/generate/%s{c}", new StringContent(""))
            let! content = r.Content.ReadAsStringAsync()
            if r.IsSuccessStatusCode then
                return content
            else
                return failwith $"Error: %O{r.StatusCode} - {content}"
        }
    t.Wait()
    t.Result
    

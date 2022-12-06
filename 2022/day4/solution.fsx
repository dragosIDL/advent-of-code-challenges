open System
open System.IO

let inputPath =
    "C:\\Users\\dragos\\source\\dragosIDL\\advent-of-code-challenges\\2022\\day4\\input.txt"

let input = File.ReadAllLines inputPath

let data =
    input
    |> Array.map (fun s ->
        s.Split(',')
        |> Array.map (fun x -> x.Split('-') |> fun x -> (int x[0], int x[1]))
        |> fun x -> (x[0], x[1]))

let part1 =
    data
    |> Array.filter (fun ((al, ah), (bl, bh)) -> (al <= bl && bh <= ah) || (bl <= al && ah <= bh))
    |> Array.length

let part2 =
    data
    |> Array.filter (fun ((al, ah), (bl, bh)) -> (ah >= bl && ah <= bh) || (bh >= al && bh <= ah))
    |> Array.length

let part2m =
    data
    |> Array.filter (fun ((al, ah), (bl, bh)) -> ah >= bl && bh >= al)
    |> Array.length

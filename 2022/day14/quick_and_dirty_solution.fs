open System
open System.IO

let inputPath = "2022\\day14\\input.txt"

let input = File.ReadAllLines inputPath

let generateRockPositions (line: string array) =
    line
    |> Array.map (fun l -> l.Split(",") |> fun a -> (int a[0], int a[1]))
    |> Array.windowed 2
    |> Array.map (fun a -> (a[0], a[1]))
    |> Array.map (fun ((sy, sx), (ey, ex)) ->
        if sx = ex then
            let (s, e) = if sy > ey then (ey, sy) else (sy, ey)

            [ s..e ]
            |> Array.ofList
            |> Array.map (fun l -> (sx, l))
        else
            let (s, e) = if sx > ex then (ex, sx) else (sx, ex)

            [ s..e ]
            |> Array.ofList
            |> Array.map (fun l -> (l, sy)))

let rocks =
    input
    |> Array.map (fun l -> l.Split(" -> "))
    |> Array.map generateRockPositions
    |> Array.collect id
    |> Array.collect id
    |> Array.distinct

let map =
    [ 0..154 ]
    |> List.map (fun l -> [| for _ in [0 .. 1000] do yield '.'|])
    |> fun x -> x @  [([| for _ in [0 .. 1000] do yield '#'|])]
    |> Array.ofList

map[0][500] <- '+'

rocks
|> Array.iter (fun (x, y) -> map[x][y] <- '#')

let printMap (m: char array array) =
    m
    |> Array.map (fun x -> String(x |> Array.skip 300))
    |> fun x -> String.Join("\n", x)

File.WriteAllText("2022\\day14\\map.txt", printMap map)


let initialPosition = (0, 500)
let mutable currentPosition = initialPosition

for i in [1..2_371_600 ] do

    let x, y = currentPosition
    let nextPositions = 
        [ 0; -1; 1 ] 
        |> List.map (fun a -> (x + 1, y + a))
        |> List.filter (fun (cy, cx) -> cy >= 0 && cy <= map.Length && cx >= 0 && cx <= map[0].Length)

    let q = nextPositions |> List.filter (fun (cy, cx) -> map[cy][cx] = '.')

    currentPosition <- 
        match q with
        | [] -> 
            initialPosition
        | (cx, cy) :: tail->
            map[cx][cy] <- 'o'
            map[x][y] <- '.'
            (cx, cy)
        | _ -> failwith "how?"


File.WriteAllText("2022\\day14\\map.txt", printMap map)


let ans = 
    map 
    |> Array.collect id 
    |> Array.filter (fun a -> a = 'o') 
    |> Array.length 
    |> (fun a -> a + 1)

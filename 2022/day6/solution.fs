open System
open System.IO

let inputPath =
    "C:\\Users\\dragos\\source\\dragosIDL\\advent-of-code-challenges\\2022\\day6\\input.txt"

let input = File.ReadAllText inputPath

let part1 = 
    input.ToCharArray()
    |> Array.windowed 4
    |> Array.map (Array.distinct >> Array.length)
    |> Array.findIndex (fun x -> x = 4) 
    |> fun el -> el + 4

let part2 = 
    input.ToCharArray()
    |> Array.windowed 14
    |> Array.map (Array.distinct >> Array.length)
    |> Array.findIndex (fun x -> x = 14) 
    |> fun el -> el + 14
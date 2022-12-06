open System.IO
open System

let inputPath =
    "C:\\Users\\dragos\\source\\dragosIDL\\advent-of-code-challenges\\2021\\day1\\input.txt"

let input = File.ReadAllLines inputPath |> Array.map int

let part1 =
    input
    |> Array.windowed 2
    |> Array.map (Array.reduce (-))
    |> Array.filter ((>) 0)
    |> Array.length

let part2 =
    input
    |> Array.windowed 3
    |> Array.map Array.sum
    |> Array.windowed 2
    |> Array.map (Array.reduce (-))
    |> Array.filter ((>) 0)
    |> Array.length

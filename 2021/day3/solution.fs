open System.IO
open System

let inputPath =
    "C:\\Users\\dragos\\source\\dragosIDL\\advent-of-code-challenges\\2021\\day3\\input.txt"

let input = File.ReadAllLines inputPath

let mostCommon rs = 
    let a = rs |> Array.filter ((=) '0') |> Array.length
    let b = rs |> Array.filter ((=) '1') |> Array.length
    if a > b then '0' else '1'

let part1 =
    input
    |> Array.map (fun x -> x.ToCharArray())
    |> Array.transpose
    |> Array.map mostCommon
    |> fun s ->
        [| s
           s
           |> Array.map (fun a -> if a = '1' then '0' else '1') |]
    |> Array.map String
    |> Array.map (fun x -> Convert.ToInt32(x, 2))
    |> Array.reduce (*)


let part2 = 
    input
    |> Array.map (fun x -> x.ToCharArray())
    |> Array.map mostCommon
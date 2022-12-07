open System
open System.IO

let inputPath = "2022\\day3\\input.txt"

let input = File.ReadAllLines inputPath

let compute (c: char) =
    if (Char.IsLower c) then
        int c - int 'a' + 1
    else
        int c - int 'A' + 27

let part1 =
    input
    |> Array.map (fun x -> x.Trim())
    |> Array.map (fun str ->
        [ str[.. (str.Length / 2) - 1]
          str[(str.Length / 2) ..] ])
    |> Array.map (List.map set >> Set.intersectMany >> Seq.head)
    |> Array.sumBy compute

let part2 =
    input
    |> Array.map (fun x -> x.Trim())
    |> Array.chunkBySize 3
    |> Array.map (Array.map set >> Set.intersectMany >> Seq.head)
    |> Array.sumBy compute

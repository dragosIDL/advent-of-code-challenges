open System
open System.IO
open System.Collections.Generic

let inputPath = "2022\\day10\\input.txt"

type Op =
    | Addx of int
    | Noop

let parse (s: string) =
    let a = s.Split(" ")

    match a[0] with
    | "addx" -> Addx(int a[1])
    | "noop" -> Noop
    | _ -> failwith "not a valid operation"

let input = File.ReadAllLines inputPath |> Array.map parse

let (cycles, _, _) =
    input
    |> Array.fold
        (fun (s: (int * int * int) list, x: int, cycleNr: int) op ->
            match op with
            | Noop -> (s @ [ (cycleNr, x, x) ], x, cycleNr + 1)
            | Addx a -> (s @ [ (cycleNr, x, x); (cycleNr + 1, x, x + a) ], x + a, cycleNr + 2))
        ([], 1, 1)

let part1 =
    cycles
    |> fun a ->
        seq { 20..40..220 }
        |> Seq.map (fun index -> a[index - 1])
        |> Seq.toList
    |> List.map (fun (cycle, during, _) -> cycle * during)
    |> List.sum

let convert i x =
    [ -1; 0; 1 ]
    |> List.map ((+) x)
    |> List.contains i
    |> fun b -> if b then "#" else "."

let strJoin (s: string list) = String.Join("", s)

let part2 =
    cycles
    |> List.map (fun (_, _, x) -> x)
    |> fun a -> 0 :: a
    |> List.chunkBySize 40
    |> List.map (List.mapi convert >> strJoin)

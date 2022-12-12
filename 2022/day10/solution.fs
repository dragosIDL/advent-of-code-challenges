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

let folder (s: (int * int * int) list, x: int, cycleNr: int) (op: Op) =
    match op with
    | Noop -> (s @ [ (cycleNr, x, x) ], x, cycleNr + 1)
    | Addx a -> (s @ [ (cycleNr, x, x) (cycleNr + 1, x, x + a) ], x + a, cycleNr + 2)

let cycles =
    input
    |> Array.fold folder ([], 1, 1)
    |> fun (a, _, _) -> a

let part1 =
    cycles
    |> fun a ->
        seq { 20..40..220 }
        |> Seq.map (fun index -> a[index - 1])
        |> Seq.toList
        |> List.map (fun (cycle, during, _) -> cycle * during)
        |> List.sum

let draw i x =
    [ x - 1; x; x + 1 ]
    |> List.contains i
    |> function
        | true -> '#'
        | false -> '.'

let part2 =
    cycles
    |> List.map (fun (_, _, x) -> x)
    |> fun a ->
        0 :: a
        |> List.chunkBySize 40
        |> List.map (List.mapi draw >> List.toArray >> System.String)

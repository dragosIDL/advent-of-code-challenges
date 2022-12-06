open System.IO
open System

let inputPath =
    "C:\\Users\\dragos\\source\\dragosIDL\\advent-of-code-challenges\\2021\\day2\\input.txt"

let input = File.ReadAllLines inputPath

let part1 =
    input
    |> Array.map (fun s -> s.Split(" ") |> fun a -> (a[0], int a[1]))
    |> Array.fold
        (fun (forward, depth) (dir, amount) ->
            match dir with
            | "forward" -> (forward + amount, depth)
            | "down" -> (forward, depth + amount)
            | "up" -> (forward, depth - amount))
        (0, 0)
    |> fun (forward, depth) -> forward * depth

let part2 =
    input
    |> Array.map (fun s -> s.Split(" ") |> fun a -> (a[0], int a[1]))
    |> Array.fold
        (fun (forward, depth, aim) (dir, amount) ->
            match dir with
            | "forward" -> (forward + amount, depth + (aim * amount), aim)
            | "down" -> (forward, depth, aim + amount)
            | "up" -> (forward, depth, aim - amount))
        (0, 0, 0)
    |> fun (forward, depth, z) -> forward * depth

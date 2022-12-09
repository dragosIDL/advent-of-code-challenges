open System
open System.IO
open System.Collections.Generic

let inputPath = "2022\\day9\\input.txt"

type pos = int * int

let directions =
    File.ReadAllLines inputPath
    |> Array.map (fun x -> x.Split(" "))
    |> Array.collect (fun (x: string array) -> [| 1 .. int x[1] |] |> Array.map (fun _ -> x[0]))
    |> Array.toList

let isTouching (head: int * int) (tx, ty) =

    let neighbors i j = (tx + i, ty + j)

    [ -1; 0; 1 ] 
    |> List.collect (fun i -> List.map (neighbors i) [ -1; 0; 1 ] )
    |> List.contains head

let move (directions) (snake: pos list) =

    let moveTowards (hx, hy) (tx, ty) =
        let (dx, dy) = (hx - tx), (hy - ty)
        (tx + sign dx, ty + sign dy)

    let moveSnake (s: pos list, lastVisited: pos list) direction =
        let (hx, hy) = s.Head

        let newHeadPosition =
            match direction with
            | "U" -> (hx + 1, hy)
            | "D" -> (hx - 1, hy)
            | "L" -> (hx, hy - 1)
            | "R" -> (hx, hy + 1)

        let newSnake =
            s.Tail
            |> List.fold
                (fun (newSnake, previous) next ->

                    if isTouching previous next then
                        newSnake @ [ next ], next
                    else
                        let next = moveTowards previous next
                        newSnake @ [ next ], next)
                ([ newHeadPosition ], newHeadPosition)
            |> fst

        (newSnake, (newSnake |> List.last) :: lastVisited)

    directions
    |> List.fold moveSnake (snake, [])
    |> snd

let part1 =
    move directions (List.replicate 2 (0, 0))
    |> List.distinct
    |> List.length

let part2 =
    move directions (List.replicate 10 (0, 0))
    |> List.distinct
    |> List.length

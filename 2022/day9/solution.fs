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

let isTouching head tail =
    let isOverlapping (hx, hy) (tx, ty) = hx = tx && hy = ty

    let aroundPos (tx, ty) =
        [ (tx + 1, ty - 1)
          (tx + 1, ty)
          (tx + 1, ty + 1)
          (tx, ty - 1)
          (tx, ty + 1)
          (tx - 1, ty - 1)
          (tx - 1, ty)
          (tx - 1, ty + 1) ]

    isOverlapping head tail
    || aroundPos tail |> List.contains head

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
                (fun (newSnake, previous) ((tx, ty): pos) ->

                    if isTouching previous (tx, ty) then
                        (newSnake @ [ (tx, ty) ], (tx, ty))
                    else
                        let (tx, ty) = moveTowards previous (tx, ty)
                        (newSnake @ [ (tx, ty) ], (tx, ty)))

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

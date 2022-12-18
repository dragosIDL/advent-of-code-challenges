open System
open System.IO
open System.Collections.Generic
open System.Linq

let inputPath = "2022\\day18\\input.txt"
let outputPath = "2022\\day18\\output.txt"

let input =
    File.ReadAllLines inputPath
    |> Array.map (fun el ->
        el.Split(",")
        |> Array.map (fun e -> int e)
        |> fun a -> (a[0], a[1], a[2]))


let faces (x, y, z) =
    seq {
        // yield (x, y, z)
        yield (x - 1, y, z)
        yield (x + 1, y, z)
        yield (x, y - 1, z)
        yield (x, y + 1, z)
        yield (x, y, z - 1)
        yield (x, y, z + 1)
    }


let (a, b, c) =
    (Array.map (fun (a, _, _) -> a) input |> Array.max,
     Array.map (fun (_, b, _) -> b) input |> Array.max,
     Array.map (fun (_, _, c) -> c) input |> Array.max)

let outbound (x, y, z) = x > a || y > b || z > c

let isAirPocket (cubes: Set<(int * int * int)>) position =


    // let rec isSurounded (c: Set<int * int * int>) positions =
    //     // printfn "AASHTA"

    //     match positions with
    //     | [] -> true
    //     | head :: _ when outbound head -> false
    //     | head :: tail ->
    //         let emptyNeighbors =
    //             faces head
    //             |> Seq.filter (fun f -> not (c.Contains f))
    //             |> Seq.toList

    //         let newSet = c.Add head
    //         isSurounded newSet (emptyNeighbors @ tail)

    // isSurounded cubes [ position ]

    faces position |> Seq.forall (cubes.Contains)

let part1 =
    input
    |> Array.toSeq
    |> Seq.map faces
    |> Seq.collect id
    |> Seq.filter (fun k -> Array.contains k input |> not)
    |> Seq.toList
    |> Seq.length

let cubesSet = input |> set

let part2 =
    input
    |> Array.toSeq
    |> Seq.map faces
    |> Seq.collect id
    |> Seq.filter (fun face -> Array.contains face input |> not)
    |> Seq.filter (fun face -> not (isAirPocket cubesSet face))
    |> Seq.toList
    |> Seq.length




// let a =
//     input
//     |> Array.toSeq
//     |> Seq.map matching
//     |> Seq.collect id
//     // |> Seq.map (fun a -> a |> Seq.toList)
//     |> Seq.groupBy (fun a -> a)
//     |> Seq.map (fun (k, el) -> (k, el |> Seq.length))
//     // |> Seq.map (fun (k, el) -> (k, el |> Seq.toList ))
//     // // |> Seq.filter( fun (k, el ) -> el < 6 )
//     |> Seq.map (fun (k, el) -> (k, 6 - el))
//     |> Seq.filter (fun (k, el) -> input |> Array.contains k)
//     |> Seq.toList
//     // |> Seq.map (fun (k, el) -> el)
//     // |> Seq.sum
//     // |> Seq.length

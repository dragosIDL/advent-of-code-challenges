open System
open System.IO

let mapAsInt = Array.map Int32.Parse

let input =
    File.ReadAllLines "2017/day6/input.txt"
    |> Array.head
    |> fun s -> s.Split '\t'
    |> mapAsInt

let reallocation arr =

    let rec realloc arr (states: Map<int list, int>) step =
        
        let larr = Array.toList arr
        if states.ContainsKey larr then
            (step, step - states.[larr])
        else
            let newMap = states.Add (larr, step)

            let pos, v =
                arr
                |> Array.mapi (fun i v -> i, v)
                |> Array.maxBy snd
            
            arr.[pos] <- 0
            [ 1 .. v ]
            |> List.map (fun id -> (pos + id) % arr.Length)
            |> List.iter (fun index -> arr.[index] <- arr.[index] + 1)

            realloc arr newMap (step + 1)

    realloc arr Map.empty<int list, int> 0

// solution to part two is to record the step at which every state of the array happens
// then if we find a previous state in the map we get the step at which it first happened
// and then return current step - recorded step

let (partOne, partTwo) = reallocation input // 7864, 1695


open System
open System.IO

let inputPath = "2022\\day11\\input.txt"

let input = File.ReadAllLines inputPath

let parseInput = File.ReadAllText inputPath

type Monkey =
    { Items: double list
      Operation: double -> double
      Test: double -> int
      Modulo: double
      Inspections: int }

let initMonkeys () =
    [| { Items = [ 54; 61; 97; 63; 74 ]
         Operation = (fun x -> x * 7.0)
         Test = (fun x -> if x % 17.0 = 0.0 then 5 else 3)
         Modulo = 17.0
         Inspections = 0 }
       { Items = [ 61; 70; 97; 64; 99; 83; 52; 87 ]
         Operation = (fun x -> x + 8.0)
         Test = (fun x -> if x % 2.0 = 0.0 then 7 else 6)
         Modulo = 2.0
         Inspections = 0 }
       { Items = [ 60; 67; 80; 65 ]
         Operation = (fun x -> x * 13.0)
         Test = (fun x -> if x % 5.0 = 0.0 then 1 else 6)
         Modulo = 5.0
         Inspections = 0 }
       { Items = [ 61; 70; 76; 69; 82; 56 ]
         Operation = (fun x -> x + 7.0)
         Test = (fun x -> if x % 3.0 = 0.0 then 5 else 2)
         Modulo = 3.0
         Inspections = 0 }
       { Items = [ 79; 98 ]
         Operation = (fun x -> x + 2.0)
         Test = (fun x -> if x % 7.0 = 0.0 then 0 else 3)
         Modulo = 7.0
         Inspections = 0 }
       { Items = [ 72; 79; 55 ]
         Operation = (fun x -> x + 1.0)
         Test = (fun x -> if x % 13.0 = 0.0 then 2 else 1)
         Modulo = 13.0
         Inspections = 0 }
       { Items = [ 63 ]
         Operation = (fun x -> x + 4.0)
         Test = (fun x -> if x % 19.0 = 0.0 then 7 else 4)
         Modulo = 19.0
         Inspections = 0 }
       { Items = [ 72; 51; 93; 63; 80; 86; 81 ]
         Operation = (fun x -> x * x)
         Test = (fun x -> if x % 11.0 = 0.0 then 0 else 4)
         Modulo = 11.0
         Inspections = 0 } |]


let play monkeys n =
    
    let mutable monkeyArr = monkeys

    let divs =  monkeys |> Array.fold (fun s m -> s * m.Modulo) 1.0
    
    [ 1 .. n ]
    |> List.iter (fun _ ->
        monkeyArr
        |> Array.iteri (fun i monkey ->

            monkey.Items
            |> List.iteri (fun index item ->

                let newWorryLevel = monkey.Operation item
                let actualWorryLevel = newWorryLevel % divs

                let throwTo = monkey.Test actualWorryLevel

                monkeyArr[throwTo] <- { monkeyArr[throwTo] with Items = monkeyArr[throwTo].Items @ [ (actualWorryLevel) ] })

            monkeyArr[i] <- { monkey with Items = []; Inspections = monkey.Inspections + monkey.Items.Length }))

    monkeyArr
    |> Array.sortByDescending (fun a -> a.Inspections)
    |> Array.take 2
    |> Array.fold (fun s a -> s * (decimal a.Inspections)) (1m)

let part1 = play (initMonkeys()) 20
let part2 = play (initMonkeys()) 10_000

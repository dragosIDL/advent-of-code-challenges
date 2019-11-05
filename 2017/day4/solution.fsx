open System.IO

let input() = 
    File.ReadAllLines "2017/day4/input.txt"

let isSeen (set:string Set) el = 
    if set.Contains el then (set, true)
    else (set.Add el, false)

let isValidPassphrase (phrase:string) = 
    let mutable holdingSet = Set.empty<string>

    phrase.Split ' '
    |> Seq.map (fun el -> 
        let (set, v) = isSeen holdingSet el
        holdingSet <- set
        v)
    |> Seq.exists id
    |> not

let partOne = 
    input ()
    |> Array.filter isValidPassphrase
    |> Array.length

let rec distribute e = function
  | [] -> [[e]]
  | x::xs' as xs -> (e::xs)::[for xs in distribute e xs' -> x::xs]

let rec permute = function
  | [] -> [[]]
  | e::xs -> List.collect (distribute e) (permute xs)

let generatePermutations (str:string) =
    str.ToCharArray() 
    |> Array.toList
    |> permute
    |> List.distinct
    |> List.map (List.toArray >> System.String)

let isValidPermutationPassphrase (phrase:string) = 
    let mutable holdingSet = Set.empty<string>

    phrase.Split ' '
    |> Array.map generatePermutations
    |> Seq.collect id
    |> Seq.map (fun el -> 
        let (set, v) = isSeen holdingSet el
        holdingSet <- set
        v)
    |> Seq.exists id
    |> not

let partTwo = 
    input ()
    |> Array.filter isValidPermutationPassphrase
    |> Array.length

// 208
// a bit slow some optimization is in order
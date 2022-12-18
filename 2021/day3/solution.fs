open System
open System.IO

let inputPath = "2021\\day3\\input.txt"

let input = File.ReadAllLines inputPath

let mostCommon rs =
    let a = rs |> Array.filter ((=) '0') |> Array.length
    let b = rs |> Array.filter ((=) '1') |> Array.length
    if a > b then '0' else '1'

let interpretNumber (s: char array) =
    [| s
       s
       |> Array.map (fun a -> if a = '1' then '0' else '1') |]

let chars (s: string) = s.ToCharArray()
let toInt (s: string) = Convert.ToInt32(s, 2)

let mostCommonNrs =
    input
    |> Array.map chars
    |> Array.transpose
    |> Array.map mostCommon

let part1 =
    mostCommonNrs
    |> interpretNumber
    |> Array.map String
    |> Array.map toInt
    |> Array.reduce (*)

let qq = input |> Array.map chars |> Array.map String

let part2 = 
    qq 
    |> Array.fold 
        (fun s x -> 
            let r = 
                mostCommonNrs 
                |> Array.zip (x.ToCharArray())
                |> Array.forall (fun (c1,c2) -> c1 = c2)
            if r then x :: s else s
        ) []
    // |> List.map toInt
// |> Array.transpose

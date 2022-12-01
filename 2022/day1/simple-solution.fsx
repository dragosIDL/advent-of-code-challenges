open System.IO
open System

let inputPath =
    "C:\\Users\\dragos\\source\\dragosIDL\\advent-of-code-challenges\\2022\\day1\\input.txt"

let input = File.ReadAllText inputPath

let repeat s : string = s + s

let caloriesPerElf (input:string) = 
    input.Split (repeat Environment.NewLine)
    |> Array.map (fun x -> x.Split Environment.NewLine)
    |> Array.map (fun xs -> Array.map (fun x -> (int) x) xs |> Array.sum)

input 
|> caloriesPerElf
|> Array.max
|> printfn "The elf with the most calories %A"

input
|> caloriesPerElf
|> Array.sortDescending
|> Array.take 3
|> Array.sum
|> printfn "Top 3 elfs with the most calories %A"

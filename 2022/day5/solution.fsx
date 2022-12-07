open System
open System.IO

let inputPath = "2022\\day5\\input.txt"

let input = File.ReadAllLines inputPath

let splitInput =
    input
    |> Array.splitAt 8
    |> fun (a, b) -> (a, b |> Array.skip 2)

let inputMap =
    splitInput
    |> fst
    |> Array.map (fun x ->
        x.ToCharArray()
        |> Array.chunkBySize 4
        |> Array.map (fun x ->
            String x
            |> fun x -> x.Replace("[", "").Replace("]", ""))
        |> Array.map (fun x -> x.Trim()))
    |> Array.transpose
    |> Array.map (Array.filter (fun x -> x <> ""))

let inputMoves =
    splitInput
    |> snd
    |> Array.map (fun x ->
        x
            .Replace("move", "")
            .Replace("from", "")
            .Replace("to", "")
            .Split("  "))
    |> Array.map (fun x -> (int x[0], int x[1] - 1, int x[2] - 1))

let charsToStr (s: string array) = String.Join("", s)

let replaceAt index arr =
    Array.removeAt index >> Array.insertAt index arr

let part1 =

    let part1Folder (state: string array array) (amount, fromIndex, toIndex) =

        let (toHead, fromStack) =
            state[fromIndex]
            |> Array.splitAt amount
            |> fun (a, b) -> Array.rev a, b

        let toStack = Array.append toHead state[toIndex]

        state
        |> replaceAt fromIndex fromStack
        |> replaceAt toIndex toStack

    inputMoves
    |> Array.fold part1Folder inputMap
    |> Array.map (fun x -> x[0])
    |> charsToStr

let part2 =

    let part2Folder (state: string array array) (amount, fromIndex, toIndex) =

        let (toHead, fromStack) = state[fromIndex] |> Array.splitAt amount
        let toStack = Array.append toHead state[toIndex]

        state
        |> replaceAt fromIndex fromStack
        |> replaceAt toIndex toStack

    inputMoves
    |> Array.fold part2Folder inputMap
    |> Array.map (fun x -> x[0])
    |> charsToStr

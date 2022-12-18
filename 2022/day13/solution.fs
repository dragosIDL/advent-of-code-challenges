open System
open System.IO
open System.Text.Json
open System.Collections.Generic

let inputPath = "2022\\day13\\input.txt"

let input = File.ReadAllText inputPath

let pairs =
    input
    |> fun s -> s.Split("\r\n\r\n")
    |> Array.map (fun a -> a.Split("\r\n") |> fun x -> (x[0], x[1]))

type Kind =
    | Int of int
    | Ls of Kind list

let chars (s: string) = s.ToCharArray()

let (|Digit|Else|) (d: char) =
    if Char.IsDigit d then
        Digit(int d - 48)
    else
        Else

let parser (l: string) =

    let rec parseRec rest (accum: Kind list) =
        match rest with
        | [] -> (List.rev accum, [])
        | '[' :: rest ->
            let parsed, rest = parseRec rest []
            parseRec rest (Ls parsed :: accum)
        | ']' :: rest -> List.rev accum, rest
        | ',' :: rest -> parseRec rest accum
        | Digit x :: Digit y :: rest -> parseRec rest (Int(10 * x + y) :: accum)
        | Digit x :: rest -> parseRec rest (Int x :: accum)
        | _ -> failwith "Invalid input"

    parseRec (chars l |> List.ofArray) []
    |> fst
    |> List.head

let rec matcher x y =
    match x, y with
    | Int a, Int b -> a.CompareTo b
    | Ls a, Int b -> matcher x (Ls [ Int b ])
    | Int a, Ls b -> matcher (Ls [ Int a ]) y
    | Ls a, Ls b ->

        let smallerLength = min (List.length a) (List.length b)

        List.zip (List.take smallerLength a) (List.take smallerLength b)
        |> List.map (fun (x, y) -> matcher x y)
        |> List.skipWhile (fun c -> c = 0)
        |> List.tryHead
        |> function
            | Some c -> c
            | None -> (List.length a).CompareTo(List.length b)

    | _ -> failwith "invalid input"


let part1 =
    pairs
    |> Array.map (fun (a, b) -> parser a, parser b)
    |> Array.mapi (fun i (a, b) -> (i + 1, matcher a b))
    |> Array.filter (fun (_, c) -> c < 0)
    |> Array.sumBy fst


let dividers =
    [| Ls [ Ls [ Int 2 ] ]
       Ls [ Ls [ Int 6 ] ] |]

let part2 =
    pairs
    |> Array.collect (fun (a, b) -> [| parser a; parser b |])
    |> Array.append dividers
    |> Array.sortWith matcher
    |> fun a ->
        dividers
        |> Array.map (fun d -> Array.IndexOf(a, d) + 1)
    |> Array.reduce (*)

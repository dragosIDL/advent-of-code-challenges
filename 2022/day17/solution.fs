open System
open System.IO
open System.Collections.Generic
open System.Linq

let inputPath = "2022\\day17\\input.txt"
let outputPath = "2022\\day17\\output.txt"

let input = File.ReadAllText inputPath

type Dir =
    | Left
    | Right

type RockType =
    | HorizontalLine of char list list
    | PlusShape of char list list
    | LShape of char list list
    | VerticalLine of char list list
    | Square of char list list

let parse =
    function
    | '>' -> Right
    | '<' -> Left

let moves = input.ToCharArray() |> Array.map parse

let charList (s: string) = s.ToCharArray() |> Array.toList

let rockTypes =
    [| HorizontalLine([ (charList "@@@@") ])
       PlusShape(
           [ (charList ".@.")
             (charList "@@@")
             (charList ".@.") ]
       )
       LShape(
           [ (charList "..@")
             (charList "..@")
             (charList "@@@") ]
       )
       VerticalLine(List.replicate 4 ([ '@' ]))
       Square([ (charList "@@"); (charList "@@") ]) |]

let rockTypeCycle = Seq.initInfinite (fun i -> rockTypes[i % rockTypes.Length])
let movesCycle = Seq.initInfinite (fun i -> moves[i % moves.Length])


let produce rocks =


    let mutable ls: char list list = []

    let emptyLs () = List.replicate 7 '.'

    let inPosition (rock: char list list) =

        rock
        |> List.map (fun l ->
            let newL = (charList "..") @ l

            let right =
                [ 0 .. (6 - newL.Length) ]
                |> List.map (fun _ -> '.')

            newL @ right)


    let playRock (moves: Dir seq) (rock: RockType) (matrix: char list list) = 


        let spawnedRock = 
            match rock with
            | HorizontalLine x -> (inPosition x) @ matrix
            | PlusShape x -> (inPosition x) @ matrix
            | LShape x -> (inPosition x) @ matrix
            | VerticalLine x -> (inPosition x) @ matrix
            | Square x -> (inPosition x) @ matrix

        

        


    for rock in rocks do

        let tempLs  =  emptyLs () :: emptyLs () :: emptyLs () :: ls

        let isStuck = false



        let updated = 
            match rock with
            | HorizontalLine x -> (inPosition x) @ tempLs
            | PlusShape x -> (inPosition x) @ tempLs
            | LShape x -> (inPosition x) @ tempLs
            | VerticalLine x -> (inPosition x) @ tempLs
            | Square x -> (inPosition x) @ tempLs

        
        ls <- updated

    ls

let part1 = produce (rockTypeCycle |> Seq.take 2022)

part1 |> List.length


let str =
    part1
    |> List.map (fun l -> new String(l |> List.toArray))

let strW = String.Join("\n", str)

File.WriteAllText(outputPath, strW)

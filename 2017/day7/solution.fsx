open System
open System.IO

#r "../../packages/FSharp.Text.RegexProvider.dll"

let input = File.ReadAllLines "2017/day7/input.txt"

type Data =
    { Name: string
      Weight: int }

type Row =
    { Data: Data
      Children: string [] }

open FSharp.Text.RegexProvider

type Extractor = Regex< @"(\w+) \((\d+)\)(.-> )?([\w, ]+)?" >

let readData (str: string) =

    let result = Extractor().TypedMatch str

    let name = (result.Groups.Item 1).Value
    let weight = Int32.Parse (result.Groups.Item 2).Value
    let children = 
        let ch = result.Groups.Item 4 in 
        if ch.Success then ch.Value.Split ([|", "|],StringSplitOptions.RemoveEmptyEntries)
        else [||]

    {
        Data = { Name = name; Weight = weight };
        Children = children
    }

let rows() = input |> Array.map readData

let folder (lfp,lfk) el =

    let lfp, lfk =
        if lfk |> Set.contains el.Data.Name
        then lfp, lfk|> Set.remove el.Data.Name
        else lfp |> Set.add el.Data.Name, lfk

    el.Children 
    |> Array.fold (fun (lfp, lfk) e -> 
        if lfp |> Set.contains e
        then lfp |> Set.remove e, lfk
        else lfp, lfk|> Set.add e) (lfp,lfk)

let partOne = 
    rows()
    |> Array.fold folder (Set.empty<string>,Set.empty<string>)
    |> fun (lfp,_) -> lfp |> Set.toSeq |> Seq.head


// Part two
type T = 
    { Data: Data
      Kids: T list }

let toTree (row:Row) =
    { Data=row.Data; Kids=[] }

let treePairs (l:Row []) : Map<string, Row * T> =
    l 
    |> Array.map (fun row ->(row.Data.Name, (row, row |> toTree)))
    |> Map.ofArray<string, Row * T>

let rec buildTree start (map: Map<string, Row * T>) = 

    let (row, tree) = map.Item start

    let kids = 
        row.Children
        |> Array.map (fun ch -> buildTree ch map)
        |> Array.toList

    { tree with Kids = kids }

let totalTree = buildTree partOne (treePairs (rows()))


let balance node =
    let totalNodeWeight =
        node.Kids 
        |> List.sumBy (fun el -> el.Data.Weight)
        |> fun s -> s + node.Data.Weight

    let diff =
        node.Kids 
        |> List.map (fun e -> e.Data.Weight)
        |> List.distinct
        |> fun l -> 
            match l with
            | [x] -> 0
            | x::[y] -> absDiff x y
            | _ -> failwith "impossible as per input"

    (totalNodeWeight, diff)


let rec balanced (rootNode:T) =

    if rootNode.Kids.IsEmpty 
    then 
        rootNode.Data.Weight
    else

        let weights = 
            rootNode.Kids 
            |> List.map balanced
        
        let diff =
            weights
            |> List.distinct
            |> fun l ->
                match l with
                | [x] -> 0
                | x::[y] -> x - y
                | _ -> failwith "impossible as per input"
        
        let v = 
            if diff <> 0 
            then printfn "diff : %i of  %A, kids: %A" diff rootNode.Data (rootNode.Kids |> List.map (fun k -> k.Data))
        
        rootNode.Data.Weight + ( weights |> List.sum)

balanced totalTree
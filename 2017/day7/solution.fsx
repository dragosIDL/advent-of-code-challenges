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
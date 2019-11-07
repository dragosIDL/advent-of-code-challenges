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

type State =   
    { 
        LFP: Set<string> // looks for parent
        LFK: Set<string> // looks for kid
    }

let resultInMutable = 

    let mutable lfp = Set.empty<string>
    let mutable lfk = Set.empty<string>
    for r in rows() do
        
        if lfk.Contains r.Data.Name 
        then lfk <- lfk.Remove r.Data.Name
        else lfp <- lfp.Add r.Data.Name

        for ch in r.Children do
            if lfp.Contains ch
                then lfp <- lfp.Remove ch
                else lfk <- lfk.Add ch

    lfp |> Set.toSeq |> Seq.head

let folder state el =

    let state =
        if state.LFK |> Set.contains el.Data.Name
        then {state with LFK = state.LFK |> Set.remove el.Data.Name}
        else {state with LFP = state.LFP |> Set.add el.Data.Name}

    el.Children 
    |> Array.fold (fun (s:State) e -> 
        if s.LFP |> Set.contains e
        then {state with LFP = state.LFP |> Set.remove e}
        else {state with LFK = state.LFK |> Set.add e}) state

let resultInImmutable = 
    rows()
    |> Array.fold folder { LFP = Set.empty<string>; LFK= Set.empty<string> }
    |> fun s -> s.LFP |> Set.toSeq |> Seq.head


#r "nuget: Newtonsoft.Json"

open System
open System.IO
open System.Collections.Generic

let inputPath =
    "C:\\Users\\dragos\\source\\dragosIDL\\advent-of-code-challenges\\2022\\day7\\input.txt"

let input = File.ReadAllLines inputPath

type Tree =
    | File of F
    | Dir of D

and F = { Name: string; Size: int }

and D =
    { Name: string
      Parent: D option
      Children: List<Tree> }

let pickChildDir dir name =
    dir.Children
    |> Seq.choose (function
        | Dir d -> Some d
        | _ -> None)
    |> Seq.find (fun x -> x.Name = name)

let rec goToRoot dir =
    match dir.Parent with
    | Some p ->
        printfn "%A" dir
        goToRoot (p)
    | None -> dir

let folder s line =
    match line with
    | "$ cd /" ->
        { Name = "/"
          Parent = None
          Children = new List<Tree>() }
    | "$ ls" -> s
    | "$ cd .." ->
        match s.Parent with
        | Some a -> a
        | None -> failwith "how?"
    | x when x.StartsWith("$ cd") -> pickChildDir s (x.Split(" ") |> Array.last)
    | x when x.StartsWith("dir") ->

        s.Children.Add(
            Dir(
                { Name = (x.Split(" ") |> Array.last)
                  Children = new List<Tree>()
                  Parent = Some s }
            )
        )

        s
    | x ->
        let d = x.Split(" ")
        let file = { Name = d[1]; Size = int d[0] }
        s.Children.Add(File(file))

        s

let directoryTree =
    input
    |> Array.fold
        folder
        { Name = "/"
          Parent = None
          Children = new List<Tree>() }
    |> goToRoot

let directorySizes tree =
    let rec accumulate (s: List<int>) acc =
        function
        | File f -> acc + f.Size
        | Dir d ->
            let ss = d.Children |> Seq.fold (accumulate s) 0
            s.Add(ss)
            ss + acc

    let ds = new List<int>()
    let ts = accumulate ds 0 tree
    ds, ts

let dirs, usedSize = directorySizes (Dir(directoryTree))

let part1 = dirs |> Seq.filter ((>) 100_000) |> Seq.sum

let needed = 30_000_000 - 70_000_000 + usedSize
let part2 = dirs |> Seq.filter ((<) needed) |> Seq.min
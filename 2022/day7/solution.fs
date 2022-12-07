open System
open System.IO
open System.Collections.Generic

let inputPath =
    "C:\\Users\\DragosIsmana\\source\\dragosIDL\\advent-of-code-challenges\\2022\\day7\\input.txt"

let input = File.ReadAllLines inputPath

// type File = {
//     Name: string
//     Size: int
// }

// type Dir = {
//     Name: string
//     Dirs: Dir list
//     Files: File list
//     Size: int
// }

// type Tree = 
// | Dir of Dir
// | File of File

// let root = { Name= "/"; Dirs = []; Files= []; Size = 0 }

let mutable dirs = "";
let mutable files = "";

let di = new Dictionary<list<string>, int>()

let folder (ls: string list) (command : string) =
    match command with 
    | "$ ls" -> ls
    | "$ cd .." -> ls.Tail
    | y when y.StartsWith("$ cd") ->  
        let name = y.Split(" ")[2];
        ls @ [name]
    | y when y.StartsWith("dir") ->
        let name = y.Split(" ")[1]
        // let path = ls |> fun x -> String.Join("", x)
        di.Add(ls @ ["["+name+"]"] , 0)
        ls
    | a -> 
        let asx = a.Split(" ")
        let size = int asx[0]
        let name = asx[1]
        // let path = ls |> fun x -> String.Join("", x)
        di.Add(ls @ [name] , size)
        ls




let systemTree = 
    input
    |> Array.skip 1
    |> Array.fold  folder ["[/]"]
open System
open System.IO

let inputPath =
    "C:\\Users\\DragosIsmana\\source\\dragosIDL\\advent-of-code-challenges\\2022\\day7\\input.txt"

let input = File.ReadAllLines inputPath

type File = {
    Name: string
    Size: int
}
type Dir = {
    Name: string
    Dirs: Dir list
    Parent: Dir option
    Files: File list
}

type Tree = 
| Dir of Dir
| File of File

let root = { Name= "/"; Dirs = []; Files= []; Parent= None; }

let systemTree = 
    input
    |> Array.skip 1
    |> Array.fold 
        (fun (currentDir: Dir) c ->
            match c with 
            | "$ ls" -> currentDir
            | "$ cd .." ->
                match currentDir.Parent with 
                | None -> failwith "impossible"
                | Some parent -> parent
            | y when y.StartsWith("dir") ->
                let dirName = y.Split(" ")[1]
                let foundDir = { Name = dirName; Files=[]; Dirs=[]; Parent = currentDir}
                let newS = { currentDir with Dirs = currentDir.Dirs @ [foundDir]}
                newS
            | y when y.StartsWith("$ cd") ->
                let dirName = y.Split(" ")[2]
                currentDir
            | a -> 
                let d = a.Split(" ")
                let fileName = d[1]
                let fileSize = int d[0]
                currentDir
        ) 
        (root)



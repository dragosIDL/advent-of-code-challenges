open System
open System.IO

let inputPath = "2022\\day6\\input.txt"

let input = File.ReadAllText inputPath

let compute nr =
    Array.windowed nr
    >> Array.map (Array.distinct >> Array.length)
    >> Array.findIndex ((=) nr)
    >> ((+) nr)

let part1 = compute 4 (input.ToCharArray())
let part2 = compute 14 (input.ToCharArray())

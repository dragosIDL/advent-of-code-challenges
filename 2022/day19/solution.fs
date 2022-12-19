open System
open System.IO
open System.Collections.Generic
open System.Linq

let inputPath = "2022\\day19\\input.txt"
let outputPath = "2022\\day19\\output.txt"

let input = inputPath |> File.ReadAllLines


[<Measure>]
type ore

[<Measure>]
type clay

[<Measure>]
type obsidian

type Blueprint =
    { Id: int
      Ore: int<ore>
      Clay: int<ore>
      Obsidian: int<ore> * int<clay>
      Geode: int<ore> * int<obsidian> }

let removeStr (toRemove: string) (a: string) = a.Replace(toRemove, "")

let parseLine (line: string) =

    let parts1 = line.Split(':') |> List.ofArray

    let id =
        parts1[0]
        |> fun a -> a.Replace("Blueprint", "") |> int

    let costs = parts1[1] |> fun a -> a.Split('.')


    { Id = id
      Ore =
        (costs[0]
         |> removeStr "Each ore robot costs "
         |> fun a -> (int a[..1]) * 1<ore>)
      Clay =
        (costs[1]
         |> removeStr "Each clay robot costs "
         |> fun a -> (int a[..1]) * 1<ore>)
      Obsidian =
        (costs[2]
         |> removeStr "Each obsidian robot costs "
         |> fun a -> ((int a[..1]) * 1<ore>, (int a[^6..^5]) * 1<clay>))
      Geode =
        (costs[3]
         |> removeStr "Each geode robot costs "
         |> fun a -> ((int a[..1]) * 1<ore>, (int a[^10..^9]) * 1<obsidian>)) }


let blueprints = input |> Array.map parseLine

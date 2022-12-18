open System
open System.IO
open System.Collections.Generic
open System.Linq

let inputPath = "2022\\day16\\input.txt"

let input = File.ReadAllLines inputPath

type Valve =
    { Name: string
      FlowRate: int
      Tunnels: List<Valve> }

let parse (line: string) =

    let valveStr, tunnelsStr = line.Split("; ") |> fun a -> a[0], a[1]
    let valveName = valveStr[6..7]
    let flowRate = int valveStr[23..]

    let tunnelsName =
        tunnelsStr[22..]
        |> fun a -> a.Split(",")
        |> Array.map (fun a ->
            { Name = a.Trim()
              FlowRate = 0
              Tunnels = new List<Valve>() })
        |> fun a -> new List<Valve>(a)


    { Name = valveName
      FlowRate = flowRate
      Tunnels = tunnelsName 
      
      }


let valves = input |> Array.map parse

let valvesDict = valves |> Array.map (fun a -> (a.Name, a)) |> dict

let valvesMap =
    valves
    |> Array.map (fun v ->
        let definedValves =
            v.Tunnels
            |> Seq.map (fun a -> valvesDict[a.Name])
            |> fun a -> new List<Valve>(a)

        { v with Tunnels = definedValves })

// parse "Valve AA has flow rate=0; tunnels lead to valves DD, II, BB"

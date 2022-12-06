open System.IO
open System

let inputPath =
    "C:\\Users\\dragos\\source\\dragosIDL\\advent-of-code-challenges\\2022\\day1\\input.txt"

let input = File.ReadAllText inputPath

let caloriesPerElf (input: string) =
    input.Split(Environment.NewLine + Environment.NewLine)
    |> Array.map (fun x -> x.Split Environment.NewLine)
    |> Array.map (Array.sumBy (fun x -> (int) x))

let part1 = input |> caloriesPerElf |> Array.max

let part2 =
    input
    |> caloriesPerElf
    |> Array.sortDescending
    |> Array.take 3
    |> Array.sum

type State =
    { BestElfIndex: int
      BestElfCalories: int
      ElfIndex: int
      ElfCalories: int }

    static member updateBestElf(state: State) =
        match state.ElfCalories > state.BestElfCalories with
        | false -> state
        | true ->
            { state with
                BestElfIndex = state.ElfIndex
                BestElfCalories = state.ElfCalories }

    static member goToNextElf state =
        { state with
            ElfIndex = state.ElfIndex + 1
            ElfCalories = 0 }

    static member incrementCurrentElfBag amount state =
        { state with ElfCalories = state.ElfCalories + amount }

let (index, calories) =

    let folder (state: State) =
        function
        | "" -> state |> State.updateBestElf |> State.goToNextElf
        | x -> State.incrementCurrentElfBag (int x) state

    File.ReadAllLines inputPath
    |> Array.fold
        folder
        { BestElfIndex = 0
          BestElfCalories = 0
          ElfIndex = 0
          ElfCalories = 0 }
    |> (fun state -> (state.BestElfIndex, state.BestElfCalories))

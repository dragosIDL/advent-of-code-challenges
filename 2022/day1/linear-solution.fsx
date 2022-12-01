open System.IO

let inputPath =
    "C:\\Users\\dragos\\source\\dragosIDL\\advent-of-code-challenges\\2022\\day1\\input.txt"

let input = File.ReadAllLines inputPath

type State =
    { BestElfIndex: int
      BestElfCalories: int
      ElfIndex: int
      ElfCalories: int }

let folder (state: State) =
    function
    | "" ->
        let state =
            match (state.ElfCalories > state.BestElfCalories) with
            | true ->
                { state with
                    BestElfIndex = state.ElfIndex
                    BestElfCalories = state.ElfCalories }
            | false -> state

        { state with
            ElfIndex = state.ElfIndex + 1
            ElfCalories = 0 }

    | x -> { state with ElfCalories = state.ElfCalories + (int) x }

let (index, calories) =
    let state = { 
        BestElfIndex = 0
        BestElfCalories = 0 
        ElfIndex = 0 
        ElfCalories = 0 
    }

    input
    |> Array.fold folder state
    |> (fun state -> (state.BestElfIndex, state.BestElfCalories))

printfn "The elf number %i has %i calories" index calories

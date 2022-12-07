open System.IO

let inputPath = "2022\\day2\\input.txt"

let input = File.ReadAllLines inputPath

let part1Mapping =
    function
    | "A X" -> (1 + 3)
    | "B X" -> (1 + 0)
    | "C X" -> (1 + 6)

    | "A Y" -> (2 + 6)
    | "B Y" -> (2 + 3)
    | "C Y" -> (2 + 0)

    | "A Z" -> (3 + 0)
    | "B Z" -> (3 + 6)
    | "C Z" -> (3 + 3)
    | _ -> 0

let part2Mapping =
    function
    | "A X" -> (3 + 0)
    | "B X" -> (1 + 0)
    | "C X" -> (2 + 0)

    | "A Y" -> (1 + 3)
    | "B Y" -> (2 + 3)
    | "C Y" -> (3 + 3)

    | "A Z" -> (2 + 6)
    | "B Z" -> (3 + 6)
    | "C Z" -> (1 + 6)
    | _ -> 0

let mapSum mapping = Array.map mapping >> Array.sum

let part1 = mapSum part1Mapping input
let part2 = mapSum part2Mapping input

open System 
open System.IO

let input() = 
    File.ReadAllLines "2017/day5/input.txt"
    |> Array.map Int32.Parse

let posMut oldVal = 
    oldVal + 1

let pathToExit posMut (input:int array) =

    let rec path step position =
        if position >= input.Length || position < 0 
            then step
        else
            let currentVal = input.[position]
            input.[position] <- posMut currentVal
            path (step + 1) (position + currentVal)

    path 0 0

let partOne = pathToExit ((+) 1) (input()) // 336905

let newPos off = 
    if off >= 3 then off - 1 
    else off + 1

let partTwo = pathToExit newPos (input()) // 21985262

(*
    As you walk through the door, a glowing humanoid shape yells in your direction. 
    "You there! Your state appears to be idle. Come help us repair the corruption 
    in this spreadsheet - if we take another millisecond, we'll have to display an hourglass cursor!"

    The spreadsheet consists of rows of apparently-random numbers. To make sure the
    recovery process is on the right track, they need you to calculate the spreadsheet's 
    checksum. For each row, determine the difference between the largest value and the 
    smallest value; the checksum is the sum of all of these differences.

    5 1 9 5
    7 5 3
    2 4 6 8
  
    The first row's largest and smallest values are 9 and 1, and their difference is 8.
    The second row's largest and smallest values are 7 and 3, and their difference is 4.
    The third row's difference is 6.
    In this example, the spreadsheet's checksum would be 8 + 4 + 6 = 18.

    What is the checksum for the spreadsheet in your puzzle input?
*)

open System
open System.IO

let read fPath = File.ReadAllLines fPath

let spreadSheet =
    read "2017/CorruptionChecksum/checksum.txt" |> Array.map (fun line -> line.Split '\t' |> Array.map Int32.Parse)

let minMaxLine (l: int array) =
    let min = l |> Array.min
    let max = l |> Array.max
    (min, max)

let result =
    spreadSheet
    |> Array.map minMaxLine
    |> Array.sumBy (fun (min, max) -> max - min)

(*
    --- Part Two ---
    "Great work; looks like we're on the right track after all. Here's a star for your effort.
    " However, the program seems a little worried. Can programs be worried?

    "Based on what we're seeing, it looks like all the User wanted is some information 
    about the evenly divisible values in the spreadsheet. Unfortunately, none of us are 
    equipped for that kind of calculation - most of us specialize in bitwise operations."

    It sounds like the goal is to find the only two numbers in each row where one 
    evenly divides the other - that is, where the result of the division operation 
    is a whole number. They would like you to find those numbers on each line, divide
    them, and add up each line's result.

    For example, given the following spreadsheet:

    5 9 2 8
    9 4 7 3
    3 8 6 5
    In the first row, the only two numbers that evenly divide are 8 and 2; the result of this division is 4.
    In the second row, the two numbers are 9 and 3; the result is 3.
    In the third row, the result is 2.
    In this example, the sum of the results would be 4 + 3 + 2 = 9.
*)

let divideEvenly (l: int array) =
    seq {
        for i in [ 0 .. l.Length - 2 ] do
            for j in [ i + 1 .. l.Length - 1 ] do
                let iv = l.[i]
                let jv = l.[j]
                if iv % jv = 0 then yield (iv, jv)
                if jv % iv = 0 then yield (jv, iv)
        failwith "based on assumptions, impossbile"
    }
    |> Seq.head

let partTwo =
    spreadSheet
    |> Array.map divideEvenly
    |> Array.sumBy (fun (a, b) -> a / b)
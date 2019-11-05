open System
open System.IO

let read fPath = File.ReadAllLines fPath

let spreadSheet() =
    read "2017/day2/input.txt" 
    |> Array.map (fun line -> line.Split '\t' |> Array.map Int32.Parse)

let maxMinLine (l: int array) =
    let min = l |> Array.min
    let max = l |> Array.max
    (max, min)

let pairF f (a,b) = f a b

let partOne =
    spreadSheet()
    |> Array.map maxMinLine
    |> Array.sumBy (pairF (-))

let divideEvenly (l: int array) =
    seq {
        for i in [ 0 .. l.Length - 2 ] do
            for j in [ i + 1 .. l.Length - 1 ] do
                let iv = l.[i]
                let jv = l.[j]
                if iv % jv = 0 then yield (iv, jv)
                if jv % iv = 0 then yield (jv, iv)
    }
    |> Seq.head

let partTwo =
    spreadSheet()
    |> Array.map divideEvenly
    |> Array.sumBy (pairF (/))
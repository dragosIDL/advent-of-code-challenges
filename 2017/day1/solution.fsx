open System
open System.IO

let captcha() =
    File.ReadAllLines "2017/day1/input.txt" 
    |> Array.head
    |> fun line -> line.ToCharArray() 
    |> Array.map (fun el -> Int32.Parse (el.ToString()))

let generatePairs pairWith (c: 'a array) =
    c |> Array.mapi (fun i el -> (el, (pairWith c i)))
    
let sumMatching matchWith captcha =

    captcha
    |> generatePairs matchWith
    |> Seq.sumBy (fun (x, y) ->
        if x = y then x
        else 0)

let matchWithOffset offsetPicker (arr: int array)  i =
    arr.[( offsetPicker arr i)]

let withNext (arr:'a array) i = (i + 1) % arr.Length
let partOne = captcha() |> sumMatching (matchWithOffset withNext)

let withMiddle (arr:'a array) i = (i + (arr.Length/2))% arr.Length
let partTwo = captcha() |> sumMatching (matchWithOffset withMiddle)
open System
(*
    https://adventofcode.com/2017/day/1
    
    The captcha requires you to review a sequence of digits (your puzzle input) 
    and find the sum of all digits that match the next digit in the list. The list 
    is circular, so the digit after the last digit is the first digit in the list.

    For example:

    1122 produces a sum of 3 (1 + 2) because the first digit (1) matches the second digit and the third digit (2) matches the fourth digit.
    1111 produces 4 because each digit (all 1) matches the next.
    1234 produces 0 because no digit matches the next.
    91212129 produces 9 because the only digit that matches the next one is the last digit, 9.

*)

open System.IO

let read fPath = File.ReadAllLines fPath

let captcha =
    read "2017/InverseCaptcha/captcha.txt" 
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

let intArr = 
    captcha.ToCharArray()
    |> Array.map (fun el -> Int32.Parse(el.ToString())) // read as int array

let matchWithNextResult = intArr |> sumMatching (matchWithOffset withNext)

(*

    --- Part Two ---
    You notice a progress bar that jumps to 50% completion. Apparently, the door 
    isn't yet satisfied, but it did emit a star as encouragement. The instructions change:

    Now, instead of considering the next digit, it wants you to consider the digit 
    halfway around the circular list. That is, if your list contains 10 items, only 
    include a digit in your sum if the digit 10/2 = 5 steps forward matches it. 
    Fortunately, your list has an even number of elements.
*)

let withMiddle (arr:'a array) i = (i + (arr.Length/2))% arr.Length
let matchWithMiddleResult = intArr |> sumMatching (matchWithOffset withMiddle)
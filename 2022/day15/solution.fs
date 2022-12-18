open System
open System.IO

let inputPath = "2022\\day15\\input.txt"

let input = File.ReadAllLines inputPath


type Sensor = float * float
type Beacon = float * float

let parsePosition (l: string) =
    l[l.IndexOf("x=") ..]
    |> fun x -> x.Split(",")
    |> Array.map (fun x -> x.Trim())
    |> Array.map (fun x -> float (x[2..]))
    |> fun a -> (a[0], a[1])

let parse (l: string) =
    l.Split(":")
    |> Array.map parsePosition
    |> fun a -> (Sensor a[0], Beacon a[1])

let positions = input |> Array.map parse

let minX =
    positions
    |> Array.map (fun ((sx, _), (bx, _)) -> min sx bx)
    |> Array.min

let minY =
    positions
    |> Array.map (fun ((_, sy), (_, by)) -> min sy by)
    |> Array.min

let normalizedToZero =
    positions
    |> Array.map (fun ((sx, sy), (bx, by)) -> (Sensor(sx - minX, sy - minY), Beacon(bx - minX, by - minY)))


// https://bryceboe.com/2006/10/23/line-segment-intersection-algorithm/
// let intersect1 (a, b, c, d) =

//     let clockwise ((ax, ay), (bx, by), (cx, cy)) =
//         (cy - ay) * (bx - ax) > (by - ay) * (cx - ax)

//     clockwise (a, c, d) <> clockwise (b, c, d)
//     && clockwise (a, b, c) <> clockwise (a, b, d)


let intersect2 (a, b, c, d) =

    let onSegment ((ax, ay), (bx, by), (cx, cy)) =

        (bx <= max ax cx && bx >= min ax cx)
        && (by <= max ay cy && by >= min ay cy)

    let orientation ((ax, ay), (bx, by), (cx, cy)) =

        let r = (by - ay) * (cx - bx) - (bx - ax) * (cy - by)

        if r = 0.0 then 0.0
        else if r > 0.0 then 1.0
        else 2.0

    let o1 = orientation (a, b, c)
    let o2 = orientation (a, b, d)
    let o3 = orientation (c, d, a)
    let o4 = orientation (c, d, b)

    if (o1 <> o2 && o3 <> o4) then
        true
    else
        (o1 = 0.0 && onSegment (a, c, b))
        || (o2 = 0.0 && onSegment (a, d, b))
        || (o3 = 0.0 && onSegment (c, a, d))
        || (o4 = 0.0 && onSegment (c, b, d))


let intersect3 ((sx, sy), (bx, by)) =
    let miny = min sy by
    let maxy = max sy by

    miny <= (2_000_000.0 - minY)
    && maxy >= (2_000_000.0 - minY)

let rowstart, rowend = (0.0, 2_000_000.0 - minY), (10_000_000.0, 2_000_000.0 - minY)

// let interestingPairs1 =
//     normalizedToZero
//     |> Array.filter (fun (s, b) -> intersect1 (s, b, rowstart, rowend))

let interestingPairs2 =
    normalizedToZero
    |> Array.filter (fun (s, b) -> intersect2 (s, b, rowstart, rowend))

let divBy (y: float) ((x1, x2): Sensor, (x3, x4): Beacon) =
    (Sensor(x1 / y, x2 / y), Beacon(x3 / y, x4 / y))

let interestingPairs3 = normalizedToZero |> Array.filter intersect3
// |> Array.map (divBy 1_000_000.0)


let yRowX =
    interestingPairs3
    |> Array.map (fun ((sx, sy), (bx, by)) ->

        let dist = abs (sx - bx) + abs (sy - by)

        let onRowNr = dist - abs (sy - 2_000_000.0)

        let onRowFrom = (sx - onRowNr, sx + onRowNr)

        onRowFrom)

let normalX =
    yRowX
    |> Array.map (fun (a, b) -> min a b)
    |> Array.min

let normalized =
    yRowX
    |> Array.map (fun (a, b) -> (a - normalX, b - normalX))

let distinctRow = 
    normalized
    |> Array.sortBy (fun (a,b) -> a)
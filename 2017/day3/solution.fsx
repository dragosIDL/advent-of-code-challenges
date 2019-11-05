let input = 325489

let fsqrt = float >> sqrt
let cornerSq = input |> fsqrt |> floor |> int
let bottomRightSq = 
    if cornerSq % 2 = 0 
    then cornerSq - 1 
    else cornerSq

let sideLen = (bottomRightSq + 1)
let startPoint = (pown bottomRightSq 2) 

let sides = seq { 
    for i in [0..3] do
        yield (
            (startPoint + 1 + i * sideLen),
            (startPoint + (i + 1)*sideLen)
        )
}

let sideStart = 
    sides 
    |> Seq.find (fun (s, e) -> s < input && e > input ) 
    |> fst

let position = input - sideStart + 1 
let cartezianLen = sideLen / 2
let offsetToCartezian = cartezianLen - position

let totalMovement = offsetToCartezian + cartezianLen

type Dir =
    | R
    | U
    | L
    | D

let next = function
    | R -> U
    | U -> L
    | L -> D
    | D -> R

let dirGenerator =
    seq {
        let mutable level = 1
        let mutable dir = R
        while true do
            for _ in [1..2] do 
                for _ in [1..level] do
                    yield dir
                dir <- next dir
            level <- level + 1
    }

let nextPos currentPos dir = 
    let (x,y) = currentPos
    match dir with 
    | R -> (x, y + 1) 
    | U -> (x - 1, y)
    | L -> (x, y - 1)
    | D -> (x + 1, y)

let spiralPositions startPos = 
    seq {
        yield startPos
        let (x,y) = startPos
        let mutable xm = x
        let mutable ym = y
        for dir in dirGenerator do
            let (a, b) = nextPos (xm, ym) dir
            xm <- a
            ym <- b
            yield (xm, ym)
    }

let neighbors (x, y) =
    [
        (x - 1, y - 1); (x - 1, y); (x - 1, y + 1);
        (x    , y - 1);             (x    , y + 1);
        (x + 1, y - 1); (x + 1, y); (x + 1, y + 1);
    ]

let sumOfNeighbors (matrix:int [][]) position = 
    neighbors position
    |> List.sumBy (fun (x,y) -> matrix.[x].[y] )

let generateEmptyMatrix size = 
    [|0..size|] 
    |> Array.map (fun _ -> 
        [|0..size|] |> Array.map (fun _ -> 0))

let incremental (m:int[][]) i (x,y) =
    m.[x].[y] <- (i+1)

let sumByNeighbors (m:int[][]) i (x,y) =
    if i = 0 
        then m.[x].[y] <- 1
    else
    let sum = sumOfNeighbors m (x,y)
    m.[x].[y] <- sum

let generateSpiralMatrix how size =

    let start = size / 2
    let matrix = generateEmptyMatrix size 

    spiralPositions (start, start)
    |> Seq.take 100
    |> Seq.iteri (fun i pos -> how matrix i pos)

    matrix

let generated = generateSpiralMatrix sumByNeighbors 12

// 330785
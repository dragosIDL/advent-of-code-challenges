(*
    https://adventofcode.com/2017/day/3

    --- Day 3: Spiral Memory ---
    You come across an experimental new kind of memory stored on an infinite two-dimensional grid.

    Each square on the grid is allocated in a spiral pattern starting at a location marked 
    1 and then counting up while spiraling outward. For example, the first few squares are allocated like this:

    17  16  15  14  13
    18   5   4   3  12
    19   6   1   2  11
    20   7   8   9  10
    21  22  23---> ...
    While this is very space-efficient (no squares are skipped), requested data must be carried 
    back to square 1 (the location of the only access port for this memory system) by programs 
    that can only move up, down, left, or right. They always take the shortest path: the 
    Manhattan Distance between the location of the data and square 1.

    For example:

    Data from square 1 is carried 0 steps, since it's at the access port.
    Data from square 12 is carried 3 steps, such as: down, left, left.
    Data from square 23 is carried only 2 steps: up twice.
    Data from square 1024 must be carried 31 steps.
    How many steps are required to carry the data from the square identified in 
    your puzzle input all the way to the access port? 
    
    Input: 325489
*)

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

(*
    --- Part Two ---
    As a stress test on the system, the programs here clear the grid and then store the value 1 in square 1. Then, in the same allocation order as shown above, they store the sum of the values in all adjacent squares, including diagonals.

    So, the first few squares' values are chosen as follows:

    Square 1 starts with the value 1.
    Square 2 has only one adjacent filled square (with value 1), so it also stores 1.
    Square 3 has both of the above squares as neighbors and stores the sum of their values, 2.
    Square 4 has all three of the aforementioned squares as neighbors and stores the sum of their values, 4.
    Square 5 only has the first and fourth squares as neighbors, so it gets the value 5.
    Once a square is written, its value does not change. Therefore, the first few squares would receive the following values:

        6022  5600 5203  5022  2450
    147  142  133  122   59    2391
    304    5    4    2   57    2275
    330   10    1    1   54    2105
    351   11   23   25   26    1968 
    362  747  806  880  931     957

    What is the first value written that is larger than your puzzle input?

    Input: 325489
*)

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
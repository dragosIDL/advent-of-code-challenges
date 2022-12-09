using System.Net.NetworkInformation;
using System.IO.Pipes;
using System.Security.AccessControl;
using System.IO;
using System.Linq;

var inputPath = "input.txt";

var inputLines = File.ReadAllLines(inputPath);

var moves = inputLines
    .Select(l =>
    {
        var x = l.Split(" ");
        return (x[0], int.Parse(x[1]));
    })
    .ToList();


var initialStart = (0, 0);
var head = initialStart;
var tail = initialStart;
var set = new HashSet<(int, int)>();

foreach (var (direction, steps) in moves)
{
    for (var i = 0; i < steps; i++)
    {
        (head, tail) = Move(direction, head, tail);
        System.Console.WriteLine($"DIR:{direction}   STEP:{i}   HEAD: {head}   TAIL:{tail}");
        set.Add(tail);
    }
}

((int, int), (int, int)) Move(string direction, (int, int) head, (int, int) tail)
{
    return direction switch
    {
        "U" => MoveUp(head, tail),
        "D" => MoveDown(head, tail),
        "L" => MoveLeft(head, tail),
        "R" => MoveRight(head, tail),
        _ => throw new InvalidOperationException()
    };

    ((int, int), (int, int)) MoveUp((int hx, int hy) head, (int tx, int ty) tail)
    {
        var ((hx, hy), (tx, ty)) = (head, tail);
        var newHead = (hx + 1, hy);
        return (newHead, MoveTail(newHead, tail));
    }

    ((int, int), (int, int)) MoveDown((int hx, int hy) head, (int tx, int ty) tail)
    {
        var ((hx, hy), (tx, ty)) = (head, tail);
        var newHead = (hx - 1, hy);
        return (newHead, MoveTail(newHead, tail));
    }

    ((int, int), (int, int)) MoveLeft((int hx, int hy) head, (int tx, int ty) tail)
    {
        var ((hx, hy), (tx, ty)) = (head, tail);
        var newHead = (hx, hy - 1);
        return (newHead, MoveTail(newHead, tail));
    }
    ((int, int), (int, int)) MoveRight((int hx, int hy) head, (int tx, int ty) tail)
    {
        var ((hx, hy), (tx, ty)) = (head, tail);
        var newHead = (hx, hy + 1);
        return (newHead, MoveTail(newHead, tail));
    }

    (int, int) MoveTail((int hx, int hy) head, (int tx, int ty) tail)
    {
        var ((hx, hy), (tx, ty)) = (head, tail);
        var isOverlapping = (hx == tx && hy == ty);

        var aroundPositions = new[]{
            (tx + 1, ty - 1),
            (tx + 1, ty),
            (tx + 1, ty + 1),
            (tx, ty - 1),
            (tx, ty + 1),
            (tx - 1, ty - 1),
            (tx - 1, ty),
            (tx - 1, ty +  1),
        };

        var isTouching = isOverlapping || aroundPositions.Any(t => t == head);

        if (isTouching)
        {
            return tail;
        }
        else
        {
            var (dirX, dirY) = (hx - tx, hy - ty);

            return (
                dirX > 0 ? tx + 1 : dirX == 0 ? tx : tx - 1,
                dirY > 0 ? ty + 1 : dirY == 0 ? ty : ty - 1);
        }
    }
}

// 8687 - too high
// 5657 - too high
// 5619 - idk
Console.WriteLine(set.Count);

var ls = set.ToList();
foreach (var e in ls)
{
    System.Console.WriteLine(e);
}
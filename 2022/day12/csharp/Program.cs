var inputPath = "input.txt";
var input = File
    .ReadAllLines(inputPath)
    .Select(l => l.ToCharArray().ToList())
    .ToList();

var startPosition1 = ToPosition(input, 'S') with { Cost = 1, Height = 1 };
var endPosition = ToPosition(input, 'E') with { Cost = 26, Height = 26 };

input[startPosition1.X][startPosition1.Y] = 'a';
input[endPosition.X][endPosition.Y] = 'z';

var map = input.Select(ls => ls.Select(l => l - 96).ToList()).ToList();

var startPositions = Enumerable.Range(0, map.Count).Select(i => (i: i, j: 0)).ToList();

startPositions.Select(a => map[a.i][a.j]).ToList().ForEach(a => Console.WriteLine(a));

var ls = new List<int>();

foreach (var sp in startPositions)
{
    var c = Go(new Position() { X = sp.i, Y = sp.j });

    ls.Add(c);
}

var min = ls.Min();
var max = ls.Max();

Console.WriteLine(string.Join(",", ls));
Console.WriteLine($"MIN: {min} ... MAX: {max}");

int Go(Position startPosition)
{
    map = input.Select(ls => ls.Select(l => l - 96).ToList()).ToList();

    startPosition.SetDistance(endPosition);

    var active = new List<Position>() { startPosition };
    var visited = new List<Position>();

    while (active.Any())
    {
        var check = active.OrderBy(x => x.CostDistance).First();
        if (check.X == endPosition.X && check.Y == endPosition.Y)
        {
            var t = check;
            int c = 0;
            while (true)
            {
                if (map[t.X][t.Y] > 0)
                {
                    map[t.X][t.Y] = 0;
                }
                c++;
                t = t.Parent;
                if (t is null)
                {
                    Console.WriteLine($"COUNTER {c}");
                    return c;
                }
            }
        }

        visited.Add(check);
        active.Remove(check);

        var goTo = GetDirections(map, check, endPosition);
        foreach (var pos in goTo)
        {
            if (visited.Any(x => x.X == pos.X && x.Y == pos.Y))
            {
                continue;
            }

            if (active.Any(x => x.X == pos.X && x.Y == pos.Y))
            {
                var existing = active.First(x => x.X == pos.X && x.Y == pos.Y);
                if (existing.CostDistance > check.CostDistance)
                {
                    active.Remove(existing);
                    active.Add(pos);
                }
            }
            else
            {
                active.Add(pos);
            }
        }
    }

    return 1000;
}


static List<Position> GetDirections(List<List<int>> map, Position current, Position target)
{
    var indexes = new int[] { 0, 0, -1, 1 }.Zip(new int[] { -1, 1, 0, 0 });

    var possibleMoves = indexes
        .Select(p => (X: current.X + p.First, Y: current.Y + p.Second))
        .Where(p => p.X >= 0 && p.X <= map.Count - 1 && p.Y >= 0 && p.Y <= map[0].Count - 1)
        .Where(p => map[p.X][p.Y] <= current.Height + 1)
        .Select(move => new Position { X = move.X, Y = move.Y, Parent = current, Cost = current.Cost + map[move.X][move.Y], Height = map[move.X][move.Y] })
        .ToList();

    possibleMoves.ForEach(t => t.SetDistance(target));

    return possibleMoves;
}

static Position ToPosition(List<List<char>> map, char x)
{
    var i = map.FindIndex(cs => cs.Any(c => c == x));
    var j = map[i].FindIndex(cs => cs == x);

    return new Position() { X = i, Y = j };
}

public record Position
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Cost { get; set; }
    public int Height { get; set; }
    public int Distance { get; set; }
    public int CostDistance => Cost + Distance;
    public Position Parent { get; set; }

    public void SetDistance(Position target)
    {
        this.Distance = Math.Abs(target.X - X) + Math.Abs(target.Y - Y);
    }
}
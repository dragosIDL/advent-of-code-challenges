var inputPath = "input.txt";

var inputLines = File.ReadAllLines(inputPath);

var matrix = inputLines
    .Select(l => l.ToCharArray().Select(l => (int)l - 48).ToList())
    .ToList();

List<(int score, bool isVisible)> trees = new List<(int, bool)>();
for (var i = 1; i < matrix.Count - 1; i++)
    for (var j = 1; j < matrix[i].Count - 1; j++)
        trees.Add(ComputeTree(matrix, i, j));

(int, bool) ComputeTree(List<List<int>> matrix, int i, int j)
{
    var center = matrix[i][j];

    var topTrees = Enumerable.Range(0, i).Select(index => matrix[i - 1 - index][j]).ToList();
    var bottomTrees = Enumerable.Range(i + 1, matrix.Count - i - 1).Select(index => matrix[index][j]).ToList();
    var leftTrees = Enumerable.Range(0, j).Select(index => matrix[i][j - 1 - index]).ToList();
    var rightTrees = Enumerable.Range(j + 1, matrix[i].Count - j - 1).Select(index => matrix[i][index]).ToList();

    var score = CountVisibleTrees(topTrees, center)
        * CountVisibleTrees(bottomTrees, center)
        * CountVisibleTrees(leftTrees, center)
        * CountVisibleTrees(rightTrees, center);

    var isVisible =
        topTrees.Max() < center
        || bottomTrees.Max() < center
        || leftTrees.Max() < center
        || rightTrees.Max() < center;

    return (score, isVisible);
}

var rows = matrix[0].Count * 2;
var sides = (matrix.Count - 2) * 2;

Console.WriteLine($"The amount of visible trees is {trees.Count(a => a.isVisible) + rows + sides}");
Console.WriteLine($"Max score {trees.Max(a => a.score)}");

static int CountVisibleTrees(IEnumerable<int> el, int item)
{
    int l = 0;
    foreach (var e in el)
    {
        if (e < item)
        {
            l++;
        }
        else
        {
            l++;
            break;
        }
    }
    return l;
}
using System.Text.Json;
// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");


var path = "C:\\Users\\DragosIsmana\\source\\dragosIDL\\advent-of-code-challenges\\2022\\day7\\input.txt";

var input = File.ReadAllLines(path).Skip(1);
Console.WriteLine(JsonSerializer.Serialize(input));

var parsedTree = input.Aggregate(new Dictionary<string, object>(), ParseTree);


Dictionary<string, object> ParseTree(
    Dictionary<string, object> dict,
    List<string> paths,
    string command)
{
    return command switch
    {
        "$ cd .." => paths.
        "$ ls" => root,
        var x when x.StartsWith("dir") => root,
        var x when x.StartsWith("$ cd") => root,
        var x => root
    };
}

// public record Tree
// {
//     public record Dir(string Name, List<Tree> Tree, Dir? Parent) : Tree;
//     public record File(string Name, int Size) : Tree;
// }

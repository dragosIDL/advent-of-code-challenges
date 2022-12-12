using System.Security.AccessControl;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var lines = File.ReadAllLines("input.txt");

var sample = new List<Monkey>
{
    new Monkey(new List<double>(){ 79, 98 },i => i * 19,i => i % 23 == 0 ? 2 : 3),
    new Monkey(new List<double>(){ 54, 65, 75, 74 },i => i + 6,i => i % 19 == 0 ? 2 : 0),
    new Monkey(new List<double>(){ 79, 60, 97 }, i => i * i, i => i % 13 == 0 ? 1 : 3),
    new Monkey(new List<double>(){ 74 },i => i + 3,i => i % 17 == 0 ? 0 : 1)
};

double sampleDivs = 23 * 19 * 13 * 17;


var monkeys = new List<Monkey>
{
    new Monkey(new List<double>(){ 54, 61, 97, 63, 74 },i => i * 7,i => i % 17 == 0 ? 5 : 3),
    new Monkey(new List<double>(){ 61, 70, 97, 64, 99, 83, 52, 87 },i => i + 8,i => i % 2 == 0 ? 7 : 6),
    new Monkey(new List<double>(){ 60, 67, 80, 65 }, i => i * 13, i => i % 5 == 0 ? 1 : 6),
    new Monkey(new List<double>(){ 61, 70, 76, 69, 82, 56 },i => i + 7,i => i % 3 == 0 ? 5 : 2),
    new Monkey(new List<double>(){ 79, 98 },i => i +2,i => i % 7 == 0 ? 0 : 3),
    new Monkey(new List<double>(){ 72, 79, 55 },i => i + 1,i => i % 13 == 0 ? 2 : 1),
    new Monkey(new List<double>(){ 63 },i => i + 4,i => i % 19 == 0 ? 7 : 4),
    new Monkey(new List<double>(){ 72, 51, 93, 63, 80, 86, 81 },i => i * i,i => i % 11 == 0 ? 0 : 4)
};


double divs = 17 * 2 * 5 * 3 * 7 * 13 * 19 * 11;

for (var i = 0; i < 10_000; i++)
{
    foreach (var m in monkeys)
    {
        foreach (var item in m.Items)
        {
            var wLevel = m.InspectItem(item) % divs;
            var throwTo = m.Test(wLevel);

            monkeys[throwTo].Items.Add(wLevel);
            m.Inspections++;
        }

        m.Items.Clear();
    }
}

var part2 = monkeys.OrderByDescending(m => m.Inspections).Take(2).Aggregate(1d, (s, m) => s * m.Inspections);
Console.WriteLine(part2);

public record Monkey(List<double> Items, Func<double, double> InspectItem, Func<double, int> Test)
{
    public int Inspections { get; set; }
};
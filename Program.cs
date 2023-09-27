// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

Console.WriteLine("Hello, World!");

static void Main()
{
    TurnCondition t1 = new TurnCondition
    {
        Name = "Feather",
        Tuners = new Dictionary<Attribute, List<Tuner>>()
        {
            { Attribute.atk, new List<Tuner>(){ new TunerFlat(-20) } },
            { Attribute.spd, new List<Tuner>(){ new TunerFlat(60), new TunerMaxPerc(100) } }
        },
        Turns = 3
    };
    Unit c1 = new(50, 20);
    c1.TurnConditions.Add(t1);
    Console.WriteLine(c1.GetAtk());
    Console.WriteLine(c1.GetSpd());
    static void NextTurn(ITurnObject turnObject)
    {
        turnObject.NextTurn();
    }
    NextTurn(c1);
    NextTurn(c1);
    NextTurn(c1);
    Console.WriteLine(c1.GetAtk());
    Console.WriteLine(c1.GetSpd());
    Console.WriteLine("Hello, World!");
}
Main();
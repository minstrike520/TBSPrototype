// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

Console.WriteLine("Hello, World!");

static void Main()
{
    TurnTuner t1 = new()
    {
        Name = "Feather",
        Tuners = new Dictionary<TunedAttribute, List<Tuner>>()
        {
            { TunedAttribute.Atk, new List<Tuner>(){ new FlatTuner(-20) } },
            { TunedAttribute.Spd, new List<Tuner>(){ new FlatTuner(60), new MaxPercentageTuner(100) } }
        },
        Operations = new Dictionary<State, List<Operation>>()
        {
            { State.Hp, new List<Operation>(){ new NowPercentageOperation(-15) } },
        },
        Turns = 3
    };
    TurnCondition t2 = new()
    {
        Name = "Poisoned",
        Tuners = new Dictionary<TunedAttribute, List<Tuner>>(),
        Operations = new Dictionary<State, List<Operation>>()
        {
            { State.Hp, new List<Operation>(){ new FlatOperation(-20) }  }
        },
        Turns = 3
    };
    Unit c1 = new(50, 20, 300);
    c1.TurnConditions.Add(t1);
    Console.WriteLine(c1.Atk);
    Console.WriteLine(c1.Spd);
    Console.WriteLine(c1.Hp);
    static void NextTurn(ITurnObject turnObject)
    {
        turnObject.NextTurn();
    }
    NextTurn(c1);
    NextTurn(c1);
    NextTurn(c1);
    Console.WriteLine(c1.Atk);
    Console.WriteLine(c1.Spd);
    Console.WriteLine(c1.Hp);
    Console.WriteLine("Hello, World!");
}
Main();
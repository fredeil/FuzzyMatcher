using System;
using System.Collections.Generic;
using FuzzyMatch;

namespace csharp
{
    class Program
    {
        static void Main(string[] args)
        {
            var availableCommands = new [] { "stash", "diff", "commit" };
            Console.WriteLine("\nAvailable commands: {0}\n", string.Join(", ", availableCommands));

            var input = Console.ReadLine();
            var match = FuzzyMatcher.Match(availableCommands, input);
            Console.WriteLine($"Did you mean '{match}'?\n");
        }
    }
}

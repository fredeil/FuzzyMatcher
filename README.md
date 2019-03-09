# Fuzzy-match

Fuzzy-match algorithm using the _Jaro Winkler distance_ to measure
the similarity between two strings.

## Example

See Program.fs

```fsharp
namespace FuzzyMatch

module main =
    open FuzzyMatcher

    [<EntryPoint>]
    let main _ =

        let availableCommands = ["status"; "commit"; "stash"; "diff"; "all"]

        // Example use case: CLI user wanting to do "git stash",
        // user actually writes "git shtas", 
        // the algorithm returns "stash" as the best suggestion for the command
        let suggestCommand = availableCommands |> fuzzyMatch

        "shtas" |> suggestCommand |> printfn "Did you mean %A?"
        0
```

See csharp/Program.cs

```csharp
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
            
            Console.WriteLine("Available commands: {0}", string.Join(", ", availableCommands));

            var input = Console.ReadLine();
            var match = FuzzyMatcher.Match(availableCommands, input);
            
            Console.WriteLine($"Did you mean '{match}'?\n");
        }
    }
}

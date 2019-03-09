namespace FuzzyMatch

module main =

    open FuzzyMatcher

    [<EntryPoint>]
    let main _ =

        let availableCommands = ["status"; "commit"; "stash"; "diff"; "all"]

        // Example use case: CLI user wanting to do "git stash",
        // user actually writes "git shtas", the algorithm returns "stash" as the best suggestion for the command
        let suggestCommand = availableCommands |> fuzzyMatch

        "shtas" |> suggestCommand |> printfn "Did you mean %A?"
        0

namespace FuzzyMatch
open System

module FuzzyMatcher =
    let inline private existsWin mChar str offset radius =
        let pos = max 0 (offset - radius)
        let endPos = min (offset + radius) (String.length str - 1)
        if endPos - pos < 0 then false else
            let rec exists index =
                if str.[index] = mChar then true elif index = endPos then false
                else exists (index + 1)
            pos |> exists

    let inline private jaroScore (s1, s2) =
        let matchRadius =
            let minLen = min (String.length s1) (String.length s2)
            minLen / 2 + minLen % 2
            
        let common (s1 : string, s2 : string) =
            let rec inner i result =
                match i with
                | -1 -> result
                | _ -> if existsWin s1.[i] s2 i matchRadius then 
                        inner (i - 1) (s1.[i] :: result) else inner (i - 1) result
            inner (String.length s1 - 1) []

        let c1 = (s1, s2) |> common
        let c2 = (s2, s1) |> common
        let c1len = c1 |> List.length |> float
        let c2len = c2 |> List.length |> float
        let (^/) x y = float (x) / float (y)

        let transpos =
            let rec inner cl1 cl2 result =
                match cl1, cl2 with
                | [], _ | _, [] -> result
                | c1h :: c1t, c2h :: c2t ->
                    if c1h <> c2h then inner c1t c2t (result + 1.0) else inner c1t c2t result
            let mismatches = inner c1 c2 0.0
            (mismatches + abs (float c1len - float c2len)) / 2.0

        let s1length = (s1 |> String.length) |> float
        let s2length = (s2 |> String.length) |> float
        let maxLength = max c1len c2len |> float
        let result = (c1len ^/ s1length + (c2len ^/ s2length) + (maxLength - transpos) / maxLength) / 3.0
        if result |> Double.IsNaN then 0.0 else result

    let jaroDistance (s1, s2) =
        if String.length s1 = 0 || String.length  s2 = 0 then 0.0 elif s1 = s2 then 1.0
        else
            let score =
                let jaroscore = (s1, s2) |> jaroScore
                let minLength = (min s1.Length s2.Length) - 1
                let rec calcLength idx acc = if idx > minLength || s1.[idx] <> s2.[idx] then acc else calcLength (idx + 1) (acc + 1.0)
                let l = min (calcLength 0 0.0) 4.0
                jaroscore + (l * 0.1 * (1.0 - jaroscore))
            let b = if s1.[0] = s2.[0] then 1.0 else 0.0
            ((1.0 - 0.2) * score) + (0.2 * b)

    let inline private internalMatch words input =
       words |> Seq.filter(fun word -> abs (String.length word - String.length input) <= 3)
             |> Seq.map (fun word -> (word, jaroDistance (word, input))) |> Seq.sortByDescending snd

    /// Finds the best match for a given word
    [<CompiledNameAttribute("Match")>]
    let fuzzyMatch words input = internalMatch words input |> Seq.head |> fst
namespace GitAliases

open System.Collections.Generic
open Pinicola.FSharp
open System

[<RequireQualifiedAccess>]
module KeyValuePair =
    let toTuple (kv: KeyValuePair<_, _>) = kv.Key, kv.Value

[<RequireQualifiedAccess>]
module PowershellOutput =

    let private indent (s: string) =

        s.Split('\n', '\r')
        |> Seq.map (fun line ->
            if String.IsNullOrWhiteSpace line then
                ""
            else
                "  " + line
        )

    let private buildGlobalFunction (name, definition) =
        seq {
            yield $"function global:{name} {{"
            yield! indent definition
            yield "}"
            yield ""
        }

    let private buildGlobalFunctions (globalFunctions: GlobalFunctions) =
        seq {
            yield "# Global functions"
            yield! globalFunctions |> Seq.collect (KeyValuePair.toTuple >> buildGlobalFunction)
        }

    let private buildRemoveAlias (removeAlias: AliasName array) =
        seq {
            yield "# Remove aliases"

            yield!
                removeAlias
                |> Seq.map (fun alias -> $"Remove-Alias {alias} -Force -ErrorAction SilentlyContinue")

            yield ""
        }

    let private buildFunction gitCommand (name, definition) =
        let fixedGitCommand =
            definition.Command |> String.replace "${GitCommand}" gitCommand

        seq {
            yield $"function global:{name} {{"
            // TODO VRM verbose commande here
            yield! indent fixedGitCommand
            yield "}"
            yield ""
        }

    let private buildFunctions gitCommand (functions: Functions) =
        seq {
            yield "# Functions"
            yield! functions |> Seq.collect (KeyValuePair.toTuple >> (buildFunction gitCommand))
        }

    let build (config: Config) =

        seq {
            yield "Import-Module PSWriteColor"
            yield ""

            yield! buildGlobalFunctions config.Global
            yield! buildRemoveAlias config.RemoveAlias
            yield! buildFunctions config.GitCommand config.Functions
        }
        |> Seq.toList

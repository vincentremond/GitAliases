namespace GitAliases

open System.Text.Json
open Pinicola.FSharp

[<RequireQualifiedAccess>]
module JsonOutput =

    type RootModel = AliasModel array

    and AliasModel = {
        ShortCommand: string
        FullCommand: string
        Description: string
    }

    let private buildAliasModel (gitCommand: string) (name, definition) =
        let fullCommand = definition.Command |> String.replace "${GitCommand}" gitCommand

        {
            ShortCommand = name
            FullCommand = fullCommand
            Description = definition.Description
        }

    let private buildRootModel (gitCommand: string) (functions: Functions) : RootModel =
        functions
        |> Seq.map (KeyValuePair.toTuple >> (buildAliasModel gitCommand))
        |> Seq.toArray

    let build (functions: Functions) =
        let rootModel = buildRootModel "git" functions

        let options =
            JsonSerializerOptions(WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase)

        JsonSerializer.Serialize(rootModel, options)

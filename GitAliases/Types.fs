namespace GitAliases

open System.Collections.Generic

[<CLIMutable>]
type Config = {
    GitCommand: string
    Global: GlobalFunctions
    RemoveAlias: AliasName array
    Functions: Functions
}

and GlobalFunctions = Dictionary<GlobalFunctionName, GlobalFunctionDefinition>

and GlobalFunctionName = string

and GlobalFunctionDefinition = string

and AliasName = string

and Functions = Dictionary<FunctionName, FunctionDefinition>
and FunctionName = string

and [<CLIMutable>] FunctionDefinition = {
    Description: string
    Command: string
}

open System
open System.IO
open GitAliases
open Pinicola.FSharp.IO
open Pinicola.FSharp.SpectreConsole

let configContent = File.ReadAllText("aliases.yaml")

let yamlDeserializer = YamlDotNet.Serialization.Deserializer()
let config = yamlDeserializer.Deserialize<Config>(configContent)

config
|> PowershellOutput.build
|> File.writeAllText @"C:\Users\remond\repos\Perso\personalconfig\git-aliases.ps1"

AnsiConsole.markupLineInterpolated $"[green]PowerShell script generated successfully.[/] [grey]{DateTimeOffset.Now}[/]"

config.Functions
|> JsonOutput.build
|> File.writeAllText @"C:\Users\remond\TMP\2025-05-15--test-vue\VueSample\VueSample\src\json\aliases.json"

AnsiConsole.markupLineInterpolated $"[green]JSON file generated successfully.[/] [grey]{DateTimeOffset.Now}[/]"

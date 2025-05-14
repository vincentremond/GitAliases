open System
open System.IO
open GitAliases
open Pinicola.FSharp.SpectreConsole

let configContent = File.ReadAllText("aliases.yaml")

let yamlDeserializer = YamlDotNet.Serialization.Deserializer()
let config = yamlDeserializer.Deserialize<Config>(configContent)

let powershellOutput =
    config |> PowershellOutput.build |> String.concat Environment.NewLine

File.WriteAllText(@"C:\Users\remond\repos\Perso\personalconfig\git-aliases.ps1", powershellOutput)

AnsiConsole.markupLineInterpolated $"[green]PowerShell script generated successfully.[/] [grey]{DateTimeOffset.Now}[/]"

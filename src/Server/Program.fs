module Server.Program

open Suave
open System.IO
open System.Threading
open System
open Helpers
open Argu

type Arguments =
    | Home_Folder of string
    | Conntection_String of string
    interface IArgParserTemplate with
        member s.Usage =
            match s with
            | Home_Folder _ -> "Home folder"
            | Conntection_String _ -> "Conntection String"

[<EntryPoint>]
let main _ = 
    let argumentParser = ArgumentParser.Create<Arguments>()
    let arguments = argumentParser.Parse()

    let connectionString =
        match arguments.TryGetResult <@ Conntection_String @> with
        | Some c -> c
        | _ ->
            let cs = Configuration.ConfigurationManager.ConnectionStrings.["ConnectionString"]
            if cs |> isNull then
                invalidOp "Connection string is missing"
            else
                cs.ConnectionString

    let repository = RepositorySql.Adapter.create connectionString
    let api = Api.Adapter.create repository 

    use cts = new CancellationTokenSource()

    let conf =
        { defaultConfig with
            homeFolder = Path.Combine(exeFolder, defaultArg (arguments.TryGetResult <@ Home_Folder @>) "wwwroot" ) |> normalizePath |> Some
            cancellationToken = cts.Token }

    let _, server = startWebServerAsync conf (App.appHandler api)
    Async.Start(server, cts.Token)

    Console.ReadKey() |> ignore

    0

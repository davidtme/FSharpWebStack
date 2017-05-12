#r "packages/FAKE/tools/FakeLib.dll"

open Fake
open Fake.AssemblyInfoFile
open Fake.Testing
open System
open Fake.NpmHelper
open System.IO
open System.Data.SqlClient

let buildDir = "./Build"
let artifactsDir = "./Artifacts"

let dotNetCoreRestore path =
    let dotnetPath = DotNetCli.InstallDotNetSDK "1.0.1"
    DotNetCli.DotnetSDKPath <- Path.GetDirectoryName dotnetPath

    DotNetCli.Restore 
        (fun p -> { p with WorkingDir = path })

let npmInstall path =
    Npm (fun p ->
        { p with
            Command = Install Standard
            WorkingDirectory = path })

let fable path cmd =
    DotNetCli.RunCommand 
        (fun p -> { p with WorkingDir = path })
        ("fable npm-run " + cmd)

Target "Client" <| fun _ ->
    let path =  "./src/Client"
    npmInstall path
    dotNetCoreRestore path 
    fable path "build"

Target "Client.Tests" <| fun _ ->
    let path =  "./src/Client.Tests"
    npmInstall path
    dotNetCoreRestore path 
    
    match environVarOrDefault "TEAMCITY_PROJECT_NAME" "" with
    | "" -> "test"
    | _ -> "testteamcity"
    |> fable path

Target "Database" <| fun _ ->
    let path = "./src/Database/Database.sqlproj"
    build id path 

Target "Server" <| fun _ ->
    let path = "./src/Server/Server.fsproj"
    build id path 

Target "Server.Tests" <| fun _ ->
    let path = "./src/Server.Tests/Server.Tests.fsproj"
    let outputDir = buildDir </> "Server.Tests"
    build id path
    Expecto.Expecto id [ outputDir </> "Server.Tests.exe" ]

Target "ServerPackage" <| fun _ ->
    Paket.Pack(fun p ->
        { p with
            OutputPath = artifactsDir
            TemplateFile = "./src/Server/paket.template"
        })

Target "All" ignore

"Client"
==> "Client.Tests"
==> "All"

"Client"
==> "Database"
==> "Server"
==> "Server.Tests"
==> "ServerPackage"
==> "All"

RunTargetOrDefault "All"

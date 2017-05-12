#r "../../packages/Microsoft.SqlServer.Dac/lib/Microsoft.Data.Tools.Schema.Sql.dll"
#r "../../packages/Microsoft.SqlServer.Dac/lib/Microsoft.SqlServer.Dac.dll"
#r "System.IO.Compression"
#r "System.IO.Compression.FileSystem"

open System.Data.SqlClient
open System.IO
open System.IO.Compression
open Microsoft.SqlServer.Dac
open System

let databaseName = "SampleData_4F4C89C2"

let database = Path.Combine(__SOURCE_DIRECTORY__, "..", "..", "Temp", databaseName + ".mdf")
let dacpac =
    Environment.GetCommandLineArgs().[2]
    |> fun p ->
        if p.Contains(@":\") then p
        else Path.Combine(__SOURCE_DIRECTORY__, p)
    |> fun p ->
        Path.Combine(p, "Database.dacpac")

let connectionString =
    @"Data Source=(localdb)\MSSQLLocalDB; Integrated Security=True"

let hasChanged () =
    let hashFile = Path.ChangeExtension(database, ".md5")

    use archive = ZipFile.OpenRead(dacpac)
    let model =
        archive.Entries
        |> Seq.pick(fun i -> if i.FullName.EndsWith("model.xml") then Some i else None)

    use stream = model.Open()
    let hash =
        use hashImp = System.Security.Cryptography.HashAlgorithm.Create("MD5")
        let h = hashImp.ComputeHash(stream);
        BitConverter.ToString(h).Replace("-", String.Empty)

    let writeHash () =
        Directory.CreateDirectory(Path.GetDirectoryName(hashFile)) |> ignore
        File.WriteAllText(hashFile, hash)

    if not <| File.Exists(hashFile) then
        writeHash()
        true
    else
        let last = File.ReadAllText(hashFile)

        if last = hash then
            false
        else
            writeHash()
            true


let deleteDatabase () =
    printfn "Deleting database"

    if File.Exists(database) then

        let name = Path.GetFileNameWithoutExtension(database)
        use conn = new SqlConnection(connectionString)
        use cmd =
            new SqlCommand(
                "USE master;
                 IF EXISTS(SELECT * FROM master.dbo.sysdatabases WHERE name = '"+name+"')
                 BEGIN
                    ALTER DATABASE ["+name+"] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                    EXEC sp_detach_db @dbname = '"+name+"'
                 END", conn)

        conn.Open()
        cmd.ExecuteNonQuery() |> ignore
        conn.Close()

        File.Delete(database)

    if File.Exists(Path.ChangeExtension(database, "ldf")) then File.Delete(Path.ChangeExtension(database, "ldf"))

let createDatabase () =
    printfn "Creating database"

    Directory.CreateDirectory(Path.GetDirectoryName(database)) |> ignore

    let name = Path.GetFileNameWithoutExtension(database)
    use conn = new SqlConnection(connectionString)
    use cmd =
        new SqlCommand(
            "USE master;
             CREATE DATABASE "+name+"
             ON
             ( NAME = Sales_dat,
               FILENAME = '"+database+"')
             LOG ON
             ( NAME = Sales_log,
               FILENAME ='"+Path.ChangeExtension(database, "ldf")+"')", conn)

    conn.Open()
    cmd.ExecuteNonQuery() |> ignore
    conn.Close()


let applyDacpac () =
    printfn "Appling dacpac"

    let name = Path.GetFileNameWithoutExtension(database)
    let dacPackage = DacPackage.Load(dacpac)

    let dacDeployOptions = DacDeployOptions()
    dacDeployOptions.BlockOnPossibleDataLoss <- false
    dacDeployOptions.GenerateSmartDefaults <- true

    let dacServices = DacServices(connectionString + ";Initial Catalog=" + name)
    dacServices.Deploy(dacPackage, name, true, dacDeployOptions)


if hasChanged() || not (File.Exists(database)) then
    deleteDatabase()
    createDatabase()
    applyDacpac()
    printfn "Dacpac updated"
else
    printfn "Dacpac not changed"
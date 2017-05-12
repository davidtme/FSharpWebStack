module RepositorySql.User

open FSharp.Data
open Common

// Create a very simple but strongly typed & error checked sql query.
let search (connectionString : string) =
    seq {
        use cmd = new SqlCommandProvider<"
            SELECT Id, EmailAddress, Name
            FROM Users", CompileConnectionString>(connectionString)

        yield! cmd.Execute()
        |> Seq.map(fun row ->
            { Id = row.Id
              EmailAddress = row.EmailAddress
              Name = row.Name } : Models.User
        )
    }
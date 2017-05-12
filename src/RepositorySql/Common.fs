module RepositorySql.Common

open FSharp.Data.Sql

[<Literal>]
let internal CompileConnectionString =
    @"Data Source=(localdb)\MSSQLLocalDB; Integrated Security=True;Initial Catalog=SampleData_4F4C89C2;"

type SampleDataDb = SqlDataProvider<ConnectionString = CompileConnectionString, UseOptionTypes = true>
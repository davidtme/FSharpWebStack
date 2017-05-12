module RepositorySql.Adapter

open Common.Repository

let create connectionString : SampleDataRepository =
    { User = 
        { Search = fun _ -> User.search connectionString }
      Product = 
        { Search = fun _ -> Product.search connectionString }}

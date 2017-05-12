module Server.Api.Adapter

open Common

let create (repository : Repository.SampleDataRepository) : Api.SampleDataApi =
    { User = 
        { Search = fun _ -> User.search repository }
      Product = 
        { Search = fun _ -> Product.search repository } }
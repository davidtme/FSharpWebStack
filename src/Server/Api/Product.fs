module Server.Api.Product

let search (repository : Common.Repository.SampleDataRepository) =
    async {
        return repository.Product.Search () |> Seq.toList |> Result.Ok
    }
    
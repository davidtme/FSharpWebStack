module Server.Api.User

let search (repository : Common.Repository.SampleDataRepository) =
    async {
        return repository.User.Search () |> Seq.toList |> Result.Ok
    }
    
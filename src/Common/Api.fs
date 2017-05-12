namespace Common.Api

open Common

type ApiResult<'a> = Async<Result<'a, string>>

type User =
    { Search : unit -> ApiResult<Models.User list> }

type Product =
    { Search : unit -> ApiResult<Models.Product list> }

type SampleDataApi =
    { User : User
      Product : Product }
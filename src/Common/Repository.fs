namespace Common.Repository

open Common

type Product = 
    { Search : unit -> Models.Product seq }

type User = 
    { Search : unit -> Models.User seq }

type SampleDataRepository =
    { Product : Product
      User : User }
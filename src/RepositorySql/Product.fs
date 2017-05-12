module RepositorySql.Product

open Common

// Create a complex query with some ifs, wheres and sorts
// This uses the F# SqlDataProvider
let search (connectionString : string) =
    seq {
        let testFilter = true;
        let ctx = SampleDataDb.GetDataContext(connectionString)

        yield!
            query {
                for product in ctx.Dbo.Products do
                select product
            }

            |> fun searchQuery ->
                if testFilter then 
                    // Show how to filter.
                    query {
                        for product in searchQuery do
                        where (product.Name <> "Test")
                    }
                else 
                    searchQuery

            |> fun searchQuery -> 
                query {
                    for product in searchQuery do
                    select ({ Id = product.Id 
                              Name = product.Name } : Models.Product)
                }

    }
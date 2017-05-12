module Helpers

open ClientModels
open Elmish.Browser.UrlParser
open Elmish.Browser.Navigation

let toHash = 
    function
    | PageRequest.Home -> "#/home"
    | PageRequest.UserSearch -> "#/user" 
    | PageRequest.ProductSearch -> "#/product" 

let pageParser : Parser<PageRequest->_,PageRequest> =  
    oneOf [ map PageRequest.Home (s "home")
            map PageRequest.UserSearch (s "user")
            map PageRequest.ProductSearch (s "product") ]
    
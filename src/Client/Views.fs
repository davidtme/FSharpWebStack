module Views

open Fable.Core
open Fable.Import
open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fable.Core.JsInterop
open Common
open ClientModels
open Helpers
open Themes

module UI = UIComponents  

// ==================================================
// Layout
// ==================================================

let mainMenu model dispatch = 
    themed <| fun theme ->
    div [ Style [ BackgroundColor theme.Colors.Primary.Base ] ] [
        UI.button
            { UI.buttonDefaults with
                Mode = UI.ButtonMode.Primary
                Link = PageRequest.Home |> toHash |> Some
                Text = Some "Home" }
        UI.button
            { UI.buttonDefaults with
                Mode = UI.ButtonMode.Primary
                Link = PageRequest.UserSearch |> toHash |> Some
                Text = Some "Users" }
        UI.button
            { UI.buttonDefaults with
                Mode = UI.ButtonMode.Primary
                Link = PageRequest.ProductSearch |> toHash |> Some
                Text = Some "Products" } ]

let currentRequests model dispatch =
    div [ Style [ Position "absolute"
                  Top "3px"
                  Right "3px"
                  Transition "0.5s ease-in-out opacity"
                  CSSProp.Opacity !!(if model.Requests.Length > 0 then 1 else 0) ] ] [
        UI.loader { UI.loaderDefaults with Size = 30 } ]   

// ==================================================
// Pages
// ==================================================

let webUserSearch (model : UserSearchPage) dispatch = 
    UI.table
        { UI.tableDefaults with
            Headings =
                [ str "Id"
                  str "Name"]
            Rows = 
                model.Items
                |> List.map(fun item -> 
                    [ str (string item.Id)
                      str item.Name ]) }

let accountSearch (model : ProductSearchPage) dispatch = 
    UI.table
        { UI.tableDefaults with
            Headings =
                [ str "Id"
                  str "Name"]
            Rows = 
                model.Items
                |> List.map(fun item -> 
                    [ str (string item.Id)
                      str item.Name ]) }

// ==================================================
// Main View
// ==================================================

let view model dispatch =
    UI.themeContext theme [
        mainMenu model dispatch
        div [] [
            currentRequests model dispatch
            (match model.Page with
            | Page.Home -> 
                div [] [ str "Home" ]

            | Page.UserSearch model -> 
                webUserSearch model dispatch 

            | Page.ProductSearch model -> 
                accountSearch model dispatch 

            ) ] ]       
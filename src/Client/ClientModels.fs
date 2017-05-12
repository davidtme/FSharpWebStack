namespace ClientModels

open Common

type RemoteItem<'a> =
    | Loading
    | Loaded of 'a

// ==================================================
// Pages
// ==================================================

type PageRequest = 
    | Home
    | UserSearch
    | ProductSearch

type UserSearchPage =
    { Items : Models.User list }

type ProductSearchPage =
    { Items : Models.Product list }    

type Page =
    | Home
    | UserSearch of UserSearchPage  
    | ProductSearch of ProductSearchPage  

// ==================================================
// Messages
// ==================================================

type ServerResponce =
    | UserSearch of  Models.User list
    | ProductSearch of  Models.Product list

type Message =
    | RequestStarted of requestId : System.Guid * pageRequest : PageRequest
    | RequestCompleted of requestId : System.Guid * pageRequest : PageRequest * serverResponce : ServerResponce
    | Noop
    
type State = 
    { RequestedPage : PageRequest
      Page : Page
      Requests : System.Guid list }      
module UIComponents

open Fable.Core
open Fable.Core.JsInterop
open Fable.Import
open Fable.Helpers.React
open Fable.Helpers.React.Props

let private propTypes : obj = importDefault "prop-types"

let inline private Hover (css: ICSSProp list) =
    !!(":hover", keyValueList CaseRules.LowerFirst css)

// ==================================================
// Theme Context
// ==================================================

[<Pojo>]
type ThemeContextProps =
    { Theme : obj }

[<Literal>]
let ThemeContextName = "_Theme"

type ThemeContext(props) =
    inherit React.Component<ThemeContextProps, obj>(props)
        
    member __.getChildContext() =
        createObj [ ThemeContextName ==> props ]

    static member childContextTypes =
        createObj [ ThemeContextName ==> propTypes?object ]

    member this.render() =
        div [  ] [
            !!this.children ]

let themeContext theme children =
    com<ThemeContext, _, _> { Theme = theme } children

// ==================================================
// Themed
// ==================================================

type private ThemedComponent<[<Pojo>]'P, [<Pojo>]'S>(initialProps: 'P) =
    inherit React.Component<'P, 'S>(initialProps)
    
    static member val contextTypes =
        createObj [ ThemeContextName ==> propTypes?object ] with get, set

    member this.Theme =
        (!!this?context?_Theme : ThemeContextProps).Theme

[<Pojo>]
type private ThemedProps = 
    { Handler : obj -> React.ReactElement  }

type private Themed(initialProps) =
    inherit React.Component<ThemedProps, obj>(initialProps)
    
    static member val contextTypes =
        createObj [ ThemeContextName ==> propTypes?object ] with get, set

    member this.render () =
        let t = (!!this?context?_Theme : ThemeContextProps).Theme
        this.props.Handler t

let themed fn =
    com<Themed, _, _> { Handler = fn } []

// ==================================================
// Radium
// ==================================================

[<Pojo>]
type RadiumProps = 
    { Handler : unit -> React.ReactElement }

type Radium(initialProps) =
    inherit React.Component<RadiumProps, obj>(initialProps)
    
    member this.render () =
        this.props.Handler ()  

let private radiumHookup comp = importDefault "radium"
let radium =
    let comp = !!(radiumHookup typedefof<Radium>)
    fun fn -> from comp { Handler = fn } []   

// ==================================================
// Loader
// ==================================================

[<Pojo>]
type LoaderProps = 
    { Size : int
      Color : string
      ColorFaded : string }

let loaderDefaults =
    { Size = 20
      Color = "red"
      ColorFaded = "blue" }

type ILoaderTheme =
    abstract member LoaderTheme: LoaderProps -> LoaderProps

type private Loader(initialProps) =
    inherit ThemedComponent<LoaderProps, obj>(initialProps)

    let svg = """<?xml version="1.0" encoding="utf-8"?>
<svg class="uil-ring-alt" height="{2}px" width="{2}px" preserveaspectratio="xMidYMid" viewbox="0 0 100 100"  xmlns="http://www.w3.org/2000/svg">
	<rect class="bk" fill="none" height="100" width="100" x="0" y="0"></rect>
	<circle cx="50" cy="50" fill="none" r="40" stroke="{0}" stroke-linecap="round" stroke-width="20"></circle>
	<circle cx="50" cy="50" fill="none" r="40" stroke="{1}" stroke-linecap="round" stroke-width="20">
		<animate attributename="stroke-dashoffset" dur="2s" from="0" repeatcount="indefinite" to="502"></animate>
		<animate attributename="stroke-dasharray" dur="2s" repeatcount="indefinite" values="150.6 100.4;1 250;150.6 100.4"></animate>
    </circle>
</svg>"""

    member this.render () =
        let props =
            match this.Theme with
            | :? ILoaderTheme as t -> t.LoaderTheme this.props
            | _ -> this.props

        let html = System.String.Format(svg, props.Color, props.ColorFaded, props.Size)

        span [ Style [ CSSProp.Width (props.Size.ToString() + "px")
                       CSSProp.Height (props.Size.ToString() + "px")
                       Display "inline-block" ]
               DangerouslySetInnerHTML (createObj ["__html" ==> html]) ] []    

    member this.shouldComponentUpdate (nextProps : LoaderProps) _ =
        this.props.Size <> nextProps.Size

let loader =
    let comp = !!(radiumHookup typedefof<Loader>)
    fun (props : LoaderProps) -> from comp props []
    
// ==================================================
// Table
// ==================================================

[<Pojo>]
type TableProps =
    { Headings : React.ReactElement list
      Rows : (React.ReactElement list) list
      HeadingColor : string
      HeadingBackground : string
      Background: string
      BorderColor : string }

let tableDefaults =
    { Headings = []
      Rows = []
      HeadingColor = "red"
      HeadingBackground = "blue"
      Background = "white"
      BorderColor = "green" }

type ITableTheme =
    abstract member TableTheme: TableProps -> TableProps

let table (props : TableProps) =
    themed <| fun theme ->

        let props =
            match theme with
            | :? ITableTheme as t -> t.TableTheme props
            | _ -> props

        table [ CellSpacing !!0.
                CellPadding !!0.
                Style [ BorderCollapse "collapse"
                        Margin "0"
                        Border "0" ] ] [
            // Header
            (if props.Headings.Length > 0 then
                thead [ Style [ BackgroundColor props.HeadingBackground
                                Color props.HeadingColor ] ] [
                    
                    tr []
                        (props.Headings
                        |> List.map(fun h -> 
                            th [ Style [ TextAlign "left"
                                         Padding "5px"
                                         FontWeight !!"normal" ] ] [ h ] )) ]
             else !!null)
                
            // Rows
            tbody [ Style [ BackgroundColor props.Background ] ]
                (props.Rows
                 |> List.mapi(fun i row -> 
                    tr [ Style [ BorderTopColor props.BorderColor
                                 BorderTopStyle "solid"
                                 BorderTopWidth (if i > 0 then "1px" else "0")] ] 
                        (row |> List.map(fun col -> td [ Style [ Padding "5px" ] ] [ col ] )))) ]

// ==================================================
// Button
// ==================================================

type ButtonMode =
    | Default = 0
    | Primary = 1
    | Secondary = 2

type ButtonProps =
    { Mode : ButtonMode
      Link : string option
      Text : string option
      Color : string
      Background : string
      HoverBackground : string }

let buttonDefaults =
    { Mode = ButtonMode.Default
      Link = None
      Text = None
      Color = "white"
      Background = "blue"
      HoverBackground = "red" }

type IButtonTheme =
    abstract member ButtomTheme: ButtonProps -> ButtonProps

let button props =
    themed <| fun theme ->
    radium <| fun _ ->

    let props =
        match theme with
        | :? IButtonTheme as t -> t.ButtomTheme props
        | _ -> props
    
    a ([ Style [ Display "inline-block"
                 Color props.Color
                 BackgroundColor props.Background
                 Padding "10px 10px"
                 BorderTopLeftRadius "2px"
                 BorderTopRightRadius "2px"
                 BorderBottomLeftRadius "2px"
                 BorderBottomRightRadius "2px"
                 TextDecoration "none"
                 Transition "0.1s ease background-color"
                 Hover [ BackgroundColor props.HoverBackground ] ] ]
        @ match props.Link with Some l -> [Href l] | _ -> []) [
        str (defaultArg props.Text "") ]
    
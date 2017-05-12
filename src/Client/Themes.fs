module Themes

module UI = UIComponents  

type ColorGroup =
    { /// Used for main body background and button backgrounds.
      Base : string 
      /// Base color but a bit lighter.
      Light : string 
      /// Base color but a bit darker.
      Dark : string
      /// Used for text colors and button backgrounds.
      Overlay : string 
      /// Used for button backgrounds when hovering 
      Alternative : string }

type Colors = 
    { Default : ColorGroup
      Primary : ColorGroup
      Secondary : ColorGroup
      Tertiary : ColorGroup
      Error : ColorGroup }

type Theme =
    { Colors : Colors
      FontFamily : string
      FontSize : string }

    interface UI.IButtonTheme with
        member x.ButtomTheme props = 
            let colorGroup = 
                match props.Mode with
                | UI.ButtonMode.Primary -> x.Colors.Primary
                | UI.ButtonMode.Secondary -> x.Colors.Secondary
                | _ -> x.Colors.Default

            { props with 
                Color = colorGroup.Overlay
                Background = colorGroup.Base
                HoverBackground = colorGroup.Alternative }

    interface UI.ILoaderTheme with
        member x.LoaderTheme props = 
            { props with
                Color = x.Colors.Primary.Alternative
                ColorFaded = x.Colors.Primary.Dark }

    interface UI.ITableTheme with
        member x.TableTheme props = 
            { props with
                HeadingColor = x.Colors.Tertiary.Overlay
                HeadingBackground = x.Colors.Tertiary.Dark
                Background = x.Colors.Default.Base
                BorderColor = x.Colors.Tertiary.Light }                
            

let themed fn = 
    UI.themed <| fun theme -> fn ((unbox theme) : Theme)                

let theme : Theme = 
    let defaultColorGroup : ColorGroup = { Base = "#eef0f3"; Light = "#ffffff"; Dark = "#bcbec0"; Overlay = "#323a45"; Alternative = "#2dcc70" }
    
    { FontFamily = """"Helvetica Neue", Helvetica, Arial, "Lucida Grande", sans-serif"""
      FontSize = "14px"
      Colors =
        { Primary = { Base = "#3497db"; Light = "#73c7ff"; Dark = "#0069a9"; Overlay = "#ffffff"; Alternative = "#2dcc70" }
          Secondary = { Base = "#2dcc70"; Light = "#6eff9f"; Dark = "#009a43"; Overlay = "#ffffff"; Alternative = "#28b864" }
          Tertiary = { Base = "#323a45"; Light = "#5c6470"; Dark = "#0c141e"; Overlay = "#ffffff"; Alternative = "#2dcc70" }
          Default = defaultColorGroup
          Error = defaultColorGroup } }    
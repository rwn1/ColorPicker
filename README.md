# 🎨 ColorPicker Core
`ColorPicker.Core` is a universal, multi-platform color-processing library, with UI implementations available for *WPF* (`ColorPicker.View.Wpf`), *.NET MAUI* (`ColorPicker.View.Maui`) and *Blazor* (`ColorPicker.View.Blazor`). Additional UI implementations may be added in the future (a *WinUI* version is currently being planned).
This implementation solves the issue where each technology uses its own color-related classes.

<img width="721" height="233" alt="image" src="https://github.com/user-attachments/assets/a8656ea9-276d-45f6-b45c-d0b723d96ba7" />

## 🧪 Testing
`ColorPicker.Core` contains all the main logic, algorithms, and color conversion routines.
`ColorPicker.Core.Tests` uses *xUnit* to verify correctness of calculations, property updates, and other core functionality independent of the UI.

# WPF Color Picker
<img width="148" height="147" src="https://github.com/user-attachments/assets/75d27d9d-6349-4b36-b8ef-c2e11c017bcd" />

The *WPF* implementation supports both legacy *.NET Framework* applications as well as modern *.NET*, allowing the control to be used across a wide range of *WPF* projects.
The control is designed for flexible and precise color selection, suitable for both simple and advanced use cases. It is composed of multiple template parts (PART_*), allowing developers to fully customize its layout and behavior through *ControlTemplate*.

## 🔧 Installation
The core library `ColorPicker.Core` and the *WPF* UI implementation `ColorPicker.View.Wpf` (including `ColorPicker.Core`) are available as NuGet packages:

```powershell
dotnet add package ColorPicker.Core
```
```powershell
dotnet add package ColorPicker.View.Wpf
```

## 🧬 Control template parts (PART_*)
This *Control* is composed of several *WPF* template parts.
When creating a custom *ControlTemplate*, not all template parts are required — the control is designed to function even if some are omitted.

<img width="936" height="460" src="https://github.com/user-attachments/assets/9ba3e0ed-75b8-433e-a7e9-9cd1c16570c0" />


The control allows you to customize the visualization of its individual parts.
When a *ControlTemplate* is applied, not all parts have to be included.

The final color is selected through *PART_ColorSelectionView*, which visualizes the current Hue value.
The Hue itself is adjusted using *PART_HueSelectionView*. This control can automatically adapt to the available space — if its width is greater than its height, the value will be adjusted vertically.
The same adaptive behavior is used by PART_AlphaSelectionView, which is responsible for adjusting opacity.

The selected color is displayed in *PART_SelectedColorView*.

For adjusting individual components of *RGB*, *HSL*, *HSV*, and *CMYK*, you can use the predefined *TextBoxes* and *Sliders*.

It is also possible to use *PART_EyedropperButton* to pick a color from the screen.

The resulting color can be bind to your own *ViewModel* via the *SelectedColor* property exposed by the *ColorPicker*, allowing you to extend or customize the existing solution.

## 🧩 Implementations
A simple, minimalist implementation that avoids the need for manual value entry.
This setup is entirely mouse-driven and includes only the essential template parts required to select a color.

It uses only the core template elements for hue, alpha and color selection, without additional sliders, numeric inputs, or hex input. This example is ideal for applications that require a compact and lightweight color picker with a minimal UI footprint.

The control can be easily extended by adding more template parts as needed — such as sliders for fine-tuning numeric values, *TextBox* inputs for precise color entry, alpha/transparency controls, or additional visual indicators. These elements can be freely restyled or rearranged using a custom *ControlTemplate* to match the design of your application.

<img width="361" height="261" alt="image" src="https://github.com/user-attachments/assets/8637b0f8-9ceb-4d52-a8fe-63ae1a5e7c0b" />

```xml
xmlns:controls="clr-namespace:ColorPicker.View.Wpf;assembly=ColorPicker.View.Wpf"
```
```xml
<controls:ColorPicker SelectedColor="{Binding SelectedColor, Mode=TwoWay}"  Margin="4">
    <controls:ColorPicker.Style>
        <Style TargetType="{x:Type controls:ColorPicker}">
            <Setter Property="Template">
                <Setter.Value>
                    <ControlTemplate TargetType="{x:Type controls:ColorPicker}">

                        <Grid>
                            <Grid.ColumnDefinitions>
                                <ColumnDefinition Width="*"/>
                                <ColumnDefinition Width="26"/>
                                <ColumnDefinition Width="26"/>
                            </Grid.ColumnDefinitions>
                            <Grid.RowDefinitions>
                                <RowDefinition Height="*"/>
                                <RowDefinition Height="26"/>
                            </Grid.RowDefinitions>

                            <!--Color selection-->
                            <Canvas x:Name="PART_ColorSelectionView" />

                            <!--Hue selection-->
                            <Canvas x:Name="PART_HueSelectionView" Grid.Column="1" Margin="3,0" />

                            <!--Alpha selection-->
                            <Canvas x:Name="PART_AlphaSelectionView" Grid.Column="2" Margin="3,0" />

                            <!--Selected color-->
                            <Border x:Name="PART_SelectedColorView" Margin="0,3" Grid.Row="1" />

                            <!--Eye dropper-->
                            <Button x:Name="PART_EyedropperButton" Grid.ColumnSpan="2" Margin="3" Grid.Column="1" Grid.Row="1" />

                        </Grid>

                    </ControlTemplate>
                </Setter.Value>
            </Setter>
        </Style>
    </controls:ColorPicker.Style>
</controls:ColorPicker>
```

This setup can be more complex and may include additional template parts for *RGB*, *HSL*, *HSV*, *CMYK*, and *Hex*. These values can be adjusted through the *TextBox* inputs.

This advanced layout is designed for applications that require detailed color manipulation, such as graphic tools, editors, or any scenario where precise color representation matters.

Like all other template variations, each part can be freely styled, rearranged, replaced, or omitted using a custom *ControlTemplate*. Developers can choose which color models to expose and can combine or remove UI elements depending on the needs of their application.

<img width="583" height="383" src="https://github.com/user-attachments/assets/6d3de72b-aa9d-49e8-96d9-636d6b0060ac" />

```xml
xmlns:controls="clr-namespace:ColorPicker.View.Wpf;assembly=ColorPicker.View.Wpf"
```
```xml
<controls:ColorPicker SelectedColor="{Binding SelectedColor, Mode=TwoWay}"  Margin="4">
    <controls:ColorPicker.Style>
        <Style TargetType="{x:Type controls:ColorPicker}">
            <Setter Property="Template">
                <Setter.Value>
                    <ControlTemplate>
                        <Grid>
                            <Grid.Resources>
                                <Style TargetType="{x:Type TextBox}">
                                    <Setter Property="BorderThickness" Value="0"/>
                                    <Setter Property="AutomationProperties.HelpText" Value="Valid" />
                                    <Style.Triggers>
                                        <Trigger Property="Validation.HasError" Value="True">
                                            <Setter Property="AutomationProperties.HelpText" Value="Error" />
                                        </Trigger>
                                    </Style.Triggers>
                                </Style>
                                <Style TargetType="{x:Type Label}">
                                    <Setter Property="Padding" Value="0"/>
                                    <Setter Property="Content" Value=","/>
                                </Style>
                            </Grid.Resources>
                            <Grid.RowDefinitions>
                                <RowDefinition Height="*"/>
                                <RowDefinition Height="26"/>
                                <RowDefinition Height="26"/>
                                <RowDefinition Height="Auto"/>
                                <RowDefinition Height="Auto"/>
                            </Grid.RowDefinitions>
                            <Grid.ColumnDefinitions>
                                <ColumnDefinition Width="*"/>
                                <ColumnDefinition Width="3*"/>
                            </Grid.ColumnDefinitions>

                            <!--Selected color-->
                            <Border x:Name="PART_SelectedColorView" Margin="0,0,3,0"/>
                            
                            <!--Color selection-->
                            <Canvas x:Name="PART_ColorSelectionView" Grid.Column="1" />

                            <!--Hue selection-->
                            <Canvas x:Name="PART_HueSelectionView" Grid.Row="1" Grid.ColumnSpan="2" Margin="0,3" />

                            <!--Alpha selection-->
                            <Canvas x:Name="PART_AlphaSelectionView" Grid.Row="2" Grid.ColumnSpan="2" Margin="0,3" />

                            <!--HEX-->
                            <GroupBox Header="HEX" Grid.Row="3" Grid.ColumnSpan="2">
                                <Grid>
                                    <Grid.ColumnDefinitions>
                                        <ColumnDefinition Width="*"/>
                                    </Grid.ColumnDefinitions>

                                    <TextBox x:Name="PART_HexTextBox" />
                                </Grid>
                            </GroupBox>

                            <Grid Grid.Row="4" Grid.ColumnSpan="2">
                                <Grid.ColumnDefinitions>
                                    <ColumnDefinition Width="*"/>
                                    <ColumnDefinition Width="*"/>
                                    <ColumnDefinition Width="*"/>
                                    <ColumnDefinition Width="*"/>
                                </Grid.ColumnDefinitions>

                                <!--RGB-->
                                <GroupBox Header="RGB">
                                    <Grid>
                                        <Grid.ColumnDefinitions>
                                            <ColumnDefinition Width="Auto"/>
                                            <ColumnDefinition Width="Auto"/>
                                            <ColumnDefinition Width="Auto"/>
                                            <ColumnDefinition Width="Auto"/>
                                            <ColumnDefinition Width="*"/>
                                        </Grid.ColumnDefinitions>

                                        <TextBox x:Name="PART_RedTextBox"/>
                                        <Label Grid.Column="1"/>
                                        <TextBox x:Name="PART_GreenTextBox" Grid.Column="2"/>
                                        <Label Grid.Column="3"/>
                                        <TextBox x:Name="PART_BlueTextBox" Grid.Column="4"/>
                                    </Grid>
                                </GroupBox>

                                <!--CMYK-->
                                <GroupBox Header="CMYK" Grid.Column="1">
                                    <Grid>
                                        <Grid.ColumnDefinitions>
                                            <ColumnDefinition Width="Auto"/>
                                            <ColumnDefinition Width="Auto"/>
                                            <ColumnDefinition Width="Auto"/>
                                            <ColumnDefinition Width="Auto"/>
                                            <ColumnDefinition Width="Auto"/>
                                            <ColumnDefinition Width="Auto"/>
                                            <ColumnDefinition Width="*"/>
                                        </Grid.ColumnDefinitions>

                                        <TextBox x:Name="PART_CyanTextBox" />
                                        <Label Grid.Column="1"/>
                                        <TextBox x:Name="PART_MagentaTextBox" Grid.Column="2"/>
                                        <Label Grid.Column="3"/>
                                        <TextBox x:Name="PART_YellowTextBox" Grid.Column="4"/>
                                        <Label Grid.Column="5"/>
                                        <TextBox x:Name="PART_KeyTextBox" Grid.Column="6"/>
                                    </Grid>
                                </GroupBox>

                                <!--HSV-->
                                <GroupBox Header="HSV" Grid.Column="2">
                                    <Grid>
                                        <Grid.ColumnDefinitions>
                                            <ColumnDefinition Width="Auto"/>
                                            <ColumnDefinition Width="Auto"/>
                                            <ColumnDefinition Width="Auto"/>
                                            <ColumnDefinition Width="Auto"/>
                                            <ColumnDefinition Width="*"/>
                                        </Grid.ColumnDefinitions>

                                        <TextBox x:Name="PART_HueTextBox" />
                                        <Label Grid.Column="1"/>
                                        <TextBox x:Name="PART_SaturationTextBox" Grid.Column="2"/>
                                        <Label Grid.Column="3"/>
                                        <TextBox x:Name="PART_ValueTextBox" Grid.Column="4"/>
                                    </Grid>
                                </GroupBox>

                                <!--HSL-->
                                <GroupBox Header="HSL" Grid.Column="3">
                                    <Grid>
                                        <Grid.ColumnDefinitions>
                                            <ColumnDefinition Width="Auto"/>
                                            <ColumnDefinition Width="Auto"/>
                                            <ColumnDefinition Width="Auto"/>
                                            <ColumnDefinition Width="Auto"/>
                                            <ColumnDefinition Width="*"/>
                                        </Grid.ColumnDefinitions>

                                        <TextBox x:Name="PART_HslHueTextBox" />
                                        <Label Grid.Column="1"/>
                                        <TextBox x:Name="PART_HslSaturationTextBox" Grid.Column="2"/>
                                        <Label Grid.Column="3"/>
                                        <TextBox x:Name="PART_LightnessTextBox" Grid.Column="4"/>
                                    </Grid>
                                </GroupBox>
                            </Grid>

                        </Grid>
                    </ControlTemplate>
                </Setter.Value>
            </Setter>
        </Style>
    </controls:ColorPicker.Style>
</controls:ColorPicker>
```

## 🔎 Project samples
The solution includes three sample (`ColorPicker.Wpf.App1`, `ColorPicker.Wpf.App2` and `ColorPicker.Wpf.App3`) applications demonstrating how to use the ColorPicker libraries.
These samples show:

* Integration of `ColorPicker.View.Wpf`

* How to bind the *SelectedColor* of *ColorPicker* to your own *ViewModel*

* Practical usage of color UI components

You can explore the sample projects to see ready-to-use implementations and learn how to integrate the libraries into your own *WPF* applications.

## 🧪 Testing
`ColorPicker.View.Wpf` implements the *WPF* user interface for the color picker, including all *PART_** template parts.
`ColorPicker.View.Wpf.Tests` uses *FlaUI* for automated UI testing. Tests interact with *PART_** elements, which allows coverage of different UI implementations and ensures the control behaves correctly regardless of the applied template.

**Note:** UI tests rely on *PART_** names to locate elements, but not all *PART_** elements need to be present. This makes tests flexible and able to cover different templates and partial implementations without modification.

# .NET MAUI Color Picker
<img width="145" height="143" src="https://github.com/user-attachments/assets/16c5283f-987d-4b67-81de-94aa47c72828" />

The .NET MAUI implementation is intentionally very minimalistic, as the technology is not yet fully optimized for handling frequent value changes in controls such as *Entry* and *Slider* (unlike *WPF*). The control provides a graphical interface for adjusting both the color and its transparency. For optimal performance and responsiveness, standard controls were insufficient, so custom UI controls were implemented.

## 🔧 Installation
The core library `ColorPicker.Core` and the *.NET MAUI* UI implementation `ColorPicker.View.Maui` (including `ColorPicker.Core`) are available as NuGet packages:

```powershell
dotnet add package ColorPicker.Core
```
```powershell
dotnet add package ColorPicker.View.Maui
```

## 🧬 Control template parts (PART_*)
When using the *ColorPicker*, you must always define the layout of its individual parts in *ControlTemplate*.

The final color selection is handled through *PART_ColorSelectionView*, where you can also configure the selection circle radius using the *MarkRadius* property on this element.

Transparency adjustment is performed through *PART_AlphaSelectionView*. This component automatically adapts to its available size. If its height is greater than its width, the entire layout switches to a vertical orientation.

The view that displays the resulting color is *PART_SelectedColorView*.

To display the resulting color in hexadecimal form, you can use *PART_HexLabel*.
<img width="726" height="431" alt="ControlTemplate parts" src="https://github.com/user-attachments/assets/cafd6959-ea9f-484c-ad40-3d33a3d8b93c" />

None of these component parts are mandatory.
The resulting color of the *ColorPicker* can be bound to your *ViewModel* through the *SelectedColor* property, which allows you to use only *PART_ColorSelectionView* and build the rest of the UI yourself.

## 🧩 Implementations
The implementation uses SkiaSharp, so any application using this component must register it in the builder.
```xml
builder.UseSkiaSharp();
```
A simple, minimalist implementation. This setup is entirely mouse- and touch-driven and includes only the all template parts required to select a color.

<img width="514" height="398" alt="Windows" src="https://github.com/user-attachments/assets/70e0e600-7603-4261-a0ed-7bf734c33b77" />

```xml
xmlns:controls="clr-namespace:ColorPicker.View.Maui;assembly=ColorPicker.View.Maui"
```
```xml
<controls:ColorPicker SelectedColor="{Binding SelectedColor, Mode=TwoWay}" Margin="4">
    <controls:ColorPicker.ControlTemplate>
        <ControlTemplate>
            <Grid>
                <Grid.RowDefinitions>
                    <RowDefinition Height="*"/>
                    <RowDefinition Height="50"/>
                    <RowDefinition Height="50"/>
                    <RowDefinition Height="26"/>
                </Grid.RowDefinitions>

                <!--Color selection-->
                <components:ColorSelectionView x:Name="PART_ColorSelectionView" />

                <!--Alpha selection-->
                <components:AlphaSelectionView x:Name="PART_AlphaSelectionView" Grid.Row="1" Margin="0,3" />

                <!--Selected color-->
                <components:SelectedColorView x:Name="PART_SelectedColorView" Grid.Row="2" Margin="0,3"/>

                <!--HEX-->
                <Grid Grid.Row="3" Margin="0,3">
                    <Grid.ColumnDefinitions>
                        <ColumnDefinition Width="Auto" />
                        <ColumnDefinition Width="*" />
                    </Grid.ColumnDefinitions>

                    <Label Text="HEX" Margin="0,0,5,0" />
                    <Label x:Name="PART_HexLabel" Grid.Column="1" />

                </Grid>

            </Grid>
        </ControlTemplate>
    </controls:ColorPicker.ControlTemplate>
</controls:ColorPicker>
```
For the Android platform, it is recommended to adjust the color selection radius for optimal usability.
```xml
<components:ColorSelectionView x:Name="PART_ColorSelectionView" MarkRadius="30" />
```
<img width="270" height="585" alt="Android" src="https://github.com/user-attachments/assets/3988f574-7498-4146-bc1f-b6fa69bc066c" />

## 🔎 Project sample
The solution includes one sample (`ColorPicker.Maui.App1`) applications demonstrating how to use the ColorPicker libraries. The application is cross-platform and can be run on multiple platforms.
These sample show:

* Integration of `ColorPicker.View.Maui`

* How to bind the *SelectedColor* of *ColorPicker* to your own *ViewModel*

* Practical usage of color UI components

You can explore the sample projects to see ready-to-use implementations and learn how to integrate the libraries into your own *.NET MAUI* applications.

# Blazor Color Picker
<img width="161" height="149" alt="image" src="https://github.com/user-attachments/assets/bdbae827-f546-4227-bac8-ef5db6b3c327" />

The component is designed for flexible and precise color selection, suitable for both simple and advanced use cases. It is composed of multiple layout parts (PART_*), allowing developers to fully customize its layout and behavior through ControlTemplate. This solution uses working with parts of a component via *RenderFragment*.

## 🔧 Installation
The core library `ColorPicker.Core` and the *Blazor* UI implementation `ColorPicker.View.Blazor` (including `ColorPicker.Core`) are available as NuGet packages:

```powershell
dotnet add package ColorPicker.Core
```
```powershell
dotnet add package ColorPicker.View.Blazor
```
## 🧬 Control template parts (PART_*)
This Component is composed of several element parts. When creating a custom layoutl, not all parts are required — the component is designed to function even if some are omitted.

<img width="1043" height="584" alt="image" src="https://github.com/user-attachments/assets/bd571e5c-6084-4bda-a30d-cb203b146770" />

The component allows you to customize the visualization of its individual parts. When a *Layout* property is applied, not all parts have to be included. If no layout is set, the default layout is used.

The final color is selected through PART_ColorSelectionView, which visualizes the current Hue value. The Hue itself is adjusted using PART_HueSelectionView. This control can automatically adapt to the available space — if its width is greater than its height, the value will be adjusted vertically. The same adaptive behavior is used by PART_AlphaSelectionView, which is responsible for adjusting opacity.

The selected color is displayed in PART_SelectedColorView.

For adjusting individual components of RGB, HSL, HSV, and CMYK, you can use the predefined inputs (range and text - i used text for the option to display units).

The resulting color can be bind to your own ViewModel via the *@bind-SelectedColor* property exposed by the ColorPicker, allowing you to extend or customize the existing solution.
```C#
<ColorPicker.View.Blazor.ColorPicker Layout="@MyLayout" @bind-SelectedColor="_mainViewModel.SelectedColor" />
```
## 🧩 Implementations
A simple, minimalist implementation that avoids the need for manual value entry. This setup is entirely mouse-driven and includes only the essential template parts required to select a color.

It uses only the core layout elements for hue, alpha and color selection, without additional inputs. This example is ideal for applications that require a compact and lightweight color picker with a minimal UI footprint.

The control can be easily extended by adding more layout parts as needed — such as sliders for fine-tuning numeric values, text inputs for precise color entry, alpha/transparency controls, or additional visual indicators. These layout can be freely restyled or rearranged using a custom styles to match the design of your application.

<img width="456" height="306" alt="image" src="https://github.com/user-attachments/assets/8dbd9ab6-932c-4b6e-b10e-d31cc41fe72d" />

```C#
<style>
    .color-picker-layout {
        display: flex;
        flex-direction: column;
        height: 400px;
        width: 600px;
    }

    .color-selection {
        flex: 1 1 0;
        min-height: 0;
    }

    .color-selection-inner {
        height: 100%;
        width: 100%;
    }

    .color-row {
        height: 26px;
        flex: 0 0 26px;
        gap: 6px;
        margin-top: 6px;
    }
</style>

<ColorPicker.View.Blazor.ColorPicker Layout="@MyLayout" @bind-SelectedColor="_mainViewModel.SelectedColor" />

@code {
    private RenderFragment<LayoutContext> MyLayout => ctx =>
    @<div class="color-picker-layout">
        <div class="color-selection">
            @ctx.PART_ColorSelectionView
        </div>

        <div class="color-row">
            @ctx.PART_HueSelectionView
        </div>

        <div class="color-row">
            @ctx.PART_AlphaSelectionView
        </div>

        <div class="color-row">
            @ctx.PART_SelectedColorView
        </div>
    </div>;

    private MainViewModel _mainViewModel = new MainViewModel();
}
```

This setup can be more complex and may include additional template parts for RGB, HSL, HSV, CMYK, and Hex. These values can be adjusted through the inputs (text and range).

This advanced layout is designed for applications that require detailed color manipulation, such as graphic tools, editors, or any scenario where precise color representation matters.

Like all other template variations, each part can be freely styled, rearranged, replaced, or omitted using a custom *Layout*. Developers can choose which color models to expose and can combine or remove UI elements depending on the needs of their application.

<img width="676" height="475" alt="image" src="https://github.com/user-attachments/assets/7d46c972-e2bb-472b-b200-6c0c4762116f" />

```C#
<style>
    .color-picker-layout {
        display: grid;
        grid-template-columns: 200px auto;
        grid-template-rows: auto 26px 26px 50px;
        height: 100vh;
        max-height: 600px;
    }

    .selected-color{
        margin-right: 4px;
        grid-column: 1;
        grid-row: 1;
    }

    .color-selection{
        grid-column: 2;
        grid-row: 1;
    }

    .hue-selection {
        margin-top:  4px;
        grid-column: 1 / span 2;
        grid-row: 2;
    }

    .alpha-selection {
        margin-top: 4px;
        grid-column: 1 / span 2;
        grid-row: 3;
    }

    .color-control {
        margin-top: 18px;
        grid-column: 1 / span 2;
        grid-row: 4;
        display: grid;
        grid-template-columns: 1fr 1.5fr 2fr 1.5fr 1.5fr;
        gap: 12px;
        justify-content: start;
        width:100%;
    }

    .group {
        position: relative;
        border: 1px solid #cfd8dc;
        border-radius: 6px;
        padding: 14px 12px 10px;
    }

    .row {
        display: flex;
        flex-direction: row;
        align-items: center;
    }

    .row div {
        padding: 0px !important;
    }

    .row input {
        max-width: 50px !important;
    }

    .row--wide input {
        max-width: 100px !important;
    }

    .row > div {
        width: auto;
        flex: 0 0 auto;
    }

    .label {
        position: absolute;
        top: -0.6em;
        left: 10px;
        background: white;
        padding: 0 6px;
        font-size: 0.75rem;
        color: #666;
    }
</style>

<ColorPicker.View.Blazor.ColorPicker Layout="@MyLayout" @bind-SelectedColor="_mainViewModel.SelectedColor" />

@code {
    private RenderFragment<LayoutContext> MyLayout => ctx =>
    @<div class="color-picker-layout">

        <!--Selected color-->
        <div class="selected-color">
            @ctx.PART_SelectedColorView
        </div>

        <!--Color selection-->
        <div class="color-selection">
            @ctx.PART_ColorSelectionView
        </div>

        <!--Hue-->
        <div class="hue-selection">
            @ctx.PART_HueSelectionView
        </div>

        <!--Alpha-->
        <div class="alpha-selection">
            @ctx.PART_AlphaSelectionView
        </div>

        <div class="color-control">
            <!--Hex-->
            <div class="group" style="grid-column: 1">
                <span class="label">Hex</span>
                <div class="row row--wide">
                    <div>@ctx.PART_HexTextBox</div>
                </div>
            </div>

            <!--RGB-->
            <div class="group" style="grid-column: 2">
                <span class="label">RGB</span>
                <div class ="row">
                    <div>@ctx.PART_RedTextBox</div>
                    <div>@ctx.PART_GreenTextBox</div>
                    <div>@ctx.PART_BlueTextBox</div>
                </div>
            </div>

            <!--CMYK-->
            <div class="group" style="grid-column: 3">
                <span class="label">CMYK</span>
                <div class="row">
                    <div>@ctx.PART_CyanTextBox</div>
                    <div>@ctx.PART_MagentaTextBox</div>
                    <div>@ctx.PART_YellowTextBox</div>
                    <div>@ctx.PART_KeyTextBox</div>
                </div>
            </div>

            <!--HSV-->
            <div class="group" style="grid-column: 4">
                <span class="label">HSV</span>
                <div class="row">
                    <div>@ctx.PART_HueTextBox</div>
                    <div>@ctx.PART_SaturationTextBox</div>
                    <div>@ctx.PART_ValueTextBox</div>
                </div>
            </div>

            <!--HSL-->
            <div class="group" style="grid-column: 5">
                <span class="label">HSL</span>
                <div class="row">
                    <div>@ctx.PART_HslHueTextBox</div>
                    <div>@ctx.PART_HslSaturationTextBox</div>
                    <div>@ctx.PART_HslLightnessTextBox</div>
                </div>
            </div>
        </div>

    </div>;

    private MainViewModel _mainViewModel = new MainViewModel();
}
```

## 🔎 Project sample
The solution includes three sample (`ColorPicker.Blazor.App1`, `ColorPicker.Blazor.App2` and `ColorPicker.Blazor.App3`) applications demonstrating how to use the ColorPicker libraries.
These samples show:

* Integration of `ColorPicker.View.Blazor`

* How to bind the *SelectedColor* of *ColorPicker* to your own *ViewModel*

* Practical usage of color UI components

You can explore the sample projects to see ready-to-use implementations and learn how to integrate the libraries into your own *Blazor* applications.

## 🧪 Testing
`ColorPicker.View.Blazor` implements the *Blazor* user interface for the color picker, including all *PART_** layout elements.
`ColorPicker.View.Blazor.Tests.bUnit` uses *bUnit* for testing work with the component itself and `ColorPicker.View.Blazor.Tests.Playwright` uses *Playwright* for testing interaction between component parts.

**Note:** UI tests rely on *PART_** names to locate elements, but not all *PART_** elements need to be present. This makes tests flexible and able to cover different templates and partial implementations without modification.

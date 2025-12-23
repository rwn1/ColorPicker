# 🎨 ColorPicker Core
`ColorPicker.Core` is a universal, multi-platform color-processing library, with current UI implementations available for *WPF* (`ColorPicker.View.Wpf`) and *.NET MAUI* (`ColorPicker.View.Maui`). Additional UI implementations may be added in the future (a *Blazor* version is currently being planned).
This implementation solves the issue where each technology uses its own color-related classes.

<img width="571" height="291" src="https://github.com/user-attachments/assets/a57439f0-c670-46fc-b887-c9ac2857c901" />

### 🧪 Testing
`ColorPicker.Core` contains all the main logic, algorithms, and color conversion routines.
`ColorPicker.Core.Tests` uses *xUnit* to verify correctness of calculations, property updates, and other core functionality independent of the UI.

## WPF Color Picker
<img width="148" height="147" alt="image" src="https://github.com/user-attachments/assets/6d2dba81-0b72-4e4a-84ec-57fb7256ffd1" />

The *WPF* implementation supports both legacy *.NET Framework* applications as well as modern *.NET*, allowing the control to be used across a wide range of *WPF* projects.
The control is designed for flexible and precise color selection, suitable for both simple and advanced use cases. It is composed of multiple template parts (PART_*), allowing developers to fully customize its layout and behavior through *ControlTemplate*.

### 🔧 Installation
The core library `ColorPicker.Core` and the *WPF* UI implementation `ColorPicker.View.Wpf` (including `ColorPicker.Core`) are available as NuGet packages:

```powershell
dotnet add package ColorPicker.Core
```
```powershell
dotnet add package ColorPicker.View.Wpf
```

### 🧬 Control template parts (PART_*)
This *Control* is composed of several *WPF* template parts.
When creating a custom *ControlTemplate*, not all template parts are required — the control is designed to function even if some are omitted.

<img width="936" height="460" src="https://github.com/user-attachments/assets/c9b76ff5-c695-4c5d-96c2-b67fe4865b95" />

The control allows you to customize the visualization of its individual parts.
When a *ControlTemplate* is applied, not all parts have to be included.

The final color is selected through *PART_ColorSelectionView*, which visualizes the current Hue value.
The Hue itself is adjusted using *PART_HueSelectionView*. This control can automatically adapt to the available space — if its width is greater than its height, the value will be adjusted vertically.
The same adaptive behavior is used by PART_AlphaSelectionView, which is responsible for adjusting opacity.

The selected color is displayed in *PART_SelectedColorView*.

For adjusting individual components of *RGB*, *HSL*, *HSV*, and *CMYK*, you can use the predefined *TextBoxes* and *Sliders*.

It is also possible to use *PART_EyedropperButton* to pick a color from the screen.

The resulting color can be bind to your own *ViewModel* via the *SelectedColor* property exposed by the *ColorPicker*, allowing you to extend or customize the existing solution.

### 🧩 Implementations
A simple, minimalist implementation that avoids the need for manual value entry.
This setup is entirely mouse-driven and includes only the essential template parts required to select a color.

It uses only the core template elements for hue and color selection, without additional sliders, numeric inputs, or alpha controls. This example is ideal for applications that require a compact and lightweight color picker with a minimal UI footprint.

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

<img width="583" height="383" src="https://github.com/user-attachments/assets/3849abad-4e3e-437a-9ef5-eb4eb7d99872" />

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

### 🔎 Project samples
The solution includes three sample (`ColorPicker.Wpf.App1`, `ColorPicker.Wpf.App2` and `ColorPicker.Wpf.App3`) applications demonstrating how to use the ColorPicker libraries.
These samples show:

* Integration of `ColorPicker.View.Wpf`

* How to bind the *SelectedColor* of ColorPicker to your own *ViewModel*

* Practical usage of color UI components

You can explore the sample projects to see ready-to-use implementations and learn how to integrate the libraries into your own *WPF* applications.

### 🧪 Testing
`ColorPicker.View.Wpf` implements the *WPF* user interface for the color picker, including all *PART_** template parts.
`ColorPicker.View.Wpf.Tests` uses *FlaUI* for automated UI testing. Tests interact with *PART_** elements, which allows coverage of different UI implementations and ensures the control behaves correctly regardless of the applied template.

**Note:** UI tests rely on *PART_** names to locate elements, but not all *PART_** elements need to be present. This makes tests flexible and able to cover different templates and partial implementations without modification.

## .NET MAUI Color Picker
<img width="145" height="143" alt="image" src="https://github.com/user-attachments/assets/4f98254f-d7e8-472b-950d-8961bd01e24b" />

The .NET MAUI implementation is intentionally very minimalistic, as the technology is not yet fully optimized for handling frequent value changes in controls such as *Entry* and *Slider* (unlike *WPF*). The control provides a graphical interface for adjusting both the color and its transparency. For optimal performance and responsiveness, standard controls were insufficient, so custom UI controls were implemented.

### 🔧 Installation
The core library `ColorPicker.Core` and the *.NET MAUI* UI implementation `ColorPicker.View.Maui` (including `ColorPicker.Core`) are available as NuGet packages:

```powershell
dotnet add package ColorPicker.Core
```
```powershell
dotnet add package ColorPicker.View.Maui
```

### 🧬 Control template parts (PART_*)
When using the *ColorPicker*, you must always define the layout of its individual parts in *ControlTemplate*.

The final color selection is handled through *PART_ColorSelectionView*, where you can also configure the selection circle radius using the *MarkRadius* property on this element.

Transparency adjustment is performed through *PART_AlphaSelectionView*. This component automatically adapts to its available size. If its height is greater than its width, the entire layout switches to a vertical orientation.

The view that displays the resulting color is *PART_SelectedColorView*.

To display the resulting color in hexadecimal form, you can use *PART_HexLabel*.
<img width="726" height="431" alt="ControlTemplate parts" src="https://github.com/user-attachments/assets/cafd6959-ea9f-484c-ad40-3d33a3d8b93c" />

None of these component parts are mandatory.
The resulting color of the *ColorPicker* can be bound to your *ViewModel* through the *SelectedColor* property, which allows you to use only *PART_ColorSelectionView* and build the rest of the UI yourself.

### 🧩 Implementations
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

### 🔎 Project sample
The solution includes one sample (`ColorPicker.Maui.App1`) applications demonstrating how to use the ColorPicker libraries. The application is cross-platform and can be run on multiple platforms.
These sample show:

* Integration of `ColorPicker.View.Maui`

* How to bind the *SelectedColor* of ColorPicker to your own *ViewModel*

* Practical usage of color UI components

You can explore the sample projects to see ready-to-use implementations and learn how to integrate the libraries into your own *.NET MAUI* applications.

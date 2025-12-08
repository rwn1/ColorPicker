# 🎨 ColorPicker
`ColorPicker.Core` is a universal, multi-platform color-processing library, with a *WPF* UI implementation provided in `ColorPicker.View.Wpf`. Additional UI implementations may be added in the future (a *Blazor* version is currently being planned).
The *WPF* implementation supports both legacy *.NET Framework* applications as well as modern *.NET* (including *.NET 6–8*), allowing the control to be used across a wide range of *WPF* projects.
The control is designed for flexible and precise color selection, suitable for both simple and advanced use cases. It is composed of multiple template parts (PART_*), allowing developers to fully customize its layout and behavior through *ControlTemplate*.

## 🔧 Installation
The core library `ColorPicker.Core` and the *WPF* UI implementation `ColorPicker.View.Wpf` (including `ColorPicker.Core`) are available as NuGet packages:

```powershell
dotnet add package ColorPicker.Core
```
```powershell
dotnet add package ColorPicker.View.Wpf
```

## 🧬 WPF Control template parts (PART_*)
This *Control* is composed of several *WPF* template parts.
When creating a custom *ControlTemplate*, not all template parts are required — the control is designed to function even if some are omitted.

<img width="1964" height="1070" alt="image" src="https://github.com/user-attachments/assets/c2eda04c-585d-42e2-a54d-0db634a46305" />

The hue selection supports both horizontal and vertical orientations, exposed through the template parts:
* *PART_VerticalHueCanvas*
* *PART_HorizontalHueCanvas*
  
The final selected color is represented by:
* *PART_SelectedColor*
* *PART_SelectedColorBackground* (used to visualize transparency)
  
Numeric values can be adjusted using both *TextBox* inputs and *Sliders*, depending on how the template is composed.

An optional eyedropper tool is available through:

* *PART_EyedropperButton* (used to pick a color from anywhere on the screen)

All template parts can be fully restyled or replaced, giving developers complete freedom to customize the control’s appearance and behavior.

The selected color is exposed through the SelectedColor dependency property, which can be easily data-bound to a *ViewModel* in *MVVM* scenarios.

## 🧩 Implementations
A simple, minimalist implementation that avoids the need for manual value entry.
This setup is entirely mouse-driven and includes only the essential template parts required to select a color.

It uses only the core template elements for hue and color selection, without additional sliders, numeric inputs, or alpha controls. This example is ideal for applications that require a compact and lightweight color picker with a minimal UI footprint.

The control can be easily extended by adding more template parts as needed — such as sliders for fine-tuning numeric values, *TextBox* inputs for precise color entry, alpha/transparency controls, or additional visual indicators. These elements can be freely restyled or rearranged using a custom *ControlTemplate* to match the design of your application.

<img width="385" height="217" alt="image" src="https://github.com/user-attachments/assets/45a06bbd-483f-430c-a243-e21169eaf335" />

```xml
xmlns:controls="clr-namespace:ColorPicker.View.Wpf;assembly=ColorPicker.View.Wpf"
```
```xml
<controls:ColorPicker SelectedBrush="{Binding SelectedBrush, Mode=TwoWay}" Margin="4">
    <controls:ColorPicker.Style>
        <Style TargetType="{x:Type controls:ColorPicker}">
            <Setter Property="Template">
                <Setter.Value>
                    <ControlTemplate TargetType="{x:Type controls:ColorPicker}">
                        <Grid>
                            <Grid.ColumnDefinitions>
                                <ColumnDefinition Width="*"/>
                                <ColumnDefinition Width="26"/>
                            </Grid.ColumnDefinitions>
                            <Grid.RowDefinitions>
                                <RowDefinition Height="*"/>
                                <RowDefinition Height="26"/>
                            </Grid.RowDefinitions>

                            <Canvas x:Name="PART_ColorSelectionCanvas" />

                            <Canvas x:Name="PART_VerticalHueCanvas" Grid.Column="1" Margin="3,0" />

                            <Rectangle x:Name="PART_SelectedColorBackground" Margin="0,3" Grid.Row="1" />
                            <Rectangle x:Name="PART_SelectedColor" Margin="0,3" Grid.Row="1" />

                            <Button x:Name="PART_EyedropperButton" Margin="3" Grid.Column="1" Grid.Row="1" />
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

Like all other template variations, each part can be freely styled, rearranged, replaced, or omitted using a custom ControlTemplate. Developers can choose which color models to expose and can combine or remove UI elements depending on the needs of their application.

<img width="494" height="261" alt="image" src="https://github.com/user-attachments/assets/28c434d6-4b31-46dd-a4ea-17639828cd99" />

```xml
xmlns:controls="clr-namespace:ColorPicker.View.Wpf;assembly=ColorPicker.View.Wpf"
```
```xml
<controls:ColorPicker SelectedBrush="{Binding SelectedBrush, Mode=TwoWay}" Margin="4">
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
                                <RowDefinition Height="Auto"/>
                                <RowDefinition Height="Auto"/>
                            </Grid.RowDefinitions>
                            <Grid.ColumnDefinitions>
                                <ColumnDefinition Width="*"/>
                                <ColumnDefinition Width="3*"/>
                            </Grid.ColumnDefinitions>

                            <!--Selected brush-->
                            <Rectangle x:Name="PART_SelectedColor" Margin="0,0,3,0"/>
                            
                            <!--Brush for select-->
                            <Canvas x:Name="PART_ColorSelectionCanvas" Grid.Column="1" />

                            <!--Hue with main brushes-->
                            <Canvas x:Name="PART_HorizontalHueCanvas" Grid.Row="1" Grid.ColumnSpan="2" Margin="0,3" />

                            <GroupBox Header="HEX" Grid.Row="2" Grid.ColumnSpan="2">
                                <Grid>
                                    <Grid.ColumnDefinitions>
                                        <ColumnDefinition Width="*"/>
                                    </Grid.ColumnDefinitions>

                                    <TextBox x:Name="PART_HexTextBox" />
                                </Grid>
                            </GroupBox>

                            <Grid Grid.Row="3" Grid.ColumnSpan="2">
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
The solution includes three sample (`ColorPicker.App1`, `ColorPicker.App2` and `ColorPicker.App3`) applications demonstrating how to use the ColorPicker libraries.
These samples show:

* Integration of `ColorPicker.View.Wpf`

* How to bind the *SelectedColor* of ColorPicker to your own *ViewModel*

* Practical usage of color UI components

You can explore the sample projects to see ready-to-use implementations and learn how to integrate the libraries into your own *WPF* applications.

## 🧪 Testing
The ColorPicker solution is organized into separate projects for core logic and *WPF* user interface, each with its own set of tests.

#### Core Logic
`ColorPicker.Core` contains all the main logic, algorithms, and color conversion routines.
`ColorPicker.Core.Tests` uses *xUnit* to verify correctness of calculations, property updates, and other core functionality independent of the UI.

#### WPF User Interface
`ColorPicker.View.Wpf` implements the *WPF* user interface for the color picker, including all *PART_** template parts.
`ColorPicker.View.Wpf.Tests` uses *FlaUI* for automated UI testing. Tests interact with *PART_** elements, which allows coverage of different UI implementations and ensures the control behaves correctly regardless of the applied template.

**Note:** UI tests rely on *PART_** names to locate elements, but not all *PART_** elements need to be present. This makes tests flexible and able to cover different templates and partial implementations without modification.

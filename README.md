# ColorPicker
ColorPicker provides the ability to set colors using a customizable user interface.


A possible way to set it up that saves users from having to type in values manually. This is a minimalist solution. Everything is done with the mouse.

<img width="470" height="274" alt="image" src="https://github.com/user-attachments/assets/0b93dd83-4974-45e4-884a-6223d6ace84a" />

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

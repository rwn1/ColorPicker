using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace ColorPicker.View.Wpf.Shared
{
    public partial class EyedropperOverlay : Window
    {
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);
        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hwnd);
        [DllImport("gdi32.dll")]
        public static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);
        [DllImport("user32.dll")]
        public static extern int ReleaseDC(IntPtr hwnd, IntPtr hdc);

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT { public int X; public int Y; }

        private readonly DispatcherTimer _timer;

        public event EventHandler<Color?> ColorPicked;
        public event EventHandler<Color> ColorChanged;

        public EyedropperOverlay()
        {
            Loaded += EyedropperOverlay_Loaded;

            WindowStyle = WindowStyle.None;
            AllowsTransparency = true;
            Background = new SolidColorBrush(Color.FromArgb(1, 0, 0, 0));
            Topmost = true;
            ShowInTaskbar = false;
            Cursor = Cursors.Cross;

            _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(40) };
            _timer.Tick += UpdatePreview;
            _timer.Start();

            MouseLeftButtonDown += OnMouseLeftButtonDown;
            KeyDown += OnKeyDown;
            Closed += OnClosed;
        }

        private void EyedropperOverlay_Loaded(object sender, RoutedEventArgs e)
        {
            var source = PresentationSource.FromVisual(this);
            Matrix transform = source?.CompositionTarget?.TransformFromDevice ?? Matrix.Identity;

            System.Drawing.Rectangle all = System.Windows.Forms.Screen.AllScreens.Aggregate(System.Drawing.Rectangle.Empty, (acc, s) => System.Drawing.Rectangle.Union(acc, s.Bounds));

            var topLeft = transform.Transform(new System.Windows.Point(all.Left, all.Top));
            var size = transform.Transform(new System.Windows.Point(all.Width, all.Height));

            Left = topLeft.X;
            Top = topLeft.Y;
            Width = Math.Abs(size.X);
            Height = Math.Abs(size.Y);

            WindowStartupLocation = WindowStartupLocation.Manual;
        }

        private void UpdatePreview(object sender, EventArgs e)
        {
            if (GetCursorPos(out POINT p))
            {
                IntPtr hdc = GetDC(IntPtr.Zero);
                uint pixel = GetPixel(hdc, p.X, p.Y);
                ReleaseDC(IntPtr.Zero, hdc);

                byte r = (byte)(pixel & 0x000000FF);
                byte g = (byte)((pixel & 0x0000FF00) >> 8);
                byte b = (byte)((pixel & 0x00FF0000) >> 16);
                Color color = Color.FromRgb(r, g, b);
                ColorChanged?.Invoke(this, color);
            }
        }
        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _timer.Stop();
            if (GetCursorPos(out POINT p))
            {
                IntPtr hdc = GetDC(IntPtr.Zero);
                uint pixel = GetPixel(hdc, p.X, p.Y);
                ReleaseDC(IntPtr.Zero, hdc);

                byte r = (byte)(pixel & 0x000000FF);
                byte g = (byte)((pixel & 0x0000FF00) >> 8);
                byte b = (byte)((pixel & 0x00FF0000) >> 16);
                Color color = Color.FromRgb(r, g, b);
                ColorPicked?.Invoke(this, color);
            }
            Close();
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                ColorPicked?.Invoke(this, null);
                Close();
            }
        }

        private void OnClosed(object sender, EventArgs e)
        {
            _timer.Stop();
            _timer.Tick -= UpdatePreview;

            Loaded -= EyedropperOverlay_Loaded;
            MouseLeftButtonDown -= OnMouseLeftButtonDown;
            KeyDown -= OnKeyDown;

            Closed -= OnClosed;
        }
    }
}
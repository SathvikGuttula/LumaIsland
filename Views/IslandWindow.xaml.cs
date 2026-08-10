using System.ComponentModel;
using System.Windows;
using System.Windows.Media.Animation;
using LumaIsland.Services;
using LumaIsland.ViewModels;
using MouseButtonEventArgs = System.Windows.Input.MouseButtonEventArgs;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;

namespace LumaIsland.Views;

public partial class IslandWindow : Window
{
    private readonly IslandViewModel _viewModel;
    private readonly System.Windows.Threading.DispatcherTimer _collapseTimer;

    public IslandWindow(IslandViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;
        DataContext = _viewModel;

        Loaded += IslandWindow_Loaded;
        SizeChanged += (_, _) => CenterTop();
        MouseEnter += IslandWindow_MouseEnter;
        MouseLeave += IslandWindow_MouseLeave;
        PreviewMouseLeftButtonDown += IslandWindow_PreviewMouseLeftButtonDown;

        _collapseTimer = new System.Windows.Threading.DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(900)
        };

        _collapseTimer.Tick += (_, _) =>
        {
            _collapseTimer.Stop();
            if (_viewModel.Settings.ExpandOnHover)
                _viewModel.IsExpanded = false;
        };

        _viewModel.PropertyChanged += ViewModel_PropertyChanged;
    }

    private void IslandWindow_Loaded(object sender, RoutedEventArgs e)
    {
        WindowStyleHelper.MakeToolWindow(this);
        Width = _viewModel.Settings.CollapsedWidth;
        Height = _viewModel.Settings.CollapsedHeight;
        CenterTop();
    }

    private void IslandWindow_MouseEnter(object sender, MouseEventArgs e)
    {
        _collapseTimer.Stop();

        if (_viewModel.Settings.ExpandOnHover)
            _viewModel.IsExpanded = true;
    }

    private void IslandWindow_MouseLeave(object sender, MouseEventArgs e)
    {
        if (_viewModel.Settings.ExpandOnHover)
            _collapseTimer.Start();
    }

    private void IslandWindow_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (!_viewModel.IsExpanded)
        {
            _collapseTimer.Stop();
            _viewModel.IsExpanded = true;
        }
    }

    private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(IslandViewModel.IsExpanded))
        {
            AnimateState(_viewModel.IsExpanded);
        }
    }

    private void AnimateState(bool expanded)
    {
        double targetWidth = expanded ? _viewModel.Settings.ExpandedWidth : _viewModel.Settings.CollapsedWidth;
        double targetHeight = expanded ? _viewModel.Settings.ExpandedHeight : _viewModel.Settings.CollapsedHeight;

        var ease = new CubicEase { EasingMode = EasingMode.EaseOut };

        BeginAnimation(WidthProperty, new DoubleAnimation(targetWidth, TimeSpan.FromMilliseconds(220)) { EasingFunction = ease });
        BeginAnimation(HeightProperty, new DoubleAnimation(targetHeight, TimeSpan.FromMilliseconds(220)) { EasingFunction = ease });

        CollapsedView.Visibility = expanded ? Visibility.Collapsed : Visibility.Visible;
        ExpandedView.Visibility = expanded ? Visibility.Visible : Visibility.Collapsed;
    }

    private void CenterTop()
    {
        Left = (SystemParameters.PrimaryScreenWidth - Width) / 2;
        Top = 6;
    }

    private void SettingsButton_Click(object sender, RoutedEventArgs e)
    {
        _collapseTimer.Stop();
        _viewModel.IsExpanded = true;

        var settingsWindow = new SettingsWindow(App.Settings, App.SettingsService)
        {
            Owner = this
        };

        settingsWindow.ShowDialog();

        if (_viewModel.Settings.ExpandOnHover && !IsMouseOver)
            _collapseTimer.Start();
    }
}
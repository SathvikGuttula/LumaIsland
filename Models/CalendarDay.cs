using CommunityToolkit.Mvvm.ComponentModel;

namespace LumaIsland.Models;

public partial class CalendarDay : ObservableObject
{
    [ObservableProperty]
    private string shortDay = "";

    [ObservableProperty]
    private int dayNumber;

    [ObservableProperty]
    private DateTime date;

    [ObservableProperty]
    private bool isSelected;
}
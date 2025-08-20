using CommunityToolkit.Mvvm.ComponentModel;

namespace EsnafYonetim.UI.ViewModels;

public partial class ViewModelBase : ObservableObject
{
    [ObservableProperty]
    private bool _isBusy;
}

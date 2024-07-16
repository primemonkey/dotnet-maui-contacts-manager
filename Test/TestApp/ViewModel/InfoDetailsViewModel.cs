namespace TestApp.ViewModel;

[QueryProperty(nameof(Info), "Info")]
public partial class InfoDetailsViewModel : BaseViewModel
{
    [ObservableProperty]
    Info info;
}

namespace TestApp.View;

public partial class MainPage : ContentPage
{
	public MainPage(InfosViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
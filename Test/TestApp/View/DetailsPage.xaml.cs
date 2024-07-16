namespace TestApp;

public partial class DetailsPage : ContentPage
{
	public DetailsPage(InfoDetailsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
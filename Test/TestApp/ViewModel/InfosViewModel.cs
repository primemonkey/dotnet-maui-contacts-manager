namespace TestApp.ViewModel;

public partial class InfosViewModel : BaseViewModel
{
    public ObservableCollection<Info> Infos { get; } = new();
    InfoService infoService;
    IConnectivity connectivity;
    public static string lastCallNumber;
    public static int notificationId;

    public InfosViewModel(InfoService infoService, IConnectivity connectivity)
    {
        Title = "Info Finder";
        this.infoService = infoService;
        this.connectivity = connectivity;
    }
    
    [RelayCommand]
    async Task GoToDetails(Info info)
    {
        if (info == null)
        return;

        await Shell.Current.GoToAsync(nameof(DetailsPage), true, new Dictionary<string, object>
        {
            {"Info", info }
        });
    }

    [ObservableProperty]
    bool isRefreshing;

    [RelayCommand]
    async Task GetInfosAsync()
    {
        if (IsBusy)
            return;

        try
        {
            if (connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await Shell.Current.DisplayAlert("No connectivity!",
                    $"Please check internet and try again.", "OK");
                return;
            }

            IsBusy = true;
            var infos = await infoService.GetInfos();

            if(Infos.Count != 0)
                Infos.Clear();

            foreach(var info in infos)
                Infos.Add(info);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to get infos: {ex.Message}");
            await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
            IsRefreshing = false;
        }

    }

    [RelayCommand]
    public async Task CheckInfo()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            var infos = await infoService.GetInfos();

            if (Infos.Count != 0)
                Infos.Clear();

            foreach (var info in infos)
            {
                if (OnlyDigits(info.Phone) == lastCallNumber)
                {
                    Infos.Add(info);
                    
                    var request = new NotificationRequest
                    {
                        NotificationId = notificationId++,
                        Title = "Last call",
                        Subtitle = "Hello",
                        Description = $"{info.Phone} {Environment.NewLine}" +
                                      $"{info.Name} {Environment.NewLine}" +
                                      $"{info.Occupation} {Environment.NewLine}",

                        Schedule = new NotificationRequestSchedule
                        {
                            NotifyTime = DateTime.Now.AddSeconds(1),
                        }
                    };

                    await LocalNotificationCenter.Current.Show(request);

                    break;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to check info: {ex.Message}");
            await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
            IsRefreshing = false;
        }
    }

    public static string OnlyDigits(string s)
    {
        if (string.IsNullOrEmpty(s)) return s;
        StringBuilder sb = new StringBuilder(s);
        int j = 0;
        int i = 0;
        while (i < sb.Length)
        {
            bool isDigit = char.IsDigit(sb[i]);
            if (isDigit)
            {
                sb[j++] = sb[i++];
            }
            else
            {
                ++i;
            }
        }
        sb.Length = j;
        string cleaned = sb.ToString();
        return cleaned;
    }
}
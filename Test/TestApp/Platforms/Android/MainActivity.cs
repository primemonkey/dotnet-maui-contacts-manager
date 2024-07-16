using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Telephony;

namespace TestApp;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    private TelephonyManager telephonyManager;
    private PhoneCallStateListener phoneCallListener;

    async static Task CheckPermission()
    {
        var status = await Permissions.CheckStatusAsync<Permissions.Phone>();

        if (status != PermissionStatus.Granted)
        {
            status = await Permissions.RequestAsync<Permissions.Phone>();

            if (status != PermissionStatus.Granted)
            {
                // Permission denied, handle accordingly
                return;
            }
        }

        if (await LocalNotificationCenter.Current.AreNotificationsEnabled() == false)
        {
            await LocalNotificationCenter.Current.RequestNotificationPermission();
        }
    }

    protected override async void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        await CheckPermission();
        InfosViewModel.notificationId = 100;

        telephonyManager = (TelephonyManager)GetSystemService(Context.TelephonyService);
        phoneCallListener = new PhoneCallStateListener();

        telephonyManager.Listen(phoneCallListener, PhoneStateListenerFlags.CallState);
    }
}

public class PhoneCallStateListener : PhoneStateListener
{
    public override void OnCallStateChanged(CallState state, string phoneNumber)
    {
        base.OnCallStateChanged(state, phoneNumber);

        switch (state)
        {
            case CallState.Ringing:
                // Handle incoming call
                LastCallNotification(phoneNumber);
                break;

            case CallState.Offhook:
                // Handle call in progress
                break;

            case CallState.Idle:
                // Handle call ended
                break;
        }
    }
    
    public static async void LastCallNotification(string phoneNumber)
    {        
        InfoService infoService = new();
        IConnectivity connectivity = null;
        InfosViewModel.lastCallNumber = InfosViewModel.OnlyDigits(phoneNumber);
        InfosViewModel instance =  new InfosViewModel (infoService, connectivity);

        await instance.CheckInfo();
    }
}
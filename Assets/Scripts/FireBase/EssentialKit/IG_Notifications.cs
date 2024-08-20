using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.EssentialKit;
using UnityEngine.UI;
public class IG_Notifications : MonoBehaviour
{
    public static IG_Notifications instance;

    NotificationSettings settings;
    [SerializeField] private Text txtToken;
    [SerializeField] private Text txtUserID;

    bool isRegistered;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        VoxelBusters.EssentialKit.NotificationServices.GetSettings((result) =>
        {
            settings = result.Settings;
            // update console
            Debug.Log(settings.ToString());

            Debug.Log("Permission status : " + settings.PermissionStatus);
        });

        if (!settings.PushNotificationEnabled)
        {
            VoxelBusters.EssentialKit.NotificationServices.UnregisterForPushNotifications();
            isRegistered = false;
        }

    }

    //IG_Notifications.instance.CreateLocalNotification("GoldMindFull", "Your gold minds are full!", 3600, false);
    public void CreateLocalNotification(string notificationID, string notificationTitle, double time, bool canRepeat)
    {
        if (settings.PermissionStatus == NotificationPermissionStatus.NotDetermined)
        {
            VoxelBusters.EssentialKit.NotificationServices.RequestPermission(NotificationPermissionOptions.Alert | NotificationPermissionOptions.Sound | NotificationPermissionOptions.Badge, callback: (result, error) =>
            {
                Debug.Log("Request for access finished.");
                Debug.Log("Notification access status: " + result.PermissionStatus);
            });
        }
        if (settings.PermissionStatus == NotificationPermissionStatus.Denied)
        {
            Utilities.OpenApplicationSettings();
        }
        if (settings.PermissionStatus == NotificationPermissionStatus.Authorized)
        {


            INotification notification = NotificationBuilder.CreateNotification(notificationID)
        .SetTitle(notificationTitle)
        .SetTimeIntervalNotificationTrigger(time, repeats: canRepeat)
        .Create();

            VoxelBusters.EssentialKit.NotificationServices.ScheduleNotification(notification, (error) =>
            {
                if (error == null)
                {
                    Debug.Log("Request to schedule notification finished successfully.");
                }
                else
                {
                    Debug.Log("Request to schedule notification failed with error. Error: " + error);
                }
            });
        }
    }


    public void OnEnable()
    {
        VoxelBusters.EssentialKit.NotificationServices.RegisterForPushNotifications((result, error) =>
        {
            if (error == null)
            {
                Debug.Log("Remote notification registration finished successfully. Device token: " + result.DeviceToken);
                Debug.Log(result.DeviceToken.ToString());
                txtToken.text = result.DeviceToken.ToString();
                SaveDeviceTokenToFirebase(result.DeviceToken);
            }
            else
            {
                Debug.Log("Remote notification registration failed with error. Error: " + error.Description);
            }
        });

        isRegistered = VoxelBusters.EssentialKit.NotificationServices.IsRegisteredForPushNotifications();
    }


    private void OnDisable()
    {
        if (isRegistered)
            VoxelBusters.EssentialKit.NotificationServices.UnregisterForPushNotifications();
    }

    private void SaveDeviceTokenToFirebase(string token)
    {
        Firebase.Auth.FirebaseUser user = Firebase.Auth.FirebaseAuth.DefaultInstance.CurrentUser;

        if (user != null)
        {
            Firebase.Database.DatabaseReference reference = Firebase.Database.FirebaseDatabase.DefaultInstance.RootReference;
            reference.Child("users").Child(user.UserId).Child("deviceToken").SetValueAsync(token).ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    txtUserID.text = user.UserId;
                    Debug.Log("Device token saved successfully.");
                }
                else
                {
                    Debug.LogError("Failed to save device token: " + task.Exception);
                }
            });
        }
        else
        {
            Debug.LogError("User is not logged in. Cannot save device token.");
        }
    }

}
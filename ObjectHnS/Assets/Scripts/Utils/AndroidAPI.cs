using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidAPI : MonoBehaviour
{
    public static void ToastMessage(string message)
    {
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        AndroidJavaClass toast = new AndroidJavaClass("android.widget.Toast");
        unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
        {
            AndroidJavaObject toastObject = toast.CallStatic<AndroidJavaObject>("makeText", unityActivity, message, 0);
            toastObject.Call("show");
        }));
    }
}

using UnityEngine;
using UnityEngine.UI;

namespace Assets.SafariView
{
    public class Example : MonoBehaviour
    {
        public Text Log;
        
        public void Start()
        {
            Application.logMessageReceived += (condition, stacktrace, type) => Log.text += condition + "\n";
        }

        public void Navigate(string url)
        {
            #if UNITY_IOS && !UNITY_EDITOR

            SafariViewController.OpenURL(url);

            #else

            Debug.LogError("This asset works only on iOS devices.");

            #endif
        }
    }
}
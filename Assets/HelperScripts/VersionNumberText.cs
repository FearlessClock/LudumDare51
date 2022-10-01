using System.Collections.Generic;
using TMPro;
using UnityEngine;

[ExecuteInEditMode]
public class VersionNumberText : MonoBehaviour
{
    private TextMeshProUGUI text = null;
    [SerializeField] private StringVariable bundleVersionAndroid = null;
    [SerializeField] private StringVariable bundleVersionIOS = null;

    private void OnEnable()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.SetText("V"+Application.version + " - " + (Application.platform == RuntimePlatform.Android? bundleVersionAndroid.value : bundleVersionIOS.value));
    }
}

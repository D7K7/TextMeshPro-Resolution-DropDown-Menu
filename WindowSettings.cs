using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WindowSettings : MonoBehaviour
{
    #region Attributes

    #endregion

    #region Player Pref Key Constants

    private const string RESOLUTION_PREF_KEY = "resolution";

    #endregion

    #region Resolution

    [SerializeField]
    private TextMeshProUGUI resolutionText;

    private Resolution[] resolutions;

    private int currentResolutionIndex = 0;

    public TMP_Dropdown resolutionDropdown;

    private bool clickable = true;

    #endregion

    private void Start()
    {
        clickable = true;

        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        int endResolutionIndex = resolutions.Length;

        //Makes a list of resolution options.
        List<string> options = new List<string>();
        
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height + " " + resolutions[i].refreshRate + "hz";

            options.Add(option);
        }

        //Adds options to the list.
        resolutionDropdown.AddOptions(options);

        currentResolutionIndex = endResolutionIndex;

        //resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        currentResolutionIndex = PlayerPrefs.GetInt(RESOLUTION_PREF_KEY, endResolutionIndex);

        SetResolutionText(Screen.currentResolution);
        Debug.Log(Screen.currentResolution);
    }

    #region Apply Resolution

    private void SetAndApplyResolution(int newResolutionIndex)
    {
        currentResolutionIndex = newResolutionIndex;
        ApplyResolution(resolutions[currentResolutionIndex]);
        Debug.Log("Applying " + currentResolutionIndex);
        StartCoroutine(WaittoClick(1));
    }
    //To prevent click spamming.
    IEnumerator WaittoClick(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        clickable = true;
    }

    private void ApplyResolution(Resolution resolution)
    {
        SetResolutionText(resolution);
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        Debug.Log("Applying yo" + resolution.width + " x " + resolution.height);
        PlayerPrefs.SetInt(RESOLUTION_PREF_KEY, currentResolutionIndex);
    }

    #endregion

    #region Resolution Cycling
    
    private void SetResolutionText(Resolution resolution) => resolutionText.text = resolution.width + " x " + resolution.height;

    #endregion
    //This needs to be able to find what part of the list is selected.
    public void ApplyChanges(int x)
    {
        //Experimenting with bools to prevent click spamming.
        if (clickable)
        {
            clickable = false;
            resolutionDropdown.value = x;
            resolutionDropdown.RefreshShownValue();
            SetAndApplyResolution(x);
        }
    }

    public void ToggleFullScreen(bool option)
    {
        Screen.fullScreen = option;
        Debug.Log("FullScreen is " + option);
    }
}

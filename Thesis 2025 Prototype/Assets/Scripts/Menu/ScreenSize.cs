using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class ScreenSize : MonoBehaviour
{
    public Dropdown dd;


    private bool fullscreen = true;
    private Resolution[] resolutions;

    void Start()
    {
        Resolution[] _resolutions = Screen.resolutions;
        resolutions = new Resolution[_resolutions.Length];

        int j = 0;
        for (int i = _resolutions.Length - 1; i >= 0; i--)
        {
            resolutions[j] = _resolutions[i];
            j++;
        }

        List<string> opt = new List<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            opt.Add((String)(resolutions[i].width + "X" + resolutions[i].height));
        }

        opt = opt.Distinct().ToList();

        dd.ClearOptions();
        dd.AddOptions(opt);
    }

    public void ChangeScreenRes()
    {
        //Debug.Log("screen res cahnged, " + resolutions[dd.value].width + "x" + resolutions[dd.value].height);
        Screen.SetResolution(resolutions[dd.value].width, resolutions[dd.value].height, fullscreen);
    }

    public void toggleFullscreen()
    {
        fullscreen = !fullscreen;
        //Debug.Log(fullscreen);
        ChangeScreenRes();
    }
}

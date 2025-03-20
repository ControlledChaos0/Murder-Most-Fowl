using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayMenu : Menu
{
    [SerializeField]
    private TMP_Dropdown _dropdown;

    private Resolution[] _resolutions;
    private float _aspectRatio;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _aspectRatio = (float)Display.main.systemWidth / (float)Display.main.systemHeight;

        _dropdown.ClearOptions();

        _resolutions = Screen.resolutions;
        List<TMP_Dropdown.OptionData> optionList = new();
        TMP_Dropdown.OptionData optionData;
        foreach (Resolution resolution in _resolutions)
        {
            if (Math.Abs(((float)resolution.width / (float)resolution.height) - _aspectRatio) > 0.0001)
            {
                continue;
            }

            optionData = new();
            optionData.text = resolution.width + " x " + resolution.height;
            if (optionList.Exists(option => optionData.text == option.text))
            {
                continue;
            }
            optionData.color = Color.black;
            optionList.Insert(0, optionData);
        }

        optionData = new();
        optionData.text = "Native";
        optionData.color = Color.black;
        optionList.Insert(0, optionData);

        _dropdown.AddOptions(optionList);

        _dropdown.onValueChanged.AddListener(SetResolution);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy()
    {
        _dropdown.onValueChanged.RemoveAllListeners();
    }

    public void SetResolution(Int32 index)
    {
        int width;
        int height;

        TMP_Dropdown.OptionData option = _dropdown.options[index];
        if (option.text == "Native")
        {
            width = Display.main.systemWidth;
            height = Display.main.systemHeight;
        }
        else
        {
            string[] resolution = option.text.Split(" x ");
            width = int.Parse(resolution[0]);
            height = int.Parse(resolution[1]);
        }

        Screen.SetResolution(width, height, true);
    }
}

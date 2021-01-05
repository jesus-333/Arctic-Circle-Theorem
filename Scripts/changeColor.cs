using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class changeColor : MonoBehaviour
{
    private float hue, saturation, value;
    private bool abilitate_change_color = false, first_change_color = false, hide_GUI = false;
    private string material_selector;

    public Material material_prefab_UP, material_prefab_DOWN, material_prefab_LEFT, material_prefab_RIGHT;
    public Slider slider_hue, slider_saturation, slider_value;
    public GameObject UI;

    // Start is called before the first frame update
    void Start()
    {
        // Set standard color
        setStandardColor();

        // Select the defualt text value of the dropdown
        selectTiles();

        // Set the initial value for the sliders
        Color.RGBToHSV(Color.green, out hue, out saturation, out value);
        setSliderValues();

        // Disable UI
        UI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Activate/Deactivate the UI when X is pressed
        if(Input.GetKeyDown(KeyCode.X)){
            UI.SetActive(!UI.activeSelf);
        }

        if(abilitate_change_color)
        {
            switch(material_selector){
                case "UP":    material_prefab_UP.color    = Color.HSVToRGB(hue, saturation, value); break;
                case "DOWN":  material_prefab_DOWN.color  = Color.HSVToRGB(hue, saturation, value); break;
                case "LEFT":  material_prefab_LEFT.color  = Color.HSVToRGB(hue, saturation, value); break;
                case "RIGHT": material_prefab_RIGHT.color = Color.HSVToRGB(hue, saturation, value); break;
            }

            // Disable change color
            abilitate_change_color = false;
            first_change_color = true;
            print(material_selector);
        }

        // Untile the first modification mantain the color standard
        if(!first_change_color){ setStandardColor(); }
    }

    void onApplicationQuit(){
        setStandardColor();
    }

    public void selectTiles(){
        // Retrieve text
        Dropdown tmp_drop = GameObject.Find("Material Selector").GetComponent<Dropdown>();
        material_selector = tmp_drop.options[tmp_drop.value].text;

        // Set sliders values
        switch(material_selector){
            case "UP":    Color.RGBToHSV(material_prefab_UP.color, out hue, out saturation, out value);    break;
            case "DOWN":  Color.RGBToHSV(material_prefab_DOWN.color, out hue, out saturation, out value);  break;
            case "LEFT":  Color.RGBToHSV(material_prefab_LEFT.color, out hue, out saturation, out value);  break;
            case "RIGHT": Color.RGBToHSV(material_prefab_RIGHT.color, out hue, out saturation, out value); break;
        }
        setSliderValues();

    }

    public void changeHue(){
        // Modify component
        hue = slider_hue.value;

        // Abilitate change color
        abilitate_change_color = true;
    }

    public void changeSaturation(){
        // Modify component
        saturation = slider_saturation.value;

        // Abilitate change color
        abilitate_change_color = true;
    }

    public void changeValue(){
        // Modify component
        value = slider_value.value;

        // Abilitate change color
        abilitate_change_color = true;
    }

    public void setStandardColor(){
        material_prefab_UP.color = Color.green;
        material_prefab_DOWN.color = Color.red;
        material_prefab_LEFT.color = Color.blue;
        material_prefab_RIGHT.color = Color.yellow;
    }

    public void setSliderValues(){
        slider_hue.value = hue;
        slider_saturation.value = saturation;
        slider_value.value = value;
    }
}

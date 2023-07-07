using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TraitUpdater : MonoBehaviour
{
    [SerializeField] private TMP_Text _traitName;
    [SerializeField] private Slider _traitSlider;
    [SerializeField] private Image _traitBackgroundImage;
    [SerializeField] private Image _traitFillImage;

    public void InitializeTrait(TraitScriptableObject traitValues)
    {
        _traitName.text = traitValues.traitName;

        Color traitColor = traitValues.color;

        traitColor.a = 1;
        
        _traitFillImage.color = traitColor;

        // Darken background
        float hue, saturation, value;
        Color.RGBToHSV(traitValues.color, out hue, out saturation, out value);
        _traitBackgroundImage.color = Color.HSVToRGB(hue, saturation, 0.6f);

        UpdateTraitValue(1f);
    }
    
    public void UpdateTraitValue(float newTraitValue)
    {
        _traitSlider.value = newTraitValue;
    }
}

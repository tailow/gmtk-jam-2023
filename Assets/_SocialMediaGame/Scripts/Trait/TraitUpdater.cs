using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TraitUpdater : MonoBehaviour
{
    [SerializeField] private TMP_Text _traitName;
    [SerializeField] private Slider _traitSlider;
    [SerializeField] private Image _traitBackgroundImage;
    [SerializeField] private Image _traitFillImage;

    public TraitScriptableObject TraitScriptableObject;

    private float _traitValue;
    private float _traitDrain;

    bool _isFlashingRed = false;
    Color traitColor;
  
    private void Update()
    {
        UpdateTraitValue(-_traitDrain * GameManager.Instance.TraitDrainMultiplier * 0.1f * Time.deltaTime); // trait drain
        if (_isFlashingRed)
        {
            _traitFillImage.color = Color.Lerp(Color.red,traitColor, Mathf.PingPong(Time.time, 1));
        }
        else
        {
            _traitFillImage.color = traitColor;
        }
    }

    public void InitializeTrait(PersonScriptableObject.PersonalTrait traitData)
    {
        TraitScriptableObject = traitData.traitScriptableObject;

        _traitDrain = traitData.drainRate;

        _traitName.text = TraitScriptableObject.traitName;

        // set alpha to 1
        traitColor = TraitScriptableObject.color;
        traitColor.a = 1;
        _traitFillImage.color = traitColor;

        // Darken trait bar background
        float hue, saturation, value;
        Color.RGBToHSV(TraitScriptableObject.color, out hue, out saturation, out value);
        _traitBackgroundImage.color = Color.HSVToRGB(hue, saturation, 0.6f);

        SetTraitValue(1f);
    }

    public void UpdateTraitValue(float amount)
    {
        // flash red if trait value is negative
        if (amount < -_traitDrain * GameManager.Instance.TraitDrainMultiplier * 0.15f * Time.deltaTime)
        {
            FlashRed();
        }

        _traitValue += amount;

        _traitValue = Mathf.Clamp01(_traitValue); ;

        _traitSlider.value = _traitValue;

        if (_traitValue <= 0)
        {
            GameManager.Instance.GameOver();
        }
    }

    public void SetTraitValue(float value)
    {
        _traitValue = Mathf.Clamp01(value);

        _traitSlider.value = _traitValue;
    }

    // change the color of the trait bar to red. SHould be a timed event lasting 0.5 seconds
    // should last for 0.5 seconds even though only called once
    public void FlashRed()
    {
        _isFlashingRed = true;
        Invoke("StopFlashingRed", 1f);
    }
    public void StopFlashingRed()
    {
        _isFlashingRed = false;
    }
}

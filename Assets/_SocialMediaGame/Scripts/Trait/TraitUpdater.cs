using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TraitUpdater : MonoBehaviour
{
    [SerializeField] private TMP_Text _traitName;
    [SerializeField] private Slider _traitSlider;
    [SerializeField] private Slider _traitHighlightSlider;
    [SerializeField] private Image _traitBackgroundImage;
    [SerializeField] private Image _traitFillImage;
    [SerializeField] private Image _traitHighlightFillImage;

    [SerializeField] private float _traitFillSpeed;

    public TraitScriptableObject TraitScriptableObject;

    private float _traitValue;
    private float _traitDrain;
    private float _time;

    bool _isFlashingRed = false;
    Color traitColor;

    Vector3 pointA;
    Vector3 pointB;
    Transform personCardTransform;

    private PersonCard _personCard;
 
    void Start () {
        personCardTransform = transform.parent.parent;
        pointA = transform.eulerAngles + new Vector3 (0f, 0f, 10f);
        pointB = transform.eulerAngles + new Vector3 (0f, 0, -10f);

        _personCard = personCardTransform.GetComponent<PersonCard>();
    }

    private void Update()
    {
        UpdateTraitValue(-_traitDrain * GameManager.Instance.TraitDrainMultiplier * 0.1f * Time.deltaTime); // trait drain

        _traitSlider.value = Mathf.Lerp(_traitSlider.value, _traitValue, Time.deltaTime * _traitFillSpeed);
        
        _time += Time.deltaTime;

        if (_isFlashingRed)
        {
            _traitFillImage.color = Color.Lerp(Color.red,traitColor, Mathf.PingPong(Time.time, 1));
            // get parent of gameObject
            float time = Mathf.PingPong (Time.time * 5f, 1);
            personCardTransform.eulerAngles = Vector3.Lerp (pointA, pointB, time);
        }
        else
        {
            _traitFillImage.color = traitColor;
            personCardTransform.rotation = Quaternion.identity;
        }
    }

    public void InitializeTrait(PersonScriptableObject.PersonalTrait traitData)
    {
        float hue, saturation, value;
        
        TraitScriptableObject = traitData.traitScriptableObject;

        _traitDrain = traitData.drainRate;

        _traitName.text = TraitScriptableObject.traitName;

        // set alpha to 1
        traitColor = TraitScriptableObject.color;
        traitColor.a = 1;
        Color.RGBToHSV(TraitScriptableObject.color, out hue, out saturation, out value);
        
        traitColor = Color.HSVToRGB(hue, 0.5f, 0.9f);;
        
        _traitFillImage.color = Color.HSVToRGB(hue, 0.5f, 0.9f);
        _traitHighlightFillImage.color = Color.HSVToRGB(hue, 0.5f, 0.8f);
        _traitBackgroundImage.color = Color.HSVToRGB(hue, 0.5f, 0.6f);

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

        _traitHighlightSlider.value = _traitValue;

        if (_traitValue <= 0)
        {
            GameManager.Instance.GameOver();
        }
    }

    public void SetTraitValue(float value)
    {
        _traitValue = Mathf.Clamp01(value);
        
        _traitHighlightSlider.value = _traitValue;
        _traitSlider.value = _traitValue;
    }
    
    public float GetTraitValue()
    {
        return _traitValue;
    }

    // change the color of the trait bar to red. SHould be a timed event lasting 0.5 seconds
    // should last for 0.5 seconds even though only called once
    public void FlashRed()
    {
        _isFlashingRed = true;
        Invoke("StopFlashingRed", 1f);
        // do a shake effect on the gameObject by rotating it back and forth
    }
    public void StopFlashingRed()
    {
        _isFlashingRed = false;
    }
}

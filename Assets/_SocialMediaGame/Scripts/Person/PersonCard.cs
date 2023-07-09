using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PersonCard : MonoBehaviour
{
    public PersonScriptableObject PersonData;
    
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private Image _personImage;
    [SerializeField] private Image _iconImage;
    [SerializeField] private Transform _traitParent;

    public float ShakeAmount;

    [SerializeField] private float _shakeSpeed = 100f;

    private string wrongSound = "incorrect";
    private string correctSound = "correct";

    private void Update()
    {
        float lowestTrait = GetLowestTraitValue();
        
        ShakeAmount = Mathf.Clamp((1 - lowestTrait - 0.8f) * 10f, 0, 10);
            
        Shake();
    }

    private void Shake()
    {
        transform.localPosition = new Vector3(Mathf.Sin(Time.time * _shakeSpeed) * ShakeAmount, Mathf.Cos(Time.time * _shakeSpeed / 2) * ShakeAmount, 0);
    }

    public float GetLowestTraitValue()
    {
        float lowestValue = 1f;
        
        for (int i = 0; i < _traitParent.childCount; i++)
        {
            lowestValue = Mathf.Min(_traitParent.GetChild(i).GetComponent<TraitUpdater>().GetTraitValue(), lowestValue);
        }

        return lowestValue;
    }
    
    public void UpdatePersonCard(PersonScriptableObject personData){
        PersonData = personData;
        _nameText.text = personData.personName;
        
        // check if anything gets dropped on this, should probably check which object gets dropped
        GetComponent<DropTarget>().DropEvent.AddListener(ConsumeContent);

        if (personData.sprite != null)
        {
            _personImage.sprite = personData.sprite;
        }

        // Loop through trait objects and apply values from scriptable object
        for (int i = 0; i < _traitParent.childCount; i++)
        {
            if (i < personData.traits.Count)
            {
                _traitParent.GetChild(i).GetComponent<TraitUpdater>().InitializeTrait(personData.traits[i]);
                
                _traitParent.GetChild(i).gameObject.SetActive(true);
            }

            else
            {
                _traitParent.GetChild(i).gameObject.SetActive(false); // if there are too many trait objects
            }
        }
        if(personData.icon != null)
        {
            _iconImage.sprite = personData.icon;
            // also set iconimages alpha to be 1
            _iconImage.color = new Color(_iconImage.color.r, _iconImage.color.g, _iconImage.color.b, 1);

        }
    }

    // Add content values to person trait bars
    public void ConsumeContent(GameObject contentObject)
    {
        if (contentObject == null) return;

        ContentScriptableObject contentData = contentObject.GetComponent<ContentCard>().ContentData;
        int allignedTraits = 0;

        foreach (Transform trait in _traitParent)
        {
            TraitScriptableObject traitData = trait.GetComponent<TraitUpdater>().TraitScriptableObject;
            
            float traitValueIncrease = 0f;

            foreach (ContentScriptableObject.WeightedTrait weightedTrait in contentData.traits)
            {
                if (weightedTrait.traitScriptableObject.traitName == traitData.traitName)
                {
                    traitValueIncrease += weightedTrait.weight * GameManager.Instance.TraitIncreaseMultiplier;
                    allignedTraits++;
                }
            }
            trait.GetComponent<TraitUpdater>().UpdateTraitValue(traitValueIncrease);
        }

        // if no traits are alligned decrease all trait values by 1
        if (allignedTraits == 0)
        {
            float weightedTraitValueDecrease = -1f * GameManager.Instance.TraitIncreaseMultiplier;
            foreach (Transform trait in _traitParent)
            {
                trait.GetComponent<TraitUpdater>().UpdateTraitValue(weightedTraitValueDecrease);
            }
            GameManager.Instance.PlaySound(wrongSound);
        }else{
            GameManager.Instance.PlaySound(correctSound);
        }

        GameManager.Instance.ReplaceContentCard();
    }
}

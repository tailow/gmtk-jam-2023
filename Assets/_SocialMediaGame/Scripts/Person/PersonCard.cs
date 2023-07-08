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
    [SerializeField] private Transform _traitParent;

    private void Start()
    {
        _nameText.text = PersonData.personName;
        
        // check if anything gets dropped on this, should probably check which object gets dropped
        GetComponent<DropTarget>().DropEvent.AddListener(ConsumeContent);

        if (PersonData.sprite != null)
        {
            _personImage.sprite = PersonData.sprite;
        }

        // Loop through trait objects and apply values from scriptable object
        for (int i = 0; i < _traitParent.childCount; i++)
        {
            if (i < PersonData.traits.Count)
            {
                _traitParent.GetChild(i).GetComponent<TraitUpdater>().InitializeTrait(PersonData.traits[i]);
                
                _traitParent.GetChild(i).gameObject.SetActive(true);
            }

            else
            {
                _traitParent.GetChild(i).gameObject.SetActive(false); // if there are too many trait objects
            }
        }
    }

    // Add content values to person trait bars
    public void ConsumeContent(GameObject contentObject)
    {
        if (contentObject == null) return;

        ContentScriptableObject contentData = contentObject.GetComponent<ContentCard>().ContentData;

        foreach (Transform trait in _traitParent)
        {
            TraitScriptableObject traitData = trait.GetComponent<TraitUpdater>().TraitScriptableObject;
            
            float traitValueIncrease = 0f;

            foreach (ContentScriptableObject.WeightedTrait weightedTrait in contentData.traits)
            {
                if (weightedTrait.traitScriptableObject.traitName == traitData.traitName)
                {
                    traitValueIncrease += weightedTrait.weight * GameManager.Instance.TraitIncreaseMultiplier;
                }
            }
            
            trait.GetComponent<TraitUpdater>().UpdateTraitValue(traitValueIncrease);
        }
        
        GameManager.Instance.ReplaceContentCard();
    }
}

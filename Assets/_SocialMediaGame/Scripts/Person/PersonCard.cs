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

        if (PersonData.sprite != null)
        {
            _personImage.sprite = PersonData.sprite;
        }

        // Loop through trait objects and apply values from scriptable object
        for (int i = 0; i < _traitParent.childCount; i++)
        {
            if (i < PersonData.traits.Count)
            {
                _traitParent.GetChild(i).GetComponent<TraitUpdater>().InitializeTrait(PersonData.traits[i].traitScriptableObject);
                
                _traitParent.GetChild(i).gameObject.SetActive(true);
            }

            else
            {
                _traitParent.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ContentCard : MonoBehaviour
{
    public ContentScriptableObject ContentData;
    
    [SerializeField] private TMP_Text _titleText;
    [SerializeField] private TMP_Text _descriptionText;
    [SerializeField] private Image _contentImage;

    private void Start()
    {
        _titleText.text = ContentData.title;

        if (ContentData.sprite != null)
        {
            _contentImage.sprite = ContentData.sprite;
        }

        string descriptionText = ContentData.description;

        // Add tags to description
        foreach (ContentScriptableObject.WeightedTrait trait in ContentData.traits)
        {
            if (trait.weight > 0.5f)
            {
                descriptionText += " #" + trait.traitScriptableObject.traitName.ToLower();
            }
        }

        _descriptionText.text = descriptionText;
    }
}

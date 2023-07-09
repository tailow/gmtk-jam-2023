using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private Sprite[] _tutorialSprites;

    // change image on Ui
    [SerializeField] private GameObject _image;
    private Image sR;

    [SerializeField] private string[] _tutorialTexts;
    [SerializeField] private TMP_Text _tutorialText;

    private int _selectedIndex = 0;

    private void Start() {
        sR = _image.GetComponent<Image>();
        updateSprite();

    }

    public void NextImage()
    {
        if (_selectedIndex < _tutorialSprites.Length - 1)
        {
            _selectedIndex++;
        }
        updateSprite();
    }
    
    public void PreviousImage()
    {
        if (_selectedIndex > 0)
        {
            _selectedIndex--;
        }
        updateSprite();
    }

    private void updateSprite()
    {
        sR.sprite = _tutorialSprites[_selectedIndex];
        _tutorialText.text = _tutorialTexts[_selectedIndex];
    }
    
}

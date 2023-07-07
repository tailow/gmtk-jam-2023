using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Content", menuName = "ScriptableObjects/Content")]
public class ContentScriptableObject : ScriptableObject
{
    [System.Serializable]
    public class WeightedTrait
    {
        public TraitScriptableObject traitScriptableObject;

        public float weight;
    }

    public string title;
    public string description;
    public Sprite sprite;
    
    public List<WeightedTrait> traits;
}

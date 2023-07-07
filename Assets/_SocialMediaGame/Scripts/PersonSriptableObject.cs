using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Person", menuName = "ScriptableObjects/Person")]
public class PersonSriptableObject : ScriptableObject
{
    [System.Serializable]
    public class Trait
    {
        public TraitScriptableObject traitScriptableObject;

        public float drainRate;
    }
    
    public string personName;

    public Sprite personSprite;

    public float globalDrainRate;
    
    public List<Trait> traits;
}

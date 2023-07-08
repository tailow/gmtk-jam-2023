using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Person", menuName = "ScriptableObjects/Person")]
public class PersonScriptableObject : ScriptableObject
{
    [System.Serializable]
    public class PersonalTrait
    {
        public TraitScriptableObject traitScriptableObject;

        public float drainRate;
    }
    
    public string personName;

    public Sprite sprite;

    public Sprite icon;

    public float globalDrainRate;
    
    public List<PersonalTrait> traits;
}

using UnityEngine;

[CreateAssetMenu(fileName = "Trait", menuName = "ScriptableObjects/Trait")]
public class TraitScriptableObject : ScriptableObject
{
    public string traitName;

    public Color color;
}

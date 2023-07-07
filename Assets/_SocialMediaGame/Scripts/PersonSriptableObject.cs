using UnityEngine;

[CreateAssetMenu(fileName = "Person", menuName = "ScriptableObjects/Person")]
public class PersonSriptableObject : ScriptableObject
{
    public string personName;

    public Sprite personSprite;

    public float globalDrainRate;
}

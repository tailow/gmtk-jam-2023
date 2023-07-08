using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [HideInInspector]
    public GameObject CurrentDraggingObject;

    [SerializeField] private GameObject _contentCardPrefab;
    [SerializeField] private Transform _contentCardParent;

    public float TraitDrainMultiplier;
    public float TraitIncreaseMultiplier;

    private Object[] _contentScriptableObjects;

    private void Start()
    {
        // apparently not recommended, but it works, loads all scriptable objects into array
        _contentScriptableObjects = Resources.LoadAll("ScriptableObjects/Content", typeof(ContentScriptableObject));
    }

    public void GameOver()
    {
        Debug.Log("Game over!");
    }

    public void ReplaceContentCard()
    {
        DeleteContentCard();
        
        InstantiateContentCard();
    }
    
    private void InstantiateContentCard()
    {
        GameObject contentCardObject = Instantiate(_contentCardPrefab, _contentCardParent.position, Quaternion.identity,
            _contentCardParent);

        // pick random content card from scriptable objects
        ContentScriptableObject randomContentData = (ContentScriptableObject)_contentScriptableObjects[Random.Range(0, _contentScriptableObjects.Length)];
        
        contentCardObject.GetComponent<ContentCard>().UpdateContentCard(randomContentData);
    }

    // currently destroys all content cards under parent
    private void DeleteContentCard()
    {
        foreach (Transform child in _contentCardParent)
        {
            Destroy(child.gameObject);
        }
    }
}

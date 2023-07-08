using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [HideInInspector]
    public GameObject CurrentDraggingObject;

    [SerializeField] private GameObject _contentCardPrefab;
    [SerializeField] private Transform _contentCardParent;

    private Object[] _contentScriptableObjects;

    private void Start()
    {
        _contentScriptableObjects = Resources.LoadAll("ScriptableObjects/Content", typeof(ContentScriptableObject));
    }

    public void TrashContentCard()
    {
        DeleteContentCard();
        
        InstantiateContentCard();
    }
    
    private void InstantiateContentCard()
    {
        GameObject contentCardObject = Instantiate(_contentCardPrefab, _contentCardParent.position, Quaternion.identity,
            _contentCardParent);

        contentCardObject.GetComponent<ContentCard>().ContentData = (ContentScriptableObject)_contentScriptableObjects[Random.Range(0, _contentScriptableObjects.Length)];
    }

    private void DeleteContentCard()
    {
        foreach (Transform child in _contentCardParent)
        {
            Destroy(child.gameObject);
        }
    }
}

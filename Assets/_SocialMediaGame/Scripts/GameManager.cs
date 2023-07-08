using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [HideInInspector]
    public GameObject CurrentDraggingObject;

    [SerializeField] private GameObject _contentCardPrefab;
    [SerializeField] private Transform _contentCardParent;
    [SerializeField] private GameObject _playerCardPrefab;
    [SerializeField] private Transform _playerCardGrid;

    public float TraitDrainMultiplier;
    public float TraitIncreaseMultiplier;

    private float gameTimer;
    private int playerCount = 0;

    private Object[] _contentScriptableObjects;
    private Object[] _easyPersonScriptableObjects;
    private Object[] _mediumPersonScriptableObjects;
    private Object[] _hardPersonScriptableObjects;

    private void Start()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        // apparently not recommended, but it works, loads all scriptable objects into array
        _contentScriptableObjects = Resources.LoadAll("ScriptableObjects/Content", typeof(ContentScriptableObject));
        // yeah we will still do this for now
        _easyPersonScriptableObjects = Resources.LoadAll("ScriptableObjects/People/Easy", typeof(PersonScriptableObject));
        Debug.Log(_easyPersonScriptableObjects.Length);
        _mediumPersonScriptableObjects = Resources.LoadAll("ScriptableObjects/People/Medium", typeof(PersonScriptableObject));
        _hardPersonScriptableObjects = Resources.LoadAll("ScriptableObjects/People/Hard", typeof(PersonScriptableObject));

        InstantiatePlayerCard(0);
    }

    private void Update() {
        gameTimer += Time.deltaTime;
        if(gameTimer > 30 && playerCount< 3){
            gameTimer = 0;
            InstantiatePlayerCard(0);
        }
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

    private void InstantiatePlayerCard(int difficulty){
        // instantiate player card inside of the playercard grid objects grid component
        GameObject playerCardObject = Instantiate(_playerCardPrefab, _playerCardGrid);
        playerCount++;

        // pick random content card from scriptable objects
        // FOR SOME REASON RANDOM.RANGE IS NOT RANDOM AND ALWAYS GIVES TEH SAME PERSON THE FIRST TIME ROUND.
        PersonScriptableObject randomPersonData;
        switch(difficulty){
            case 0:
                randomPersonData = (PersonScriptableObject)_easyPersonScriptableObjects[Random.Range(0, _easyPersonScriptableObjects.Length)];
                break;
            case 1:
                randomPersonData = (PersonScriptableObject)_mediumPersonScriptableObjects[Random.Range(0, _mediumPersonScriptableObjects.Length)];
                break;
            case 2:
                randomPersonData = (PersonScriptableObject)_hardPersonScriptableObjects[Random.Range(0, _hardPersonScriptableObjects.Length)];
                break;
            default:
                randomPersonData = (PersonScriptableObject)_easyPersonScriptableObjects[Random.Range(0, _easyPersonScriptableObjects.Length)];
                break;
        }
        
        playerCardObject.GetComponent<PersonCard>().UpdatePersonCard(randomPersonData);
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

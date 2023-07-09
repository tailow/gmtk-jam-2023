using UnityEngine;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private GameObject _contentCardPrefab;

    [SerializeField]
    private Transform _contentCardParent;

    [SerializeField]
    private GameObject _playerCardPrefab;

    [FormerlySerializedAs("_playerCardGrid")] [SerializeField]
    private Transform _personCardGrid;

    public float TraitDrainMultiplier;
    public float TraitIncreaseMultiplier;

    private float _lowestTraitValue = 1f;

    private int _previousContentCardIndex = -1;

    private int LOWEST_DIFFICULTY = 0;
    private int CURRENT_HIGHTEST_DIFFICULTY = 0;
    private int HIGHEST_DIFFICULTY = 2;
    private float playerSpawnTimer = 0f;
    private int playerCount = 0;
    List<GameObject> playerList = new List<GameObject>();

    private Object[] _contentScriptableObjects;
    private PersonScriptableObject[] _easyPersonScriptableObjects;
    private PersonScriptableObject[] _mediumPersonScriptableObjects;
    private PersonScriptableObject[] _hardPersonScriptableObjects;

    private void Start()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        // apparently not recommended, but it works, loads all scriptable objects into array
        _contentScriptableObjects = Resources.LoadAll(
            "ScriptableObjects/Content",
            typeof(ContentScriptableObject)
        );
        _easyPersonScriptableObjects = Resources.LoadAll<PersonScriptableObject>(
            "ScriptableObjects/People/Easy"
        );
        _mediumPersonScriptableObjects = Resources.LoadAll<PersonScriptableObject>(
            "ScriptableObjects/People/Medium"
        );
        _hardPersonScriptableObjects = Resources.LoadAll<PersonScriptableObject>(
            "ScriptableObjects/People/Hard"
        );

        InstantiatePlayerCard(0);
        InstantiateContentCard();
    }

    private void Update()
    {
        playerSpawnTimer += Time.deltaTime;

        // spawn a new player every 30 seconds
        // if player list full despawn one player
        if (playerSpawnTimer > 30 && playerCount < 3)
        {
            playerSpawnTimer = 0;
            InstantiatePlayerCard(Random.Range(LOWEST_DIFFICULTY, CURRENT_HIGHTEST_DIFFICULTY));
        }
        if (playerSpawnTimer > 60)
        {
            // remove random player and then spawn a new one
            playerSpawnTimer = 0;
            int randomPlayer = Random.Range(0, playerCount);
            // remove from list
            Destroy(playerList[randomPlayer]);

            // increase difficulty
            // maybe add global drain also here
            if (CURRENT_HIGHTEST_DIFFICULTY < HIGHEST_DIFFICULTY)
            {
                CURRENT_HIGHTEST_DIFFICULTY += 1;
            }

            playerCount--;
        }
    }

    private float GetLowestTraitValue()
    {
        float lowestValue = 1f;
        
        foreach (Transform personCardParent in _personCardGrid)
        {
            lowestValue = Mathf.Min(personCardParent.GetComponentInChildren<PersonCard>().GetLowestTraitValue(), lowestValue);
        }

        return lowestValue;
    }

    public void GameOver()
    {
        Debug.Log("Game over!");
        SceneManager.LoadScene("DeathScene");
    }

    public void ReplaceContentCard()
    {
        DeleteContentCard();

        InstantiateContentCard();
    }

    private void InstantiatePlayerCard(int difficulty)
    {
        GameObject playerCardObject = Instantiate(_playerCardPrefab, _personCardGrid);
        playerList.Add(playerCardObject);
        playerCount++;

        PersonScriptableObject randomPersonData;
        switch (difficulty)
        {
            case 0:
                randomPersonData = _easyPersonScriptableObjects[
                    Random.Range(0, _easyPersonScriptableObjects.Length)
                ];
                break;
            case 1:
                randomPersonData = _mediumPersonScriptableObjects[
                    Random.Range(0, _mediumPersonScriptableObjects.Length)
                ];
                break;
            case 2:
                randomPersonData = _hardPersonScriptableObjects[
                    Random.Range(0, _hardPersonScriptableObjects.Length)
                ];
                break;
            default:
                randomPersonData = _easyPersonScriptableObjects[
                    Random.Range(0, _easyPersonScriptableObjects.Length)
                ];
                break;
        }

        playerCardObject.GetComponentInChildren<PersonCard>().UpdatePersonCard(randomPersonData);
    }

    private void InstantiateContentCard()
    {
        GameObject contentCardObject = Instantiate(
            _contentCardPrefab,
            _contentCardParent.position,
            Quaternion.identity,
            _contentCardParent
        );

        int randomIndex;

        while (true)
        {
            randomIndex = Random.Range(0, _contentScriptableObjects.Length - 1);

            if (randomIndex != _previousContentCardIndex) break;
        }

        _previousContentCardIndex = randomIndex;

        // pick random content card from scriptable objects
        ContentScriptableObject randomContentData = (ContentScriptableObject)
            _contentScriptableObjects[randomIndex];

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

    public void PlaySound(string eventName)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/" + eventName);
    }
}

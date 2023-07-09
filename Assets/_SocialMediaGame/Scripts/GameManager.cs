using UnityEngine;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using System.Linq;
using TMPro;
using System;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private GameObject _contentCardPrefab;

    [SerializeField]
    private Transform _contentCardParent;

    [SerializeField]
    private GameObject _playerCardPrefab;

    [FormerlySerializedAs("_playerCardGrid")]
    [SerializeField]
    private Transform _personCardGrid;

    [SerializeField] private TMPro.TMP_Text _timerText;
    private float _gameTimer;

    public float TraitDrainMultiplier;
    public float TraitIncreaseMultiplier;

    private float _lowestTraitValue = 1f;

    private int _previousContentCardIndex = -1;

    private int CURRENT_DIFFICULTY = 0;
    private int HIGHEST_DIFFICULTY = 2;
    private float playerSpawnTimer = 0f;

    private UnityEngine.Object[] _contentScriptableObjects;

    private PersonScriptableObject[] _easyPersonScriptableObjects;
    private PersonScriptableObject[] _mediumPersonScriptableObjects;
    private PersonScriptableObject[] _hardPersonScriptableObjects;

    private PersonScriptableObject[][] allPersons;

    private void Start()
    {
        UnityEngine.Random.InitState(System.DateTime.Now.Millisecond);
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

        allPersons = new PersonScriptableObject[][]
        {
            _easyPersonScriptableObjects,
            _mediumPersonScriptableObjects,
            _hardPersonScriptableObjects
        };

        InstantiatePlayerCard(0);
        InstantiateContentCard();
    }

    private void Update()
    {
        playerSpawnTimer += Time.deltaTime;
        _gameTimer += Time.deltaTime;
        TimeSpan timeSpan = TimeSpan.FromSeconds(_gameTimer);
        _timerText.text = "Time: " + timeSpan.ToString("mm':'ss");
        // spawn a new player every 30 seconds
        // if player list full despawn one player
        int playerCount = GetPlayerCount();

        if (playerSpawnTimer > 30 && playerCount < 3)
        {
            playerSpawnTimer = 0;
            InstantiatePlayerCard(CURRENT_DIFFICULTY);
        }
        else if (playerSpawnTimer > 35)
        {
            // remove random player and then spawn a new one
            playerSpawnTimer = 10;
            int randomPlayer = UnityEngine.Random.Range(0, playerCount);

            // Destroy random child of playergrid
            Destroy(_personCardGrid.GetChild(randomPlayer).gameObject);

            // increase difficulty
            // maybe add global drain also here
            if (CURRENT_DIFFICULTY < HIGHEST_DIFFICULTY)
            {
                IncreaseDifficulty();
            }
            else
            {
                Debug.Log("Already at highest difficulty");
                TraitDrainMultiplier += 0.025f;
            }
        }
    }

    private float GetLowestTraitValue()
    {
        float lowestValue = 1f;

        foreach (Transform personCardParent in _personCardGrid)
        {
            lowestValue = Mathf.Min(
                personCardParent.GetComponentInChildren<PersonCard>().GetLowestTraitValue(),
                lowestValue
            );
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

        PersonScriptableObject personData = GetPersonDataThatDoesNotExistInGrid(difficulty);
        playerCardObject.GetComponentInChildren<PersonCard>().UpdatePersonCard(personData);
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
            randomIndex = UnityEngine.Random.Range(0, _contentScriptableObjects.Length - 1);

            if (randomIndex != _previousContentCardIndex)
                break;
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

    // Pasta Carbonara
    public void IncreaseDifficulty()
    {
        bool playersAreOfCurrentDifficulty = true;
        foreach (Transform child in _personCardGrid)
        {
            PersonCard personCard = child.GetComponentInChildren<PersonCard>();
            PersonScriptableObject personData = personCard.PersonData;
            if (!allPersons[CURRENT_DIFFICULTY].Contains(personData))
            {
                playersAreOfCurrentDifficulty = false;
                break;
            }
        }
        if (playersAreOfCurrentDifficulty)
        {
            if (CURRENT_DIFFICULTY < HIGHEST_DIFFICULTY)
            {
                CURRENT_DIFFICULTY++;
            }
            Debug.Log("Difficulty increased to " + CURRENT_DIFFICULTY);
        }
    }

    public int GetPlayerCount()
    {
        return _personCardGrid.childCount;
    }

    public PersonScriptableObject GetPersonDataThatDoesNotExistInGrid(int difficulty)
    {
        PersonScriptableObject randomPersonData;
        while (true)
        {
            switch (difficulty)
            {
                case 0:
                    randomPersonData = _easyPersonScriptableObjects[
                        UnityEngine.Random.Range(0, _easyPersonScriptableObjects.Length)
                    ];
                    break;
                case 1:
                    randomPersonData = _mediumPersonScriptableObjects[
                        UnityEngine.Random.Range(0, _mediumPersonScriptableObjects.Length)
                    ];
                    break;
                case 2:
                    randomPersonData = _hardPersonScriptableObjects[
                        UnityEngine.Random.Range(0, _hardPersonScriptableObjects.Length)
                    ];
                    break;
                default:
                    randomPersonData = _easyPersonScriptableObjects[
                        UnityEngine.Random.Range(0, _easyPersonScriptableObjects.Length)
                    ];
                    break;
            }
            bool DuplicatePerson = false;
            for (int i = 0; i < GetPlayerCount(); i++)
            {
                if (
                    _personCardGrid.GetChild(i).GetComponentInChildren<PersonCard>().PersonData
                    == randomPersonData
                )
                {
                    DuplicatePerson = true;
                }
            }
            if (!DuplicatePerson)
            {
                break;
            }
        }
        return randomPersonData;
    }
}

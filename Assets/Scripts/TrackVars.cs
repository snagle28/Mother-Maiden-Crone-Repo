using UnityEngine;
using Yarn.Unity;
//https://docs.yarnspinner.dev/api/csharp/yarn/yarn.memoryvariablestore/yarn.memoryvariablestore.trygetvalue

public class TrackVars : MonoBehaviour
{
    public InMemoryVariableStorage variableStorage;

    [Header("Character Presence Animation")]
    [SerializeField] private GameObject emGameObject;
    [SerializeField] private GameObject prudenceGameObject;
    [SerializeField] private GameObject johnGameObject;
    [SerializeField] private GameObject danielGameObject;
    [SerializeField] private GameObject constableGameObject;
    [SerializeField] private GameObject josephGameObject;
    [SerializeField] private GameObject estherGameObject;
    [SerializeField] private GameObject lauraGameObject;
    
    //[SerializeField] private string animationTriggerName = "CharacterEnter";

    
    int previousHysteria = int.MinValue;
    int prevPrudenceFavor = int.MinValue;
    int prevEstherFavor = int.MinValue;
    int prevRuthFavor = int.MinValue;
    
    
    // Track previous states of presence variables
    bool prevEMPresent = false;
    bool prevPrudencePresent = false;
    bool prevJohnPresent = false;
    bool prevDanielPresent = false;
    bool prevConstablePresent = false;
    bool prevJosephPresent = false;
    bool prevEstherPresent = false;
    bool prevLauraPresent = false;


    void Start()
    {
        variableStorage = FindObjectOfType<InMemoryVariableStorage>();
    }

    void Update()
    {
        /*
         * hysteria system
         */
        if (variableStorage.TryGetValue("$hysteria", out float hysteriaFloat))
        {
            int currentHysteria = Mathf.RoundToInt(hysteriaFloat);

            if (currentHysteria != previousHysteria)
            {
                Debug.Log("Hysteria changed: " + currentHysteria);
                previousHysteria = currentHysteria;
            }
        }
        else
        {
            
            // Debug.Log("No $hysteria var in storage yet");
        }
        
        /*
         * PrudenceFavor
         */
        if (variableStorage.TryGetValue("$PrudenceFavor", out float PrudenceFavorFloat))
        {
            int currentPruFavor = Mathf.RoundToInt(PrudenceFavorFloat);

            if (currentPruFavor != prevPrudenceFavor)
            {
                Debug.Log("Prudence favor changed: " + currentPruFavor);
                prevPrudenceFavor = currentPruFavor;
            }
        }
        else
        {
            // Debug.Log("No $PrudenceFavor variable in storage yet.");
        }
        
        /*
         * Ruth
         */
        if (variableStorage.TryGetValue("$RuthFavor", out float RuthFavorFloat))
        {
            int currentRuthFavor = Mathf.RoundToInt(RuthFavorFloat);

            if (currentRuthFavor != prevRuthFavor)
            {
                Debug.Log("Ruth favor changed: " + currentRuthFavor);
                prevRuthFavor = currentRuthFavor;
            }
        }
        else
        {
            // Debug.Log("No $PrudenceFavor variable in storage yet.");
        }
        
        /*
         * EstherFavor
         */
        if (variableStorage.TryGetValue("$EstherFavor", out float EstherFavorFloat))
        {
            int currentEstherFavor = Mathf.RoundToInt(EstherFavorFloat);

            if (currentEstherFavor != prevRuthFavor)
            {
                Debug.Log("Esther favor changed: " + currentEstherFavor);
                prevRuthFavor = currentEstherFavor;
            }
        }
        else
        {
            // Debug.Log("No $PrudenceFavor variable in storage yet.");
        }
        
        /*
         * Tracking charcter appearence.
         */
        CheckPV("$EMPresent", ref prevEMPresent, "EM", emGameObject);
        CheckPV("$PrudencePresent", ref prevPrudencePresent, "Prudence", prudenceGameObject);
        CheckPV("$JohnPresent", ref prevJohnPresent, "John", johnGameObject);
        CheckPV("$DanielPresent", ref prevDanielPresent, "Daniel", danielGameObject);
        CheckPV("$ConstablePresent", ref prevConstablePresent, "Constable", constableGameObject);
        CheckPV("$JosephPresent", ref prevJosephPresent, "Joseph", josephGameObject);
        CheckPV("$EstherPresent", ref prevEstherPresent, "Esther", estherGameObject);
        CheckPV("$LauraPresent", ref prevLauraPresent, "Laura", lauraGameObject);
        
    }
    
    void CheckPV(string variableName, ref bool previousValue, string characterName, GameObject targetObject)
    {
        if (variableStorage.TryGetValue(variableName, out bool currentValue))
        {
            // Check if the value changed from false to true
            if (currentValue && !previousValue)
            {
                Debug.Log($"{characterName} is present");
                PlayAnim(targetObject, characterName);
                previousValue = true; // Update immediately to prevent repeated calls
            }
            // Check if the value changed from true to false
            else if (!currentValue && previousValue)
            {
                Debug.Log($"{characterName} is leaving");
                PlayReverse(targetObject, characterName);
                previousValue = false;

            }
        }
    }
    
    void PlayAnim(GameObject targetObject, string characterName)
    {
        if (targetObject != null)
        {
            Animator animator = targetObject.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("risingUp");
            }
            else
            {
                Debug.LogWarning($"No Animator component found on {characterName}'s GameObject!");
            }
        }
        else
        {
            Debug.LogWarning($"No target object for {characterName}");
        }
    }
    
    void PlayReverse(GameObject targetObject, string characterName)
    {
        if (targetObject != null)
        {
            Animator animator = targetObject.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("goingDown");
            }
            else
            {
                Debug.LogWarning($"No Animator component found on {characterName}'s GameObject!");
            }
        }
        else
        {
            Debug.LogWarning($"No target object for {characterName}");
        }
    }


    
}
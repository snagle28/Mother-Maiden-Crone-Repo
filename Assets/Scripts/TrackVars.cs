using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;
using System.Collections.Generic;
//https://docs.yarnspinner.dev/api/csharp/yarn/yarn.memoryvariablestore/yarn.memoryvariablestore.trygetvalue

public class TrackVars : MonoBehaviour
{
    //added audio source ref for paper sounds
    public AudioSource audioSource;
    [Header("Yarn References")]
    [SerializeField] private DialogueRunner dialogueRunner;
    public InMemoryVariableStorage variableStorage;

    [Header("Character Presence Animation")]
    [SerializeField] private GameObject oliverGameObject;
    [SerializeField] private GameObject lizzyGameObject;
    [SerializeField] private GameObject fatherGameObject;
    [SerializeField] private GameObject prudenceGameObject;
    [SerializeField] private GameObject johnGameObject;
    [SerializeField] private GameObject danielGameObject;
    [SerializeField] private GameObject constableGameObject;
    [SerializeField] private GameObject josephGameObject;
    [SerializeField] private GameObject estherGameObject;
    [SerializeField] private GameObject lauraGameObject;
    [SerializeField] private GameObject title1Object;
    [SerializeField] private GameObject title2Object;
    [SerializeField] private GameObject title3Object;
    [SerializeField] private GameObject EndObject;
    [SerializeField] private GameObject ruthGameObject;

    [SerializeField] private GameObject endingGameObject;

    [SerializeField] private GameObject physicalEvidence;



    int previousHysteria = int.MinValue;
    int prevPrudenceFavor = int.MinValue;
    int prevEstherFavor = int.MinValue;
    int prevRuthFavor = int.MinValue;


    // Track previous states of presence variables
    bool prevOliverPresent = false;
    bool prevLizzyPresent = false;
    bool prevFatherPresent = false;
    bool prevPrudencePresent = false;
    bool prevJohnPresent = false;
    bool prevDanielPresent = false;
    bool prevConstablePresent = false;
    bool prevJosephPresent = false;
    bool prevEstherPresent = false;
    bool prevLauraPresent = false;
    bool prevRuthPresent = false;

    bool prevTitle1Present = false;
    bool prevTitle2Present = false;
    bool prevTitle3Present = false;
    bool prevEndingTitlePresent = false;
    bool hasGasped = false;
    bool hasWhispers = false;

    bool prevEnding = false;

    // Add these for bobbing control
    [Header("Bobbing Settings")]
    [SerializeField] private float bobAmplitude = 0.2f;  // Height of bob (e.g., 0.2 units)
    [SerializeField] private float bobSpeed = 2f;       // Speed of bob cycle

    private List<GameObject> bobbingObjects = new List<GameObject>();  // Tracks currently bobbing characters
    private Dictionary<GameObject, Vector3> originalPositions = new Dictionary<GameObject, Vector3>();  // Stores base positions


    private Vector2 startPos;
    void Start()
    {
        if (dialogueRunner == null)
            dialogueRunner = FindObjectOfType<DialogueRunner>();

        // Prefer the DialogueRunner's storage (so we're reading the same one Yarn writes to)
        if (dialogueRunner != null)
            variableStorage = dialogueRunner.VariableStorage as InMemoryVariableStorage;

        // Fallback to old behavior if needed
        if (variableStorage == null)
            variableStorage = FindObjectOfType<InMemoryVariableStorage>();

        if (variableStorage == null)
            Debug.LogError("TrackVars: No InMemoryVariableStorage found. Presence checks will not work.");

        // Initialize bobbing for any characters already present
        InitializeBobbingIfPresent("$OliverPresent", ref prevOliverPresent, oliverGameObject);
        InitializeBobbingIfPresent("$LizzyPresent", ref prevLizzyPresent, lizzyGameObject);
        InitializeBobbingIfPresent("$FatherPresent", ref prevFatherPresent, fatherGameObject);
        InitializeBobbingIfPresent("$PrudencePresent", ref prevPrudencePresent, prudenceGameObject);
        InitializeBobbingIfPresent("$JohnPresent", ref prevJohnPresent, johnGameObject);
        InitializeBobbingIfPresent("$DanielPresent", ref prevDanielPresent, danielGameObject);
        InitializeBobbingIfPresent("$ConstablePresent", ref prevConstablePresent, constableGameObject);
        InitializeBobbingIfPresent("$JosephPresent", ref prevJosephPresent, josephGameObject);
        InitializeBobbingIfPresent("$EstherPresent", ref prevEstherPresent, estherGameObject);
        InitializeBobbingIfPresent("$LauraPresent", ref prevLauraPresent, lauraGameObject);
        InitializeBobbingIfPresent("$RuthPresent", ref prevRuthPresent, ruthGameObject);
        // Add any others if needed
    }

    private void InitializeBobbingIfPresent(string variableName, ref bool previousValue, GameObject targetObject)
    {
        if (variableStorage.TryGetValue(variableName, out bool currentValue) && currentValue)
        {
            previousValue = true;
            if (targetObject != null && !bobbingObjects.Contains(targetObject))
            {
                bobbingObjects.Add(targetObject);
                originalPositions[targetObject] = targetObject.transform.position;
            }
        }
    }


    public Image image;

    void Update()
    {
        /*
         * hysteria system
         */
        if (variableStorage.TryGetValue("$hysteria", out float hysteriaFloat))
        {
            int currentHysteria = Mathf.RoundToInt(hysteriaFloat);
            //print(currentHysteria);

            if (currentHysteria != previousHysteria)
            {
                //Debug.Log("Hysteria changed: " + currentHysteria);
                previousHysteria = currentHysteria;
                image.fillAmount = (currentHysteria / 100f);  // Fixed capitalization; use 100f for float division
            }
        }
        else
        {

            // Debug.Log("No $hysteria var in storage yet");
        }


        /*
         * PrudenceFavor
         */
        if (variableStorage.TryGetValue("$prudenceFavor", out float PrudenceFavorFloat))
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
        if (variableStorage != null)
        {
            if (variableStorage.TryGetValue("$RuthPresent", out bool ruthPresent))
            {
                if (ruthPresent != prevRuthPresent)
                    Debug.Log($"TrackVars sees $RuthPresent changed -> {ruthPresent}");
            }
            else
            {
                Debug.LogWarning("TrackVars: $RuthPresent not found in variableStorage (name/case or wrong storage).");
            }
        }

        /*
         * EstherFavor
         */
        if (variableStorage.TryGetValue("$estherFavor", out float EstherFavorFloat))
        {
            int currentEstherFavor = Mathf.RoundToInt(EstherFavorFloat);

            if (currentEstherFavor != prevEstherFavor)  // Fixed: Was incorrectly using prevRuthFavor
            {
                Debug.Log("Esther favor changed: " + currentEstherFavor);
                prevEstherFavor = currentEstherFavor;  // Fixed: Update the correct variable
            }
        }
        else
        {
            // Debug.Log("No $PrudenceFavor variable in storage yet.");
        }

        /*
         * Tracking charcter appearence.
         */
        CheckPV("$OliverPresent", ref prevOliverPresent, "Oliver", oliverGameObject);
        CheckPV("$LizzyPresent", ref prevLizzyPresent, "Lizzy", lizzyGameObject);
        CheckPV("$FatherPresent", ref prevFatherPresent, "Father", fatherGameObject);
        CheckPV("$PrudencePresent", ref prevPrudencePresent, "Prudence", prudenceGameObject);
        CheckPV("$JohnPresent", ref prevJohnPresent, "John", johnGameObject);
        CheckPV("$DanielPresent", ref prevDanielPresent, "Daniel", danielGameObject);
        CheckPV("$ConstablePresent", ref prevConstablePresent, "Constable", constableGameObject);
        CheckPV("$JosephPresent", ref prevJosephPresent, "Joseph", josephGameObject);
        CheckPV("$EstherPresent", ref prevEstherPresent, "Esther", estherGameObject);
        CheckPV("$LauraPresent", ref prevLauraPresent, "Laura", lauraGameObject);
        CheckPV("$RuthPresent", ref prevRuthPresent, "Ruth", ruthGameObject);

        CheckReady("$letter");
        CheckReady("$herbs");
        CheckReady("$doll");

        TitleEffects("$title1", ref prevTitle1Present, title1Object);
        TitleEffects("$title2", ref prevTitle2Present, title2Object);
        TitleEffects("$title3", ref prevTitle3Present, title3Object);
        TitleEffects("$endGame", ref prevEndingTitlePresent, EndObject);

        CheckEnding();

    }

    void CheckSound()
    {
        if (variableStorage.TryGetValue(variableName: "$gasp", out bool isGasping))
        {
            //check if the value changed from false to true
            if (isGasping && !hasGasped)
            {
                Debug.Log($"sound playing");
                audioSource.Play();
                hasGasped = true;
            }
            else if (!isGasping)
            {
                hasGasped = false;
            }
        }

        if (variableStorage.TryGetValue(variableName: "$whisper", out bool isWhispering))
        {
            //check if the value changed from false to true
            if (isWhispering && !hasWhispers)
            {
                Debug.Log($"sound playing");
                audioSource.Play();
                hasWhispers = true;
            }
            else if (!isWhispering)
            {
                hasWhispers = false;
            }
        }

    }

    void CheckPV(string variableName, ref bool previousValue, string characterName, GameObject targetObject)
    {
        if (variableStorage == null)
            return;

        if (variableStorage.TryGetValue(variableName, out bool currentValue))
        {
            if (currentValue && !previousValue)
            {
                Debug.Log($"{characterName} is present");

                if (targetObject != null && !targetObject.activeSelf)
                    targetObject.SetActive(true);

                PlayAnim(targetObject, characterName);
                previousValue = true;

                if (targetObject != null && !bobbingObjects.Contains(targetObject))
                {
                    bobbingObjects.Add(targetObject);
                    originalPositions[targetObject] = targetObject.transform.position;
                }
            }
            else if (!currentValue && previousValue)
            {
                Debug.Log($"{characterName} is leaving");

                if (targetObject != null && bobbingObjects.Contains(targetObject))
                {
                    bobbingObjects.Remove(targetObject);
                    originalPositions.Remove(targetObject);
                }

                PlayReverse(targetObject, characterName);
                previousValue = false;
            }
        }
    }

    void TitleEffects(string variableName, ref bool previousStage, GameObject targetObject)
    {
        if (variableStorage.TryGetValue(variableName, out bool currentStage))
        {

            if (currentStage && !previousStage)
            {
                targetObject.SetActive(true);
                physicalEvidence.SetActive(false);
                previousStage = true;

            }
            else if (!currentStage && previousStage)
            {

                targetObject.SetActive(false);
                physicalEvidence.SetActive(true);
                previousStage = false;

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

    void PlayEnding(GameObject targetObject)
    {
        if (targetObject != null)
        {
            Animator animator = targetObject.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("EndingAnimation"); // Replace with your specific animation trigger name
            }
            else
            {
                Debug.LogWarning("No Animator component found for ending!");
            }
        }
        else
        {
            Debug.LogWarning("No target object for ending");
        }
    }


    void CheckEnding()
    {
        if (variableStorage.TryGetValue("$ending", out bool currentValue))
        {
            // Check if the value changed from false to true
            if (currentValue && !prevEnding)
            {
                Debug.Log("Ending is true");
                PlayEnding(endingGameObject);
                prevEnding = true; // Update immediately to prevent repeated calls
            }
            // Check if the value changed from true to false (optional: remove if not needed)
            else if (!currentValue && prevEnding)
            {
                Debug.Log("Ending is false");
                // If you have a reverse animation, call it here
                prevEnding = false;
            }
        }
    }

    public float bobHeight = 0.1f;


    private void BobUpAndDown()
    {
        foreach (var obj in bobbingObjects)
        {
            if (obj != null && originalPositions.TryGetValue(obj, out Vector3 originalPos))
            {
                // Calculate bob offset using sine wave
                float bobOffset = Mathf.Sin(Time.time * bobSpeed) * bobAmplitude;
                obj.transform.position = originalPos + new Vector3(0f, bobOffset, 0f);  // Apply vertical bob
            }
        }
    }

    public bool letterVisible = false;
    public bool herbsVisible = false;
    public bool dollVisible = false;
    [Header("Evidence Objects. First letter, then herbs, then doll.")]
    [SerializeField] private GameObject[] evidenceObjects;  // Add any other evidence objects [e.g., letter, herbs, etc.]


    void CheckReady(string evidenceName)
    {
        if (variableStorage.TryGetValue(evidenceName, out bool currentValue))
        {
            // Check if the value changed from false to true
            if (currentValue && !letterVisible)
            {
                Debug.Log($"{evidenceName} is present");
                if (evidenceName == "$letter" || evidenceName == "$herbs")
                {
                    letterVisible = true;
                    evidenceObjects[0].SetActive(true);
                    evidenceObjects[1].SetActive(true);
                    evidenceObjects[2].SetActive(true);
                }
            }

        }
    }


}

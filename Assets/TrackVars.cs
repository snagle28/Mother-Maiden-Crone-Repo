using UnityEngine;
using Yarn.Unity;
//https://docs.yarnspinner.dev/api/csharp/yarn/yarn.memoryvariablestore/yarn.memoryvariablestore.trygetvalue

public class TrackVars : MonoBehaviour
{
    public InMemoryVariableStorage variableStorage;

    int previousHysteria = int.MinValue;
    int prevPrudenceFavor = int.MinValue;
    int prevEstherFavor = int.MinValue;
    int prevRuthFavor = int.MinValue;

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
    }
}
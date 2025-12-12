using UnityEngine;
using UnityEngine.TextCore.Text;

public class SpeakerEffects : MonoBehaviour
{
    [Header("Optional animation")]
    public Animator animator;
    public string talkingBoolName = "IsTalking"; // or use a trigger if you prefer

    [Header("Optional audio")]
    public AudioSource audioSource;
    public AudioClip talkingLoop; // e.g. little blip/loop; can be null

    public GameObject emissivGO;
    
    public void StartTalking()
    {
        print(this.gameObject.name + " is talking!");
        if (animator && !string.IsNullOrEmpty(talkingBoolName))
        {
            animator.SetBool(talkingBoolName, true);
            emissivGO.SetActive(true);
        }

        if (audioSource && talkingLoop)
        {
            if (audioSource.clip != talkingLoop)
                audioSource.clip = talkingLoop;
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
    }
    
    public void StopTalking()
    {
        emissivGO.SetActive(false);
        if (animator && !string.IsNullOrEmpty(talkingBoolName))
            animator.SetBool(talkingBoolName, false);
    
        if (audioSource && audioSource.isPlaying)
            audioSource.Stop();
    }
    

}
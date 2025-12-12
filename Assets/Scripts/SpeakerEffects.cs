using UnityEngine;

public class SpeakerEffects : MonoBehaviour
{
    [Header("Optional animation")]
    public Animator animator;
    public string talkingBoolName = "IsTalking"; // or use a trigger if you prefer

    [Header("Optional audio")]
    public AudioSource audioSource;
    public AudioClip talkingLoop; // e.g. little blip/loop; can be null

    public void StartTalking()
    {
        if (animator && !string.IsNullOrEmpty(talkingBoolName))
            animator.SetBool(talkingBoolName, true);
    
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
        if (animator && !string.IsNullOrEmpty(talkingBoolName))
            animator.SetBool(talkingBoolName, false);
    
        if (audioSource && audioSource.isPlaying)
            audioSource.Stop();
    }
    // public void StartTalking()
    // {
    //     Debug.Log($"START talking: {gameObject.name}");
    // }
    //
    // public void StopTalking()
    // {
    //     Debug.Log($"STOP talking: {gameObject.name}");
    // }

}
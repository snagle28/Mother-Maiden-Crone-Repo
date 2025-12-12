using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Yarn.Unity;

public class SpeakerEffectsPresenter : DialoguePresenterBase
{
    [System.Serializable]
    public class SpeakerEntry
    {
        public string characterName;
        public SpeakerEffects effects;
    }

    [Header("Map Yarn character names to SpeakerEffects")]
    public List<SpeakerEntry> speakers = new List<SpeakerEntry>();

    private Dictionary<string, SpeakerEffects> _speakerLookup =
        new Dictionary<string, SpeakerEffects>();

    private SpeakerEffects _currentSpeaker;

    [Header("UI Reference")]
    public TMPro.TMP_Text lineText;   // << DRAG YOUR Line Presenter → Text (TMP) HERE

    void Awake()
    {
        _speakerLookup.Clear();

        foreach (var entry in speakers)
        {
            if (entry != null &&
                !string.IsNullOrWhiteSpace(entry.characterName) &&
                entry.effects != null)
            {
                _speakerLookup[entry.characterName.Trim()] = entry.effects;
            }
        }
    }

    // REQUIRED BY 3.0.3 API
    public override YarnTask OnDialogueStartedAsync()
    {
        return YarnTask.CompletedTask;
    }

    // REQUIRED BY 3.0.3 API
    public override YarnTask<DialogueOption?> RunOptionsAsync(
        DialogueOption[] options,
        CancellationToken token)
    {
        return YarnTask.FromResult<DialogueOption?>(null);
    }

    // THIS IS WHERE WE ADD ITALICS FOR NARRATION
    public override YarnTask RunLineAsync(
        LocalizedLine line,
        LineCancellationToken token)
    {
        // stop previous speaker effects
        if (_currentSpeaker != null)
        {
            _currentSpeaker.StopTalking();
            _currentSpeaker = null;
        }

        // determine if this is narration (no speaker)
        bool isNarration = string.IsNullOrEmpty(line.CharacterName);

        // Apply text formatting
        if (lineText != null)
        {
            if (isNarration)
                lineText.fontStyle = TMPro.FontStyles.Italic;
            else
                lineText.fontStyle = TMPro.FontStyles.Normal;
        }

        // Handle speaker effects
        if (!isNarration)
        {
            string name = line.CharacterName.Trim();

            if (_speakerLookup.TryGetValue(name, out var speaker))
            {
                _currentSpeaker = speaker;
                speaker.StartTalking();
            }
        }

        return YarnTask.CompletedTask;
    }

    public override YarnTask OnDialogueCompleteAsync()
    {
        foreach (var kv in _speakerLookup)
            kv.Value.StopTalking();

        _currentSpeaker = null;
        return YarnTask.CompletedTask;
    }
}

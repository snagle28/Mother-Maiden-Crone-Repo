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

    [Header("Yarn Reference")]
    [SerializeField] private DialogueRunner dialogueRunner;

    [Header("MC Speaker Remap")]
    [SerializeField] private string mcVariableName = "$MC";
    [SerializeField] private string youCharacterName = "You";

    void Awake()
    {
        if (dialogueRunner == null)
            dialogueRunner = FindObjectOfType<DialogueRunner>();

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
        // If options are "You: ...", Yarn sends them here (not via RunLineAsync),
        // so we trigger the MC speaker effects while choices are on screen.
        if (options != null)
        {
            bool hasYouOption = false;

            for (int i = 0; i < options.Length; i++)
            {
                var optLine = options[i].Line;
                if (optLine == null) continue;

                var charName = optLine.CharacterName;
                if (!string.IsNullOrWhiteSpace(charName) &&
                    string.Equals(charName.Trim(), youCharacterName, System.StringComparison.OrdinalIgnoreCase))
                {
                    hasYouOption = true;
                    break;
                }
            }

            if (hasYouOption)
            {
                // stop previous speaker effects
                if (_currentSpeaker != null)
                {
                    _currentSpeaker.StopTalking();
                    _currentSpeaker = null;
                }

                string resolvedName = youCharacterName;

                if (dialogueRunner != null && dialogueRunner.VariableStorage != null &&
                    dialogueRunner.VariableStorage.TryGetValue(mcVariableName, out string mcName) &&
                    !string.IsNullOrWhiteSpace(mcName))
                {
                    resolvedName = mcName.Trim();
                }

                if (_speakerLookup.TryGetValue(resolvedName, out var speaker))
                {
                    _currentSpeaker = speaker;
                    speaker.StartTalking();
                }
                else
                {
                    Debug.LogWarning($"SpeakerEffectsPresenter: No SpeakerEffects mapped for speaker name '{resolvedName}' (from options).");
                }
            }
        }

        // We still don't render options UI here; another view can do that.
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

            // If Yarn says the speaker is "You", remap to whatever $MC currently is
            if (string.Equals(name, youCharacterName, System.StringComparison.OrdinalIgnoreCase))
            {
                if (dialogueRunner != null && dialogueRunner.VariableStorage != null)
                {
                    if (dialogueRunner.VariableStorage.TryGetValue(mcVariableName, out string mcName) &&
                        !string.IsNullOrWhiteSpace(mcName))
                    {
                        name = mcName.Trim();
                    }
                    else
                    {
                        Debug.LogWarning($"SpeakerEffectsPresenter: '{youCharacterName}' line, but {mcVariableName} is missing/empty.");
                    }
                }
                else
                {
                    Debug.LogWarning($"SpeakerEffectsPresenter: '{youCharacterName}' line, but DialogueRunner/VariableStorage not set.");
                }
            }

            if (_speakerLookup.TryGetValue(name, out var speaker))
            {
                _currentSpeaker = speaker;
                speaker.StartTalking();
            }
            else
            {
                Debug.LogWarning($"SpeakerEffectsPresenter: No SpeakerEffects mapped for speaker name '{name}'.");
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
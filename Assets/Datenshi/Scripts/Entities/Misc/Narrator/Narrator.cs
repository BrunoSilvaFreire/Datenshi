using System.Collections;
using System.Collections.Generic;
using Datenshi.Scripts.Input;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Datenshi.Scripts.Util.Misc.Narrator {
    /// <summary>
    /// Type text component types out Text one character at a time. Heavily adapted from synchrok's GitHub project.
    /// </summary>
    [RequireComponent(typeof(Text))]
    public sealed class Narrator : MonoBehaviour {
        /// <summary>
        /// The print delay setting. Could make this an option some day, for fast readers.
        /// </summary>
        private const float PrintDelaySetting = 0.02f;

        // Characters that are considered punctuation in this language. TextTyper pauses on these characters
        // a bit longer by default. Could be a setting sometime since this doesn't localize.
        private readonly List<char> punctutationCharacters = new List<char> {
            '.',
            ',',
            '!',
            '?'
        };

        [SerializeField]
        [Tooltip("Event that's called when the text has finished printing.")]
        private UnityEvent printCompleted = new UnityEvent();

        [SerializeField]
        [Tooltip("Event called when a character is printed. Inteded for audio callbacks.")]
        private CharacterPrintedEvent characterPrinted = new CharacterPrintedEvent();

        public AudioClip DefaultSpeechClip;
        public AudioSource AudioSource;

        [ShowInInspector]
        public Text textComponent;

        public float DefaultPrintDelay;
        public bool WaitForInput;
        private string printingText;
        private Coroutine typeTextCoroutine;

        /// <summary>
        /// Gets the PrintCompleted callback event.
        /// </summary>
        /// <value>The print completed callback event.</value>
        public UnityEvent PrintCompleted => printCompleted;

        /// <summary>
        /// Gets the CharacterPrinted event, which includes a string for the character that was printed.
        /// </summary>
        /// <value>The character printed event.</value>
        public CharacterPrintedEvent CharacterPrinted => characterPrinted;

        /// <summary>
        /// Gets a value indicating whether this <see cref="Narrator"/> is currently printing text.
        /// </summary>
        /// <value><c>true</c> if printing; otherwise, <c>false</c>.</value>
        public bool IsTyping => typeTextCoroutine != null;

        private Text TextComponent {
            get {
                if (textComponent == null) {
                    textComponent = GetComponent<Text>();
                }

                return textComponent;
            }
        }

        /// <summary>
        /// Types the text into the Text component character by character, using the specified (optional) print delay per character.
        /// </summary>
        /// <param name="text">Text to type.</param>
        /// <param name="printDelay">Print delay (in seconds) per character.</param>
        public void TypeText(string text, float printDelay = -1) {
            Cleanup();

            DefaultPrintDelay = printDelay > 0 ? printDelay : PrintDelaySetting;
            typeTextCoroutine = StartCoroutine(TypeTextCharByChar(text, null));
        }

        /// <summary>
        /// Skips the typing to the end.
        /// </summary>
        public void Skip() {
            Cleanup();

            var generator = new TypedTextGenerator();
            var typedText = generator.GetCompletedText(printingText);
            TextComponent.text = typedText.TextToPrint;

            OnTypewritingComplete();
        }

        /// <summary>
        /// Determines whether this instance is skippable.
        /// </summary>
        /// <returns><c>true</c> if this instance is skippable; otherwise, <c>false</c>.</returns>
        public bool IsSkippable() {
            // For now there's no way to configure this. Just make sure it's currently typing.
            return IsTyping;
        }

        private void Cleanup() {
            if (typeTextCoroutine != null) {
                StopCoroutine(typeTextCoroutine);
                typeTextCoroutine = null;
            }
        }


        public IEnumerator TypeTextCharByChar(string text, AudioClip clip = null) {
            printingText = text;
            TextComponent.text = string.Empty;

            var generator = new TypedTextGenerator();
            TypedTextGenerator.TypedText typedText;
            var printedCharCount = 0;
            var cancel = false;
            do {
                typedText = generator.GetTypedTextAt(text, printedCharCount);
                TextComponent.text = typedText.TextToPrint;
                OnCharacterPrinted(typedText.LastPrintedChar.ToString());

                ++printedCharCount;

                var delay = typedText.Delay > 0
                    ? typedText.Delay
                    : GetPrintDelayForCharacter(typedText.LastPrintedChar);
                var currentTime = delay;
                while (currentTime > 0) {
                    if (InputUtil.GetAnyPlayerButtonDown((int) Actions.Attack)) {
                        TextComponent.text = generator.GetCompletedText(text).TextToPrint;
                        yield return null;
                        cancel = true;
                    }

                    currentTime -= Time.deltaTime;
                    yield return null;
                }

                if (clip != null) {
                    AudioSource.PlayOneShot(clip);
                }

                if (cancel) {
                    break;
                }
            } while (!typedText.IsComplete);

            if (WaitForInput) {
                while (!InputUtil.GetAnyPlayerButtonDown((int) Actions.Attack)) {
                    yield return null;
                }
            }

            typeTextCoroutine = null;
            OnTypewritingComplete();
            yield return null;
        }

        private float GetPrintDelayForCharacter(char characterToPrint) {
            // Then get the default print delay for the current character
            var punctuationDelay = DefaultPrintDelay * 8.0f;
            return punctutationCharacters.Contains(characterToPrint) ? punctuationDelay : DefaultPrintDelay;
        }

        private void OnCharacterPrinted(string printedCharacter) {
            CharacterPrinted?.Invoke(printedCharacter);
        }

        private void OnTypewritingComplete() {
            PrintCompleted?.Invoke();
        }

        /// <summary>
        /// Event that signals a Character has been printed to the Text component.
        /// </summary>
        [System.Serializable]
        public class CharacterPrintedEvent : UnityEvent<string> { }
    }
}
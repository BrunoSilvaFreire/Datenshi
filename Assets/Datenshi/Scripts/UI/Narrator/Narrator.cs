using System.Collections;
using System.Collections.Generic;
using Datenshi.Input.Constants;
using Datenshi.Scripts.Util;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Datenshi.Scripts.UI.Narrator {
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

        [ShowInInspector]
        private Text textComponent;

        public float DefaultPrintDelay;
        public bool WaitForInput;
        private string printingText;
        private Coroutine typeTextCoroutine;

        /// <summary>
        /// Gets the PrintCompleted callback event.
        /// </summary>
        /// <value>The print completed callback event.</value>
        public UnityEvent PrintCompleted {
            get {
                return printCompleted;
            }
        }

        /// <summary>
        /// Gets the CharacterPrinted event, which includes a string for the character that was printed.
        /// </summary>
        /// <value>The character printed event.</value>
        public CharacterPrintedEvent CharacterPrinted {
            get {
                return characterPrinted;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Narrator"/> is currently printing text.
        /// </summary>
        /// <value><c>true</c> if printing; otherwise, <c>false</c>.</value>
        public bool IsTyping {
            get {
                return typeTextCoroutine != null;
            }
        }

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
            printingText = text;

            typeTextCoroutine = StartCoroutine(TypeTextCharByChar(text));
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

        public IEnumerator TypeTextCharByChar(string text) {
            TextComponent.text = string.Empty;

            var generator = new TypedTextGenerator();
            TypedTextGenerator.TypedText typedText;
            var printedCharCount = 0;
            do {
                typedText = generator.GetTypedTextAt(text, printedCharCount);
                TextComponent.text = typedText.TextToPrint;
                OnCharacterPrinted(typedText.LastPrintedChar.ToString());

                ++printedCharCount;

                var delay = typedText.Delay > 0
                    ? typedText.Delay
                    : GetPrintDelayForCharacter(typedText.LastPrintedChar);
                yield return new WaitForSeconds(delay);
                if (InputUtil.GetAnyPlayerButtonDown(Actions.Attack)) {
                    Skip();
                }
            } while (!typedText.IsComplete);
            if (WaitForInput) {
                while (!InputUtil.GetAnyPlayerButtonDown(Actions.Attack)) {
                    yield return null;
                }
            }

            typeTextCoroutine = null;
            OnTypewritingComplete();
        }

        private float GetPrintDelayForCharacter(char characterToPrint) {
            // Then get the default print delay for the current character
            var punctuationDelay = DefaultPrintDelay * 8.0f;
            return punctutationCharacters.Contains(characterToPrint) ? punctuationDelay : DefaultPrintDelay;
        }

        private void OnCharacterPrinted(string printedCharacter) {
            if (CharacterPrinted != null) {
                CharacterPrinted.Invoke(printedCharacter);
            }
        }

        private void OnTypewritingComplete() {
            if (PrintCompleted != null) {
                PrintCompleted.Invoke();
            }
        }

        /// <summary>
        /// Event that signals a Character has been printed to the Text component.
        /// </summary>
        [System.Serializable]
        public class CharacterPrintedEvent : UnityEvent<string> { }
    }
}
using System.Collections;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;

public class DialogManager : Singleton<DialogManager>
{
    [SerializeField] private GameObject dialogParent;
    [SerializeField] private TextMeshProUGUI textBox;
    [SerializeField] private Image speakerSprite;
    [SerializeField] private TextMeshProUGUI speakerName;

    [Header("Character Data")]
    [SerializeField] private List<CharacterProfile> characterData;
    private Dictionary<string, CharacterProfile> characters = new();
    private bool next;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        dialogParent.SetActive(false);

        foreach (var character in characterData)
            characters.Add(character.name, character);
    }

    public void StartDialogSequence(DialogConversation conversation, UnityAction callback = null)
    {
        StartCoroutine(RunDialogSequence(conversation, callback));
    }


    private IEnumerator RunDialogSequence(DialogConversation conversation, UnityAction callback)
    {
        dialogParent.SetActive(true);

        foreach (var line in conversation.conversation)
        {
            CharacterProfile character = characters[line.Character];

            var sprite = character.GetSprite(line.Emotion);
            speakerSprite.sprite = sprite;
            if (sprite == null)
            {
                speakerSprite.color = Color.clear;
            }
            else
            {
                speakerSprite.color = Color.white;
                speakerSprite.SetNativeSize();
            }

            speakerName.transform.parent.gameObject.SetActive(!string.IsNullOrEmpty(character.Name));
            speakerName.text = character.Name;
            Canvas.ForceUpdateCanvases();

            string text = line.Text;
            yield return PrintTextBox(text, string.IsNullOrEmpty(character.Voice) ? "text_blip" : character.Voice);

            yield return new WaitUntil(() => next);
            next = false;
        }
        

        dialogParent.SetActive(false);

        callback?.Invoke();
    }

    private IEnumerator PrintTextBox(string text, string voice)
    {
        for (int i = 0; i < text.Length; i++)
        {
            if (next)
            {
                AudioManager.Instance.PlaySound(voice);
                next = false;
                break;
            }

            if (i % 4 == 0)
                AudioManager.Instance.PlaySound(voice);

            textBox.text = text.Substring(0, i);

            yield return new WaitForNextFrameUnit();

            
        }

        textBox.text = text;
    }

    public void OnNext()
    {
        next = true;
    }

}


public struct DialogConversation
{
    public DialogLine[] conversation;
}

public struct DialogLine
{
    public string Character;
    public string Emotion;
    public string Text;
}
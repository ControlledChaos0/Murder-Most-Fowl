using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Yarn.Unity;

public class DialogueHelper : Singleton<DialogueHelper>
{
    [Serializable]
    public struct SpriteItem
    {
        public string name;
        public Sprite sprite;
    }
    [Serializable]
    public struct Song
    {
        public string name;
        public AudioClip song;
    }

    [Serializable]
    public class SpriteItemList
    {
        public List<SpriteItem> spriteItemList = new();
    }
    [Serializable]
    public struct NameSpriteMatch
    {
        public string charID;
        public string name;
        public SpriteItemList expressions;
    }

    [Header("Yarn Components")] 
    [SerializeField] private DialogueRunner _dialogueRunner;

    [Header("Dialogue UI")]
    [SerializeField] private GameObject _background;
    [SerializeField] private Image _leftCharacter;
    [SerializeField] private Image _rightCharacter;
    [SerializeField] private List<SpriteItem> _spriteList;
    [SerializeField] private List<NameSpriteMatch> _nameList;
    [SerializeField] private AudioSource _source;
    [SerializeField] private List<Song> _trackList;


    public bool InDialogue
    {
        get => _inDialogue;
    }
    public DialogueRunner DialogueRunner
    {
        get => _dialogueRunner;
    }

    private static Image _left;
    private static Image _right;
    private static string _leftName;
    private static string _rightName;
    private static List<SpriteItem> _sprites;
    private static List<NameSpriteMatch> _names;
    private static bool lockPortrait = false;

    private static AudioSource _audioSource;
    private static List<Song> _tracks;

    private Dictionary<string, string> _nameDict = new();
    private bool _inDialogue;

    void Awake()
    {
        InitializeSingleton();

        _left = _leftCharacter;
        _right = _rightCharacter;
        _sprites = _spriteList;
        _names = _nameList;
        _tracks = _trackList;
        _audioSource = _source;

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _inDialogue = false;
        _dialogueRunner.onNodeStart.AddListener(StartNode);
        //TMPro_EventManager.TEXT_CHANGED_EVENT.Add(NamePortraitUpdater);

        foreach (NameSpriteMatch match in _nameList)
        {
            _nameDict[match.name] = match.charID;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void StartDialogue()
    {
        _inDialogue = true;
        AddContinueInput();
        ClueBoardManager.Instance.LockToggle();
        _background.SetActive(true);
    }

    public void EndDialogue()
    {
        _inDialogue = false;
        RemoveContinueInput();
        ClueBoardManager.Instance.UnlockToggle();
        _background.SetActive(false);
    }

    public void AddContinueInput()
    {

    }

    public void RemoveContinueInput()
    {

    }

    public void StartNode(string node)
    {
        if (node == null)
        {
            return;
        }

        lockPortrait = false;

        var tagIEnum = _dialogueRunner.Dialogue.GetTagsForNode(node);

        List<string> tags = new(tagIEnum);
        foreach (string tag in tags)
        {
            MetadataParser(tag);
        }
    }

    // TODO
    // Generalize these commands

    [YarnCommand("ChangeLeftCharacter")]
    public static void ChangeLeft(string name = null)
    {
        if (lockPortrait)
        {
            return;
        }
        if (_leftName == name)
        {
            return;
        }

        _leftName = name;
        if (string.IsNullOrEmpty(name))
        {
            _left.gameObject.SetActive(false);
            return;
        }

        _left.gameObject.SetActive(true);
        SpriteItem spriteItem = _sprites.Find(e => e.name == name);

        // TODO
        // Add Error Handling
        _left.sprite = spriteItem.sprite;
    }

    [YarnCommand("ChangeRightCharacter")]
    public static void ChangeRight(string name = null)
    {
        if (lockPortrait)
        {
            return;
        }
        if (_rightName == name)
        {
            return;
        }

        _rightName = name;
        if (string.IsNullOrEmpty(name))
        {
            _right.gameObject.SetActive(false);
            return;
        }

        _right.gameObject.SetActive(true);
        SpriteItem spriteItem = _sprites.Find(e => e.name == name);

        // TODO
        // Add Error Handling
        _right.sprite = spriteItem.sprite;
    }

    public static void ChangeRightExpression(string name = null)
    {
        if (lockPortrait)
        {
            return;
        }

        if (string.IsNullOrEmpty(name))
        {
            _right.gameObject.SetActive(false);
            return;
        }

        NameSpriteMatch charInfo = _names.Find(e => e.charID == _rightName);

        SpriteItemList expressions = charInfo.expressions;
        Debug.Log(charInfo.name);
        Debug.Log(charInfo.charID);
        
        if (expressions != null)
        {
            _right.gameObject.SetActive(true);
            SpriteItem spriteItem = expressions.spriteItemList.Find(e => e.name == name);
            _right.sprite = spriteItem.sprite;
        }
    }

    [YarnCommand("ChangeCharacters")]
    public static void ChangeCharacters(string left_name = null, string right_name = null)
    {
        ChangeLeft(left_name);
        ChangeRight(right_name);
    }

    public void ChangeTrack(string trackName = null)
    {
        AudioClip newClip = _tracks.Find(e => e.name == trackName).song;
        if (newClip != _audioSource.clip)
        {
            _audioSource.clip = newClip;
            _audioSource.volume = 1;
            _audioSource.Play();
        }
    }

    public void UpdateDialogueUI(LocalizedLine localLine)
    {
        string[] nameParts = localLine.RawText.Split(':');
        if (!string.IsNullOrEmpty(nameParts[0]))
        {
            NamePortraitUpdater(nameParts[0]);
        }

        if (localLine.Metadata != null)
        {
            List<string> meta = new(localLine.Metadata);
            foreach (string s in meta)
            {
                MetadataParser(s);
            }
        }
    }

    private void MetadataParser(string tag)
    {
        string[] tagParts = tag.Split(':');
        if (tagParts[0] == "lockP")
        {
            lockPortrait = true;
        }
        if (tagParts[0] == "c")
        {
            ChangeCharacters(tagParts[1], tagParts[2]);
            return;
        }
        if (tagParts[0] == "lc")
        {
            ChangeLeft(tagParts[1]);
            return;
        }
        if (tagParts[0] == "rc")
        {
            ChangeRight(tagParts[1]);
            return;
        }
        if (tagParts[0] == "e")
        {
            ChangeRightExpression(tagParts[1]);
            return;
        }
        if (tagParts[0] == "s")
        {
            ChangeTrack(tagParts[1]);
            return;
        }
        
    }

    private void NamePortraitUpdater(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return;
        }
        _nameDict.TryGetValue(name, out string spriteName);
        if (string.IsNullOrEmpty(spriteName))
        {
            Debug.LogError("Name is not in the dictionary. Check spelling.");
            return;
        }

        if (spriteName != _leftName && spriteName != _rightName)
        {
            if (spriteName == "Owl")
            {
                ChangeLeft(spriteName);
            }
            else
            {
                ChangeRight(spriteName);
            }
        }
    }
}

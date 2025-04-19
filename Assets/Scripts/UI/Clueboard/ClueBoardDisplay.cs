using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClueBoardDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Image clueImage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ClearDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClearDisplay()
    {
        ChangeName();
        ChangeDescription();
        ChangeImage();
    }

    public void SetDisplay(string clueID = null)
    {
        if (clueID == null)
        {
            ClearDisplay();
            return;
        }

        Clue clue = GameManager.ClueManager.ClueDatabase.Clues[clueID];
        ChangeName(clue.Name);
        ChangeDescription(clue.Description);
        ChangeImage(clue.Icon);
    }

    private void ChangeName(string name = "")
    {
        nameText.text = name;
    }

    private void ChangeDescription(string desc = "")
    {
        descriptionText.text = desc;
    }

    private void ChangeImage(Sprite sprite = null)
    {
        if (sprite == null)
        {
            clueImage.enabled = false;
            return;
        }

        clueImage.enabled = true;
        clueImage.sprite = sprite;
    }
}

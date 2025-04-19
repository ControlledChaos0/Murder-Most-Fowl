using UnityEngine;
using UnityEngine.UI;

public class ClueboardButton : MonoBehaviour
{
    [SerializeField]
    private Sprite _toOverworld;
    [SerializeField] 
    private Sprite _toClueboard;

    [SerializeField] 
    private Image _buttonImage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CloseClueBoard();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenClueBoard()
    {
        _buttonImage.sprite = _toOverworld;
    }

    public void CloseClueBoard()
    {
        _buttonImage.sprite = _toClueboard;
    }
}

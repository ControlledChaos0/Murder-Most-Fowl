using UnityEngine;

[SelectionBase]
public class PlayerBody : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _characterSprite;

    private PlayerFacing _facing;

    public SpriteRenderer Sprite
    {
        get { return _characterSprite; }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public enum PlayerFacing
{
    Right = 0,
    Left = 1,
}

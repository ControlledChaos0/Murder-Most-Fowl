using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [SerializeField]
    private CharacterDatabase m_characterDatabase;
    public CharacterDatabase CharacterDatabase => m_characterDatabase;

    public State InitCharacterState(State activeState)
    {
        foreach (Character c in CharacterDatabase.CharacterList)
        {
            CharacterState cState = new();
            cState.Name = c.Name;
            cState.CurrentNode = c.StartingNode;
            cState.CurrentDismissal = c.StartingDismissal;

            switch (c.CharacterID)
            {
                case "Goose":
                    activeState.GooseState = cState;
                    break;
                case "Peacock":
                    activeState.PeacockState = cState;
                    break;
                case "Crow":
                    activeState.CrowState = cState;
                    break;
                case "Penguin":
                    activeState.PenguinState = cState;
                    break;
            }
        }

        return activeState;
    }
}
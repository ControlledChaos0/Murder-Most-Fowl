using UnityEngine;

public abstract class PlayerInteractableCommand : Command
{
    protected PlayerInteractable m_Interactable;

    public PlayerInteractableCommand(PlayerInteractable interactable)
    {
        m_Interactable = interactable;
    }

    public override void Execute()
    {
        if (m_Interactable.WithinRange(PlayerController.Instance.PlayerPos)) {
            base.Execute();
        } else
        {
            PlayerController.Instance.Interactable = null;
        }
    }

    public override void Stop()
    {
        if (!IsCompleted)
        {
            base.Stop();
        }
        PlayerController.Instance.Interactable = null;
    }
    protected override void ReadyCommand()
    {
        //PlayerController.Instance.Interactable = m_Interactable;
    }

    protected override void StopCommand()
    {
        return;
    }
}

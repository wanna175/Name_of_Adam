using UnityEngine;

public abstract class Phase
{
    protected PhaseController _controller;

    public void SetController(PhaseController controller)
    {
        this._controller = controller;
    }

    public abstract void OnStateEnter();
    // 시작할 때 한 번 실행

    public abstract void OnStateUpdate();
    // 페이즈 내내 실행

    public abstract void OnStateExit();
    // 끝날 때 한 번 실행 

    public virtual void OnClickEvent(Vector2 coord)
    {
        return;
    }

    protected virtual bool IsExit()
    {
        return true;
    }
    // 끝나는 조건
}
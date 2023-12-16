using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class UnitFall : MonoBehaviour
{
    private int _maxCount;
    private int _currentCount;
    private bool _isEdified = false;
    public bool IsEdified => _isEdified;

    [Header("타락 이벤트")]
    public UnityEvent UnitFallEvent;

    public void Init(int CurrentCount, int maxCount)
    {
        _currentCount = CurrentCount;
        _maxCount = maxCount;
    }

    public void ChangeFall(int value)
    {
        if (_isEdified) return;

        _currentCount += value;

        if (_currentCount <= 0)
        {
            _currentCount = 0;
        }
        else if (_currentCount >= _maxCount)
        {
            UnitFallEvent.Invoke();
        }

        // Add Fall Change Event (EX. UI)
        Debug.Log("FALL : " + value + ", CurFALL ; " + _currentCount);
    }

    public void Editfy()
    {
        _isEdified = true;
    }

    public int GetCurrentFallCount()
    {
        return _currentCount;
    }
}
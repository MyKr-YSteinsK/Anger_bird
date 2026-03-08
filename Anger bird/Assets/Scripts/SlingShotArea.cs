using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class SlingShotArea : MonoBehaviour
{
    [SerializeField] private LayerMask _slingshotAreaMask;
    //判断鼠标是否在弹弓的一定作用范围内，如果在则上鸟拉弓
    public bool IswithinSlingshotArea()
    {
        //获取鼠标位置
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(InputManager.MousePosition);
        //判断鼠标位置是否在范围内
        if(Physics2D.OverlapPoint(worldPosition, _slingshotAreaMask))
            return true;
        return false;
    }
}

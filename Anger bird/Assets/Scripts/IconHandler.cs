using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconHandler : MonoBehaviour
{
    //小鸟图标列表
    [SerializeField] private Image[] _icons;
    //颜色
    [SerializeField] private Color _usedColor;
    //将已发射的小鸟图标设置为灰色
    public void UseShot(int shotNumber)
    {
        for(int i = 0; i < _icons.Length; i++)
            if(shotNumber == i + 1)
            {
                _icons[i].color = _usedColor;
                return;
            }
    }
}

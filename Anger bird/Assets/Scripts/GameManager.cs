using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    //可发射小鸟数
    public int MaxNumberOfShots = 3;
    //发射后等待时间
    [SerializeField] private float _secondsToWaitBeforeDeathCheck = 3f; 
    //重试
    [SerializeField] private GameObject _restartScreenObject;
    //下一关
    [SerializeField] private Image _nextLevelImage;
    //弹弓有关脚本
    [SerializeField] private SlingShotHandler _slingShotHandle;
    //已使用的小鸟数
    private int _useNumberOfShots;
    //左下角小鸟数量图标
    private IconHandler _iconHandler;
    //小猪列表
    private List<Baddie> _baddies = new List<Baddie>();
    private void Awake() 
    {
        if(instance == null)
            instance = this;
        //默认三只
        _iconHandler = FindObjectOfType<IconHandler>();
        //添加所有小猪到列表以方便检查，移除，计算小猪数量
        Baddie[] baddies = FindObjectsOfType<Baddie>();  
        foreach(Baddie b in baddies)
            _baddies.Add(b);
            
        _nextLevelImage.enabled = false;
    }
    //更新图标显示剩余小鸟数/已使用小鸟数
    public void UseShot()
    {
        _useNumberOfShots++;
        _iconHandler.UseShot(_useNumberOfShots);
        CheckForLastShot();
    }
    //判断是否还有剩余小鸟
    public bool HasEnoughShots()
    {
        if(_useNumberOfShots < MaxNumberOfShots)
            return true;
        return false;
    }
    //检查发射完所有小鸟是否有小猪剩余
    public void CheckForLastShot () 
    {
        if(_useNumberOfShots == MaxNumberOfShots)
            StartCoroutine(CheckAfterWaitTime());
    }
    //等待若干秒for状态稳定
    private IEnumerator CheckAfterWaitTime()
    {
        yield return new WaitForSeconds(_secondsToWaitBeforeDeathCheck);
        //剩余小猪等于0则胜利,否则显示重试按钮
        if(_baddies.Count == 0)
            WinGame();
        else
            RestartGame();
    }
    //小猪被击败函数
    public void RemoveBaddie(Baddie baddie)
    {
        _baddies.Remove(baddie);

        //检查每次击倒小猪后剩余小猪的数量
        CheckForAllDeadBaddies();
    }
    private void CheckForAllDeadBaddies()
    {
        //剩余小猪数量为0则胜利
        if(_baddies.Count == 0)
            WinGame();
    }
#region 胜利/失败/下一关
    private void WinGame()
    {   
        //显示标语和重启按钮
        _restartScreenObject.SetActive(true);
        //禁用弹弓
        _slingShotHandle.enabled = false;

        //检查是否还有剩余关卡以决定显示或隐藏下一关按钮
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int maxLevels = SceneManager.sceneCountInBuildSettings;
        if(currentSceneIndex +1 < maxLevels)
            _nextLevelImage.enabled = true;
    }
    public void RestartGame()
    {
        DOTween.Clear(true);
        //重启
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
#endregion
}
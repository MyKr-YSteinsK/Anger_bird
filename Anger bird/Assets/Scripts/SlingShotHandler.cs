using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
public class SlingShotHandler : MonoBehaviour
{
    [Header("Line Renderers")]
    //弹弓的皮筋效果图
    [SerializeField] private LineRenderer _LeftLineRenderer;
    [SerializeField] private LineRenderer _RightLineRenderer;

    [Header("Transform Reference")]
    //各种初始默认位置
    [SerializeField] private Transform _LeftStartposition;
    [SerializeField] private Transform _RightStartposition;
    [SerializeField] private Transform _centerPosition;
    [SerializeField] private Transform _idlePosition;
    [SerializeField] private Transform _elasticTransform;

    [Header("弹弓参数")]
    //最大拉弓距离
    [SerializeField] private float _maxDistance = 3.5f;
    //发射力的一个基数
    [SerializeField] private float _shotForce = 9f;
    //下一只小鸟上弓时间
    [SerializeField] private float _timeBetweenBirdRespans = 2f;
    [SerializeField] private float _elasticDivider = 1.2f;
    [SerializeField] private float _maxAnimationTime = 1f;
    //应引用脚本
    [Header("Scripts")]
    [SerializeField] private SlingShotArea _slingShotArea;
    [SerializeField] private CameraManager _cameraManager;
    //引入小鸟
    [Header("Bird")]
    [SerializeField] private AngieBird _angieBirdPrefab;
    [SerializeField] private float _angieBirdPositionOffset = 2f;

    [Header("声音")]
    [SerializeField] private AudioClip _elasticPulledClip;
    [SerializeField] private AudioClip[] _elasticReleasedClips;

    private Vector2 _slingShotLinesPosition;
    private Vector2 _direction;
    private Vector2 _directionNormalized;
    //在弹弓作用范围内按下鼠标
    private bool _clickedWithinArea;
    //小鸟处于可发射状态
    private bool _birdOnSlingShot;
    private AngieBird _spanwedAngieBird;
    private AudioSource _audioSource;
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        //开始的时候隐藏弹弓皮筋
        _LeftLineRenderer.enabled = false;
        _RightLineRenderer.enabled = false;
        //生成小鸟
        SpawnAngieBird();
    }
    private void Update()
    {
        //在范围内点击鼠标
        if(InputManager.WasLeftMouseButtonPressed && _slingShotArea.IswithinSlingshotArea())
        {
            _clickedWithinArea = true;
            //弹弓上有小鸟则播放声音，镜头跟随
            if(_birdOnSlingShot)
            {
                SoundManager.instance.PlayClip(_elasticPulledClip,_audioSource);
                _cameraManager.SwitchToFollowCam(_spanwedAngieBird.transform);
            }
        }
        //按下左键且鼠标&&弹弓范围内&&有小鸟在弹弓上
        if(InputManager.IsLeftMousePressed && _clickedWithinArea && _birdOnSlingShot)
        {
            //显示橡皮筋以及小鸟的位置,小鸟的眼睛朝向
            DrawSlingShot();
            positionAndRotateAngieBird();
        }
        //松开鼠标左键
        if(InputManager.WasLeftMouseButtonReleased && _birdOnSlingShot && _clickedWithinArea)
        {
            //如果还有剩余的可发射小鸟
            if(GameManager.instance.HasEnoughShots())
            {
                _clickedWithinArea = false;
                _birdOnSlingShot = false;
                //发射小鸟
                _spanwedAngieBird.LaunchBird(_direction,_shotForce);
                //播放声音
                SoundManager.instance.PlayRandomClipe(_elasticReleasedClips,_audioSource);

                GameManager.instance.UseShot();
                AnimatesLingShot();

                //过几秒后重新成小鸟
                if(GameManager.instance.HasEnoughShots())
                    StartCoroutine(SpawnAngiBirdAfterTime());
            }
        }
    }
    #region SlingShot Method 弹弓方法
    private void DrawSlingShot()
    {
        //鼠标的位置
        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(InputManager.MousePosition);
        //设置弹弓皮筋的极限距离
        _slingShotLinesPosition = _centerPosition.position + Vector3.ClampMagnitude(touchPosition - _centerPosition.position,_maxDistance);
        //绘画出弹弓皮筋
        SetLines(_slingShotLinesPosition);
        //设置位置
        _direction = (Vector2)_centerPosition.position - _slingShotLinesPosition;
        _directionNormalized = _direction.normalized;
    }
    //生成皮筋
    private void SetLines(Vector2 position)
    {
        //显示橡皮筋
        if(!_LeftLineRenderer.enabled && !_RightLineRenderer.enabled)
        {
            _LeftLineRenderer.enabled = true;
            _RightLineRenderer.enabled = true;
        }
        //两端皮筋初始位置
        _LeftLineRenderer.SetPosition(0,position);
        _LeftLineRenderer.SetPosition(1,_LeftStartposition.position);
        _RightLineRenderer.SetPosition(0,position);
        _RightLineRenderer.SetPosition(1,_RightStartposition.position);
    }

    #endregion
    #region Angie Bird Methods 小鸟方法
    //生成小鸟相关
    private void SpawnAngieBird()
    {  
        _elasticTransform.DOComplete();
        //初始生成位置
        SetLines(_idlePosition.position);
        //方向以及位置
        Vector2 dir = (_centerPosition.position - _idlePosition.position).normalized;
        Vector2 spawnPosition = (Vector2)_idlePosition.position + dir * _angieBirdPositionOffset;
        //生成副本
        _spanwedAngieBird = Instantiate(_angieBirdPrefab, spawnPosition, Quaternion.identity);
        _spanwedAngieBird.transform.right = dir;

        _birdOnSlingShot = true;
    }
    //设置位置和旋转方向
    private void positionAndRotateAngieBird()
    {
        _spanwedAngieBird.transform.position = _slingShotLinesPosition + _directionNormalized * _angieBirdPositionOffset;
        _spanwedAngieBird.transform.right = _directionNormalized;
    }
    //重生小鸟
    private IEnumerator SpawnAngiBirdAfterTime()
    {
        yield return new WaitForSeconds(_timeBetweenBirdRespans);
        SpawnAngieBird();
        //切换镜头至初始弹弓位置
        _cameraManager.SwitchToIdleCam();
    }
    #endregion
    #region Animate SlingShot 弹弓动画
    private void  AnimatesLingShot()
    {
        _elasticTransform.position = _LeftLineRenderer.GetPosition(0);
        float dist = Vector2.Distance(_elasticTransform.position,_centerPosition.position);
        float time = dist / _elasticDivider;

        _elasticTransform.DOMove(_centerPosition.position, time).SetEase(Ease.OutElastic);
        StartCoroutine(AnimateSlingShotLines(_elasticTransform,time));
    }
    private IEnumerator AnimateSlingShotLines(Transform trans,float time)
    {
        float elapsedTime = 0f;
        while(elapsedTime < time && elapsedTime < _maxAnimationTime)
        {
            elapsedTime  += Time.deltaTime;

            SetLines(trans.position);
            yield return null;
        }
    }
    #endregion
}

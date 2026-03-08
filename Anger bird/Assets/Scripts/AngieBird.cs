using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngieBird : MonoBehaviour
{
    //hitbox声音
    [SerializeField] private AudioClip _hitClip;
    private AudioSource _audioSource;
    //2D物理模拟
    private Rigidbody2D _rb;
    //圆形碰撞器
    private CircleCollider2D _circleCollider;

    private bool _hasBeenLaunched;
    private bool _shouldFaceVelDirection;

    private void Awake()
    {
        //寻找分配物理、碰撞、声音组件
        _rb = GetComponent<Rigidbody2D>();
        _circleCollider = GetComponent<CircleCollider2D>();
        _audioSource = GetComponent<AudioSource>();
    }
    private void Start() 
    {
        //设置刚体受物理影响
        _rb.isKinematic = true;
        //禁用碰撞器
        _circleCollider.enabled = false;
    }

    private void FixedUpdate() 
    {
        //小鸟飞行时的反向每帧刷新
        if(_hasBeenLaunched && _shouldFaceVelDirection)
            transform.right = _rb.velocity;
    }

    //发射小鸟
    public void LaunchBird(Vector2 direction,float force)
    {
        _rb.isKinematic = false;
        //启用碰撞
        _circleCollider.enabled = true;
        //施加力
        _rb.AddForce(direction * force,ForceMode2D.Impulse);
        
        _hasBeenLaunched = true;
        _shouldFaceVelDirection = true;
    }
    //碰撞时调用方法
    private void OnCollisionEnter2D(Collision2D collision) 
    {
        _shouldFaceVelDirection = false;
        //播放声音
        SoundManager.instance.PlayClip(_hitClip,_audioSource);
        //作用结束后就摧毁AngieBird以节省资源
        Destroy(this);
    }
}

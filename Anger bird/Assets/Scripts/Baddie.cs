using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baddie : MonoBehaviour
{
    //小猪血量
    [SerializeField] private float _maxHealth = 3f;
    //受伤阈值
    [SerializeField] private float _damageThreshold = 0.2f;
    //死亡播放的粒子效果动画素材
    [SerializeField] private GameObject _baddieDeathParticle;
    //死亡音效
    [SerializeField] private AudioClip _deathClip;
    private float _currentHealth;
    private void Awake() 
    {
        _currentHealth = _maxHealth;
    }
    public void DamageBaddie(float damageAmount)
    {
        //受到一定冲击后扣除生命值
        _currentHealth -= damageAmount;
        //生命值小于0时死亡
        if(_currentHealth <= 0 )
            Die();
    }
    private void Die()
    {
        GameManager.instance.RemoveBaddie(this);
        //小猪死亡时播放死亡粒子动画
        Instantiate(_baddieDeathParticle,transform.position, Quaternion.identity);
        //播放死亡音效
        AudioSource.PlayClipAtPoint(_deathClip,transform.position);

        Destroy(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision) 
    {
        //计算冲击受到的伤害
        float impactVelocity = collision.relativeVelocity.magnitude;
        //受到的冲击大于一定的阈值时才会受到伤害
        if(impactVelocity > _damageThreshold)
            DamageBaddie(impactVelocity);
    }
}

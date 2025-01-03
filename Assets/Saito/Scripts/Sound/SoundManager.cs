using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //プレイヤー
    [SerializeField] //足音
    public AudioClip[] playerFootSteps;
    [SerializeField] //被ダメージ
    public AudioClip playerDamage;
    [SerializeField] //ナイフを振る
    public AudioClip playerKnifeSwing;
    [SerializeField] //拾う
    public AudioClip playerPickUp;
    [SerializeField] //食べる
    public AudioClip playerEat;
    [SerializeField] //飲む
    public AudioClip playerDrink;
    [SerializeField] //回復
    public AudioClip playerHeal;

    //銃
    [SerializeField] //空撃ち
    public AudioClip gunBlankShot;
    [SerializeField] //発砲
    public AudioClip pistolShot;
    [SerializeField] //リロード（マガジン取り出し）
    public AudioClip pistolReloadOut;
    [SerializeField] //リロード（マガジン入れ）
    public AudioClip pistolReloadIn;
    [SerializeField] //発砲
    public AudioClip assaultShot;
    [SerializeField] //リロード（マガジン取り出し）
    public AudioClip assaultReloadOut;
    [SerializeField] //リロード（マガジン入れ）
    public AudioClip assaultReloadIn;
    [SerializeField] //チャージングハンドルを引く
    public AudioClip assaultChargingHandle;
    [SerializeField] //発砲
    public AudioClip shotgunShot;
    [SerializeField] //弾込め
    public AudioClip shotgunBulletIn;

    [SerializeField] //攻撃指示
    public AudioClip whistleAttack;
    [SerializeField] //探知指示
    public AudioClip whistleDetect;

    //ゾンビ
    [SerializeField] //足音
    public AudioClip[] zombieFootStep;
    [SerializeField] //呻き
    public AudioClip zombieVoice;
    [SerializeField] //被ダメージ
    public AudioClip zombieDamage;
    [SerializeField] //死亡
    public AudioClip zombieDead;

    //犬
    [SerializeField] //足音
    public AudioClip dogFootStep;
    [SerializeField] //攻撃時の吠え
    public AudioClip dogAttackBark;
    [SerializeField] //探知時の吠え
    public AudioClip dogDetectBark;

    //システム
    [SerializeField] //ボタン押下
    public AudioClip pushButton;
    [SerializeField] //脱出時
    public AudioClip escapeMap;
    [SerializeField] //インベントリ表示
    public AudioClip inventoryOpen;
    [SerializeField] //インベントリ非表示
    public AudioClip inventoryClose;

    //BGM
    [SerializeField]
    public AudioClip titleBGM;
    [SerializeField] 
    public AudioClip nomalBGM;  

    private AudioClip nextBGM;
    private float changeBGMSpeed;
    private float maxVolume;
    private float currentVolume;

    private bool isFadeOut = false;
    private bool isFadeIn = false;

    //デバッグ用
    [SerializeField]
    private AudioClip[] debugPlaySounds;
    AudioSource audioSource;
    private void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        maxVolume = audioSource.volume;
        currentVolume = maxVolume;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Keypad0))
        {
            audioSource.PlayOneShot(debugPlaySounds[0]);
        }
        if(Input.GetKeyDown(KeyCode.Keypad1))
        {
            audioSource.PlayOneShot(debugPlaySounds[1]);
        }
        if(Input.GetKeyDown(KeyCode.Keypad2))
        {
            audioSource.PlayOneShot(debugPlaySounds[2]);
        }


        if(isFadeOut)
        {
            currentVolume -= changeBGMSpeed * Time.deltaTime;
            if (currentVolume <= 0)
            {
                isFadeOut = false;
                currentVolume = 0;

                audioSource.clip = nextBGM;
                nextBGM = null;
            }
        }
        else if(isFadeIn)
        {
            currentVolume += changeBGMSpeed * Time.deltaTime;
            if (currentVolume >= maxVolume)
            {
                isFadeIn = false;
                currentVolume = maxVolume;
            }
        }

        audioSource.volume = currentVolume;
    }

    public void ChangeBGM(AudioClip _bgm, float _speed)
    {
        nextBGM = _bgm;
        changeBGMSpeed = _speed;
        isFadeOut = true;
        isFadeIn = true;
    }

    public void Play2DSE(AudioClip _se)
    {
        audioSource.PlayOneShot(_se);
    }

}

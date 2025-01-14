using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeManager : MonoBehaviour
{
    // 当たり判定処理済みを記録  
    private Dictionary<int, bool> mHitMasters { get; } = new Dictionary<int, bool>();

    [SerializeField]//当たり判定
    Collider mCollider;

    //コルーチンキャンセル用
    Coroutine mAttackCoroutine;

    //与えるダメージ
    [SerializeField] 
    private int mAttackDamage = 2;

    private void Start()
    {
        mCollider = gameObject.GetComponent<Collider>();
        mCollider.enabled = false;
    }


    /// <summary>
    /// 攻撃開始
    /// </summary>
    public void StartAttack()
    {
        Debug.Log("ナイフ攻撃開始");

        if (mAttackCoroutine != null)
            AttackCancel();//再生中のコルーチンがあればキャンセル

        mAttackCoroutine = StartCoroutine(attack());//コルーチン開始
    }

    /// <summary>
    /// 攻撃キャンセル用
    /// </summary>
    public void AttackCancel()
    {
        //とりあえずコライダーを無効化にする
        mCollider.enabled = false;

        if (mAttackCoroutine == null) return;

        //コルーチン停止
        StopCoroutine(mAttackCoroutine);

        mAttackCoroutine = null;
    }

    IEnumerator attack()
    {
        mHitMasters.Clear(); // リセット
        mCollider.enabled = true;

        yield return new WaitForSeconds(1.3f);

        mCollider.enabled = false;
        mAttackCoroutine = null;
    }

    void OnTriggerEnter(Collider other)
    {
        string hit_tag = other.tag;
        //対象のタグ以外は接触しない
        if (hit_tag != "Body" && hit_tag != "Head") return;

        // 追加
        // 攻撃対象部位ならHitZoneが取得できる
        var hit_zone = other.GetComponent<ZombieHitZone>();
        if (hit_zone == null) return;

        // 攻撃対象部位の親のインスタンスIDで重複した攻撃を判定
        int master_id = hit_zone.Master.GetInstanceID();
        if (mHitMasters.ContainsKey(master_id)) return;
        mHitMasters[master_id] = true;

        Vector3 hit_pos = other.ClosestPointOnBounds(transform.position);

        Debug.Log("Hit!");
        // ダメージ計算とかこのへんで実装できます
        hit_zone.Master.TakeDamage(hit_tag, mAttackDamage, hit_pos);

    }
}

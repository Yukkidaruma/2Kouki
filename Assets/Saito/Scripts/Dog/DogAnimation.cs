using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 犬アニメーションクラス
/// 犬のアニメーションを呼び出す
/// </summary>
public class DogAnimation : DogBase
{
    [SerializeField]//アニメーター
    private Animator mAnimator;

    //移動アニメーションの種類
    //Managerの方で管理してもいいかも
    enum MOVE_TYPE
    {
        IDLE,
        WALK,
        RUN
    }

    //現在の移動アニメーション
    private MOVE_TYPE mCurrentMoveType;


    /// <summary>
    /// 初期設定
    /// </summary>
    public override void SetUpDog()
    {
        mCurrentMoveType = MOVE_TYPE.IDLE;
    }

    /// <summary>
    /// アタック
    /// 攻撃アニメーション再生
    /// </summary>
    public void Attack()
    {
        mCurrentMoveType = MOVE_TYPE.IDLE;

        Debug.Log("dog:Attack");
        mAnimator.SetTrigger("Attack");
    }

    /// <summary>
    /// 立ち止まり
    /// 
    /// </summary>
    public void Idle()
    {
        //同じアニメーションを複数呼び出ししないように
        if (mCurrentMoveType == MOVE_TYPE.IDLE) return;
        mCurrentMoveType = MOVE_TYPE.IDLE;

        Debug.Log("dog:Idle");
        mAnimator.SetTrigger("Idle");
    }

    /// <summary>
    /// 歩き
    /// 
    /// </summary>
    public void Walk()
    {
        //同じアニメーションを複数呼び出ししないように
        if (mCurrentMoveType == MOVE_TYPE.WALK) return;
        mCurrentMoveType = MOVE_TYPE.WALK;

        Debug.Log("dog:Walk");
        mAnimator.SetTrigger("Walk");
    }

    /// <summary>
    /// 走り
    /// </summary>
    public void Run()
    {
        //同じアニメーションを複数呼び出ししないように
        if (mCurrentMoveType == MOVE_TYPE.RUN) return;
        mCurrentMoveType = MOVE_TYPE.RUN;

        Debug.Log("dog:Run");
        mAnimator.SetTrigger("Run");
    }

}

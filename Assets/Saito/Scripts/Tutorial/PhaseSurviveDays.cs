using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>生き残らせるフェイズ</para>
/// 生存日数が3日になるまで自由行動させる
/// </summary>
public class PhaseSurviveDays : TutorialBase
{
    [SerializeField] TimeController m_timeController;

    public override void SetUpPhase()
    {
        m_tutorialManager.SetText("助けが来る3日目まで生き残ろう");
    }

    public override void UpdatePhase()
    {
        //現在の日数取得
        int dayCount = m_timeController.GetDayCount();

        //3日目なら条件を満たす
        if(dayCount >= 3)
        {
            //次のフェーズに進める
            m_tutorialManager.NextPhase();
        }
    }

    public override void EndPhase()
    {
        m_tutorialManager.HideText();
    }
}

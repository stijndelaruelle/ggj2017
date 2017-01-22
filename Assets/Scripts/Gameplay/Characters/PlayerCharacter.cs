using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    [SerializeField]
    private BrainwaveDevice m_BrainwaveDevice;

    [SerializeField]
    private CachierCharacter m_Cachier;

    private bool m_IsCrazyTalking = false;

    private void Start()
    {
        m_Cachier.ChangeTicketsEvent += OnChangeTickets;
    }

    public override void BuyTicket(Vector3 position)
    {
        StartCoroutine(BuyTicketRoutine(position));
    }

    private IEnumerator BuyTicketRoutine(Vector3 position)
    {
        m_IsCheering = true;
        yield return new WaitForSeconds(3.0f);
        m_IsCheering = false;

        MoveToPositionSequentially(position);
    }

    protected override void ExecuteNegativeCommand()
    {
        base.ExecuteNegativeCommand();

        //Change a random inut setting
        int rand = UnityEngine.Random.Range(0, 100);
        if (rand > 50)
        {
            m_BrainwaveDevice.InversedControls = !m_BrainwaveDevice.InversedControls;
        }
        else
        {
            m_BrainwaveDevice.SwitchedControls = !m_BrainwaveDevice.SwitchedControls;
        }
    }

    //Make sure crazy talking works
    private void OnChangeTickets(int tickets, int maxTickets)
    {
        if (tickets <= 1)
        {
            m_IsCrazyTalking = true;
        }
    }

    protected override void UpdateAnimation()
    {
        if (m_HasReachedDestination == false)
        {
            if (m_SkeletonAnimation.AnimationName != "walk")
                m_SkeletonAnimation.AnimationName = "walk";

            return;
        }

        if (m_IsCheering == true)
        {
            if (m_SkeletonAnimation.AnimationName != "cheer")
                m_SkeletonAnimation.AnimationName = "cheer";

            return;
        }

        if (m_IsSad == true)
        {
            if (m_SkeletonAnimation.AnimationName != "sad")
                m_SkeletonAnimation.AnimationName = "sad";

            return;
        }

        if (m_IsCrazyTalking == true)
        {
            if (m_SkeletonAnimation.AnimationName != "crazytalking")
                m_SkeletonAnimation.AnimationName = "crazytalking";

            return;
        }

        if (m_SkeletonAnimation.AnimationName == null || m_SkeletonAnimation.AnimationName != "idle")
        {
            m_SkeletonAnimation.AnimationName = "idle";
        }
    }
}

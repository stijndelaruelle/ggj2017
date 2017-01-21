using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    [SerializeField]
    private BrainwaveDevice m_BrainwaveDevice;

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
}

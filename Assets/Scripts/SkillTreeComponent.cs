using UnityEngine;
using UnityEngine.UI;

public class SkillTreeComponent : MonoBehaviour
{
    public bool isBought;
    public bool isLocked;
    public int minLevel;
    public SkillTreeComponent[] nextComponent;

    [Header("Buff1")]
    public SkillTreeManager.Buffs selectedBuff1;    
    public int buffPower1;

    [Header("Buff2")]
    public SkillTreeManager.Buffs selectedBuff2;
    public int buffPower2;

    public void UnlockNext()
    {
        foreach (SkillTreeComponent stc in nextComponent)
        {
            stc.isLocked = false;
            stc.transform.GetChild(0).GetComponent<Image>().enabled = false;
        }
    }
}

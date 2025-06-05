using UnityEngine;
using UnityEngine.UI;

public class SkillTreeComponent : MonoBehaviour
{
    #region Variables
    public bool isBought;       // Not beeing used
    public bool isLocked;       // Using but not the way intended
    public int minLevel;        // Not beeing used
    public SkillTreeComponent[] nextComponent;

    [Header("Buff1")]
    public SkillTreeManager.Buffs selectedBuff1;    
    public int buffPower1;

    [Header("Buff2")]
    public SkillTreeManager.Buffs selectedBuff2;
    public int buffPower2;
    #endregion

    #region Methods
    /// <summary>
    /// Call this method to unlock the next components
    /// </summary>
    public void UnlockNext()
    {
        // Go through all components this component can unlock
        foreach (SkillTreeComponent stc in nextComponent)
        {
            stc.isLocked = false;
            stc.transform.GetChild(0).GetComponent<Image>().enabled = false;
        }
    }
    #endregion
}

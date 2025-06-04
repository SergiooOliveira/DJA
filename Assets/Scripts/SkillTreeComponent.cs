using UnityEngine;
using UnityEngine.UI;

public class SkillTreeComponent : MonoBehaviour
{
    public bool isBought;
    public bool isLocked;
    public int minLevel;
    public SkillTreeComponent[] nextComponent;

    //public SkillTreeComponent(bool isBought, bool isLocked, int minLevel, Transform[] next)
    //{
    //    this.isBought = isBought;
    //    this.isLocked = isLocked;
    //    this.minLevel = minLevel;
    //    this.nextComponent = next;
    //}

    public void UnlockNext()
    {
        foreach (SkillTreeComponent stc in nextComponent)
        {
            stc.isLocked = false;
            stc.transform.GetChild(0).GetComponent<Image>().enabled = false;
        }
    }
}

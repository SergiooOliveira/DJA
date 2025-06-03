using UnityEngine;

public class SkillTreeComponent : MonoBehaviour
{
    public bool isBought;
    public bool isLocked;
    public int minLevel;
    public Transform previousComponent;

    public SkillTreeComponent(bool isBought, int minLevel, Transform previous)
    {
        this.isBought = isBought;
        this.isLocked = IsLocked(previous);
        this.minLevel = minLevel;
        this.previousComponent = previous;
    }

    private bool IsLocked(Transform previous)
    {
        return previous.transform.GetComponent<SkillTreeComponent>().isBought ? true : false;
    }
}

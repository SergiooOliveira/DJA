using UnityEngine;
using UnityEngine.UI;

public class SkillTreeManager : MonoBehaviour
{
    public static SkillTreeManager Instance;
    public GameObject Canvas;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);        
    }

    public void ClickedObject()
    {
        GameObject clickedButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;        
        SkillTreeComponent skillTreeComponentSelected = clickedButton.GetComponent<SkillTreeComponent>();

        Debug.Log($"Clicked on: {clickedButton.name} and it's {skillTreeComponentSelected.isLocked}");

        skillTreeComponentSelected.GetComponent<Image>().color = Color.green;
        skillTreeComponentSelected.UnlockNext();
    }
}

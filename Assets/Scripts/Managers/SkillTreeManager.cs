using UnityEngine;
using UnityEngine.UI;

public class SkillTreeManager : MonoBehaviour
{
    public static SkillTreeManager Instance;
    public GameObject Canvas;
    public enum Buffs { Nothing, Health, Strength, Armor }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);        
    }

    public void ClickedObject()
    {
        bool upgradeSuccessful = false;
        GameObject clickedButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;        
        SkillTreeComponent skillTreeComponentSelected = clickedButton.GetComponent<SkillTreeComponent>();

        Debug.Log($"Clicked on: {clickedButton.name} and it's {skillTreeComponentSelected.isLocked}");

        if (skillTreeComponentSelected.isLocked) return;
        if (Player.Instance.skillPoints == 0) return;

        skillTreeComponentSelected.GetComponent<Image>().color = Color.green;
        skillTreeComponentSelected.UnlockNext();

        // Use this space to send the buff to the Player
        Upgrade upgrade1 = null, upgrade2 = null;

        switch (skillTreeComponentSelected.selectedBuff1)
        {
            case Buffs.Nothing:
                break;
            case Buffs.Health:
                upgrade1 = new Upgrade(skillTreeComponentSelected.name, "Health Upgrade", "", 0, "Sprites/HpBuffRare", 0, ("Health", skillTreeComponentSelected.buffPower1));
                break;
            case Buffs.Strength:
                upgrade1 = new Upgrade(skillTreeComponentSelected.name, "Attack Upgrade", "", 0, "Sprites/AttackBuffRare", 0, ("Strength", skillTreeComponentSelected.buffPower1));
                break;
            case Buffs.Armor:
                upgrade1 = new Upgrade(skillTreeComponentSelected.name, "Armor Upgrade", "", 0, "Sprites/ArmorBuffRare", 0, ("Armor", skillTreeComponentSelected.buffPower1));
                break;
        }

        switch (skillTreeComponentSelected.selectedBuff2)
        {
            case Buffs.Nothing:
                break;
            case Buffs.Health:
                upgrade2 = new Upgrade(skillTreeComponentSelected.name, "Health Upgrade", "", 0, "Sprites/HpBuffRare", 0, ("Health", skillTreeComponentSelected.buffPower2));
                break;
            case Buffs.Strength:
                upgrade2 = new Upgrade(skillTreeComponentSelected.name, "Attack Upgrade", "", 0, "Sprites/AttackBuffRare", 0, ("Strength", skillTreeComponentSelected.buffPower2));
                break;
            case Buffs.Armor:
                upgrade2 = new Upgrade(skillTreeComponentSelected.name, "Armor Upgrade", "", 0, "Sprites/ArmorBuffRare", 0, ("Armor", skillTreeComponentSelected.buffPower2));
                break;
        }

        if (upgrade1 != null)
        {
            Debug.Log(upgrade1.ToString());
            Upgrades.Instance.playerUpgrades.Add(upgrade1);
            GameManager.Instance.AddStats(upgrade1);
            upgradeSuccessful = true;
        }

        if (upgrade2 != null)
        {
            Debug.Log(upgrade2.ToString());
            Upgrades.Instance.playerUpgrades.Add(upgrade2);
            GameManager.Instance.AddStats(upgrade2);
        }

        if (upgradeSuccessful)
        {            
            Player.Instance.skillPoints--;
            GameManager.Instance.UpdateSkillPoints();
        }
    }
}

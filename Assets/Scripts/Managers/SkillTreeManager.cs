using UnityEngine;
using UnityEngine.UI;

public class SkillTreeManager : MonoBehaviour
{
    #region Variables
    public static SkillTreeManager Instance;
    public GameObject Canvas;
    public enum Buffs { Nothing, Health, Strength, Armor }
    #endregion

    #region MonoBehaviour
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);        
    }
    #endregion

    #region Methods
    /// <summary>
    /// This method is used to identify what upgrade was selected from the Skill Tree
    /// </summary>
    public void ClickedObject()
    {
        bool upgradeSuccessful = false;         // flag

        // Get correct component
        GameObject clickedButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;        
        SkillTreeComponent skillTreeComponentSelected = clickedButton.GetComponent<SkillTreeComponent>();

        Debug.Log($"Clicked on: {clickedButton.name} and it's {skillTreeComponentSelected.isLocked}");

        // Cheking if player has skill points and if upgrade isLocked or not
        if (skillTreeComponentSelected.isLocked) return;
        if (Player.Instance.skillPoints == 0) return;

        // Visual feedback when buying upgrade
        skillTreeComponentSelected.GetComponent<Image>().color = Color.green;
        skillTreeComponentSelected.UnlockNext();

        // Variables for upgrade 1 and 2, both null for checking later
        Upgrade upgrade1 = null, upgrade2 = null;

        // Iterating first Buff
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

        // Iterating second Buff
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

        // Checking if first upgrade was successfully created
        if (upgrade1 != null)
        {
            Debug.Log(upgrade1.ToString());
            Upgrades.Instance.playerUpgrades.Add(upgrade1);
            GameManager.Instance.AddStats(upgrade1);
            upgradeSuccessful = true;
        }

        // Checking if second upgrade was successfully created
        if (upgrade2 != null)
        {
            Debug.Log(upgrade2.ToString());
            Upgrades.Instance.playerUpgrades.Add(upgrade2);
            GameManager.Instance.AddStats(upgrade2);
        }

        // Case everything was successful deduct Player skill points and update UI
        if (upgradeSuccessful)
        {
            Player.Instance.skillPoints--;
            GameManager.Instance.UpdateSkillPoints();
        }
        else
            Debug.LogError($"Selected upgrade error: {skillTreeComponentSelected.name}"); // In case something goes wrong
    }
    #endregion
}

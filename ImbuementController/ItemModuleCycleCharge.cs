using ThunderRoad;

namespace ImbuementController
{
    public class ItemModuleCycleCharge : ItemModule
    {
        public string[] spellIDs = { "Fire", "Lightning", "Gravity"};
        public float energyStepSize = 5.0f;
        public bool useTriggerToCycle = false;
        public bool autoImbue = false;
        public string autoImbueSpell = "Fire";
        public bool permanentImbue = false;

        public override void OnItemLoaded(Item item)
        {
            base.OnItemLoaded(item);
            if (autoImbue) item.gameObject.AddComponent<ItemAutoImbue>();
            else item.gameObject.AddComponent<ItemCycleCharge>();
        }
    }
}

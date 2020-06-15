using ThunderRoad;

namespace ImbuementController
{
    public class ItemModuleCycleCharge : ItemModule
    {
        public string[] spellIDs = { "Fire", "Lightning", "Gravity"};
        public float energyStepSize = 5.0f;
        public bool useTriggerToCycle = false;

        public override void OnItemLoaded(Item item)
        {
            base.OnItemLoaded(item);
            item.gameObject.AddComponent<ItemCycleCharge>();
        }
    }
}

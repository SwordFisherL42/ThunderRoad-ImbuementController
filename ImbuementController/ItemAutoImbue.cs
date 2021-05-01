using System.Collections;
using UnityEngine;
using ThunderRoad;

namespace ImbuementController
{
    class ItemAutoImbue : MonoBehaviour
    {
        protected Item item;
        protected ItemModuleCycleCharge module;

        private SpellCastCharge autoImbueSpell;
        private bool isActive;

        private Imbue itemMainImbue;

        private void Awake()
        {
            item = this.GetComponent<Item>();
            module = item.data.GetModule<ItemModuleCycleCharge>();

            try { autoImbueSpell = Catalog.GetData<SpellCastCharge>(module.autoImbueSpell, true); }
            catch { Debug.LogError(string.Format("[Fisher-ImbuementController] Exception! Unable to Find Spell {0}", module.autoImbueSpell)); }

            TryGetItemImbue();

        }

        private void TryGetItemImbue()
        {
            try
            {
                if (item.imbues.Count > 0) itemMainImbue = item.imbues[0];
                else itemMainImbue = null;
            }
            catch { Debug.LogError(string.Format("[Fisher-ImbuementController] Exception! Unable to Find/Set main Imbue for item {0}", item.name)); }
        }

        private void Start()
        {
            if (itemMainImbue == null) TryGetItemImbue();
            if (autoImbueSpell != null && itemMainImbue != null)
            {
                try { StartCoroutine(TransferDeltaEnergy(item.imbues[0], autoImbueSpell, module.energyStepSize)); }
                catch { }
                if (!module.permanentImbue) StartCoroutine(DestroyOnceReady());
            }

        }

        private void LateUpdate()
        {
            if (itemMainImbue == null) TryGetItemImbue();

            if (isActive || itemMainImbue == null || autoImbueSpell == null || !module.permanentImbue) return;

            if (itemMainImbue.energy < itemMainImbue.maxEnergy)
            {
                try { itemMainImbue.Transfer(autoImbueSpell, module.energyStepSize); }
                catch { }
                
            }
        }

        private IEnumerator DestroyOnceReady()
        {
            do yield return null;
            while (isActive);
            Destroy(this.gameObject.GetComponent<ItemAutoImbue>());
        }

        private IEnumerator TransferDeltaEnergy(Imbue itemImbue, SpellCastCharge activeSpell, float energyDelta = 1.0f)
        {
            int counts = (int)Mathf.Round(200.0f / energyDelta);
            isActive = true;
            for (int i = 0; i < counts; i++)
            {
                try
                {
                    itemImbue.Transfer(activeSpell, energyDelta);
                    if (itemImbue.energy >= itemImbue.maxEnergy) { break; }
                }
                catch { }
                yield return null;
            }
            isActive = false;
            yield return null;
        }

    }
}

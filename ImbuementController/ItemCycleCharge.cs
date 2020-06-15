﻿using System.Collections;
using UnityEngine;
using ThunderRoad;

/* Description: An Item plugin for `ThunderRoad`, allowing users to cycle Imbuement charges using a
 * pre-defined array of spellID strings defined in the Item Module
 * 
 * author: SwordFisherL42 ("Fisher")
 * date: 06/15/2020
 * 
 */

namespace ImbuementController
{
    public class ItemCycleCharge : MonoBehaviour
    {
        protected Item item;
        protected ItemModuleCycleCharge module;
        private int counter = 0;
        private string currentSpell;
        private SpellCastCharge activeSpell;
        private bool isActive;

        private void Awake()
        {
            item = this.GetComponent<Item>();
            module = item.data.GetModule<ItemModuleCycleCharge>();
            item.OnHeldActionEvent += OnHeldAction;
            counter = 0;
        }

        public void OnHeldAction(Interactor interactor, Handle handle, Interactable.Action action)
        {
            if ((!module.useTriggerToCycle && action == Interactable.Action.AlternateUseStart)
                ||(module.useTriggerToCycle && action == Interactable.Action.UseStart))
            {
                if (isActive) return;
                if (counter >= module.spellIDs.Length){
                    counter = 0;
                    StartCoroutine(SafeChargeDrain(item.imbues[0], module.energyStepSize));
                    return;
                }
                else
                {
                    currentSpell = module.spellIDs[counter];
                    counter++;
                    activeSpell = Catalog.GetData<SpellCastCharge>(currentSpell, true);
                    StartCoroutine(TransferDeltaEnergy(item.imbues[0], activeSpell, module.energyStepSize));
                }   
            }
        }

        private IEnumerator TransferDeltaEnergy(Imbue itemImbue, SpellCastCharge activeSpell, float energyDelta = 1.0f)
        {
            int counts = (int) Mathf.Round(200.0f/energyDelta);
            isActive = true;
            for (int i = 0; i < counts; i++)
            {
                try
                {
                    itemImbue.Transfer(activeSpell, energyDelta);
                    if (itemImbue.energy == itemImbue.maxEnergy) { break; }
                }
                catch { }
                yield return null;
            }
            isActive = false;
            yield return null;
        }

        private IEnumerator SafeChargeDrain(Imbue itemImbue, float energyDelta = 1.0f)
        {
            isActive = true;
            while (itemImbue.energy > 0.1f)
            {
                try
                {
                    if (itemImbue.CanConsume(energyDelta))
                    {
                        itemImbue.ConsumeInstant(energyDelta);
                    }
                    else if (itemImbue.CanConsume(itemImbue.energy - 0.1f))
                    {
                        itemImbue.ConsumeInstant(itemImbue.energy - 0.1f);
                        break;
                    }
                }
                catch { }
                yield return null;
            }
            isActive = false;
            yield return null;
        }

    }
}

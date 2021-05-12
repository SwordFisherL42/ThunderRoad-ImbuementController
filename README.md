# ThunderRoad-ImbuementController
A plugin for the VR game 'Blade &amp; Sorcery', which adds additional controls over imbuement states.

Currently supported plugin modes are Auto-Imbue and Spell-Cycle. For a complete user guide, see the [Google Docs User/Modder Guide](https://docs.google.com/document/d/1rudXxt6vyrHctcFpQDYolH2bckjffAKI_l773_8c3Jw)

Each mode can be enabled via the following module settings:

## Option 1: Auto Imbue Mode
```
"modules": [
  {
    "$type": "ImbuementController.ItemModuleCycleCharge, AutoImbueFramework",
    "autoImbue": true,
    "autoImbueSpell": "Fire",
    "permanentImbue": true
  }
]
```

## Option 2: Spell Cycle Mode
```
"modules": [
  {
    "$type": "ImbuementController.ItemModuleCycleCharge, AutoImbueFramework",
    "spellIDs": ["Fire", "Lightning", "Gravity"],
    "useTriggerToCycle": false,
    "permanentImbue": true
  }
]
```

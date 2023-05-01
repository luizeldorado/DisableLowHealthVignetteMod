using System.Collections.Generic;
using Modding;
using UnityEngine;

namespace DisableLowHealthVignette {
    public class DisableLowHealthVignette : Mod, IMenuMod, IGlobalSettings<GlobalSettingsClass> {
        new public string GetName() => "DisableLowHealthVignette";
        public override string GetVersion() => "1.1.0.0";

        public bool ToggleButtonInsideMenu => false;

        static GlobalSettingsClass globalSettings = new GlobalSettingsClass();

        GameObject vignette;
        GameObject furyEffects;

        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects) {
            On.HUDCamera.OnEnable += OnHUDCameraOnEnable;

            UpdateEffects();
        }

        public void Unload() {
            On.HUDCamera.OnEnable -= OnHUDCameraOnEnable;

            EnableVignette();
            EnableFuryEffects();
        }

        void OnHUDCameraOnEnable(On.HUDCamera.orig_OnEnable orig, HUDCamera self) {
            orig(self);

            UpdateEffects();
        }

        void UpdateEffects() {
            if (globalSettings.showLow) {
                EnableVignette();
            } else {
                DisableVignette();
            }

            if (globalSettings.showFury) {
                EnableFuryEffects();
            } else {
                DisableFuryEffects();
            }
        }

        void DisableVignette() {
            GameObject currentVignette = GameObject.Find("Low Health Vignette");
            if (currentVignette) {
                currentVignette.SetActive(false);
                vignette = currentVignette;
            }
        }

        void EnableVignette() {
            if (vignette) {
                vignette.SetActive(true);
                vignette = null;
            }
        }

        void DisableFuryEffects() {
            GameObject currentFuryEffects = GameObject.Find("fury_effects_v2");
            if (currentFuryEffects) {
                currentFuryEffects.SetActive(false);
                furyEffects = currentFuryEffects;
            }
        }

        void EnableFuryEffects() {
            if (furyEffects) {
                furyEffects.SetActive(true);
                furyEffects = null;
            }
        }

        public List<IMenuMod.MenuEntry> GetMenuData(IMenuMod.MenuEntry? toggleButtonEntry) {
            return new List<IMenuMod.MenuEntry> {
                new IMenuMod.MenuEntry {
                    Name = "Show low health vignette effect?",
                    Values = new string[] {
                        "No",
                        "Yes",
                    },
                    Saver = opt => {
                        globalSettings.showLow = opt switch {
                            0 => false,
                            1 => true,
                            _ => throw new System.NotImplementedException(),
                        };
                        UpdateEffects();
                    },
                    Loader = () => globalSettings.showLow switch {
                        false => 0,
                        true => 1,
                    },
                },
                new IMenuMod.MenuEntry {
                    Name = "Show Fury of the Fallen effect?",
                    Values = new string[] {
                        "No",
                        "Yes",
                    },
                    Saver = opt => {
                        globalSettings.showFury = opt switch {
                            0 => false,
                            1 => true,
                            _ => throw new System.NotImplementedException(),
                        };
                        UpdateEffects();
                    },
                    Loader = () => globalSettings.showFury switch {
                        false => 0,
                        true => 1,
                    },
                }
            };
        }

        public void OnLoadGlobal(GlobalSettingsClass s) {
            globalSettings = s;
        }

        public GlobalSettingsClass OnSaveGlobal() {
            return globalSettings;
        }
    }

    public class GlobalSettingsClass {
        public bool showLow = false;
        public bool showFury = false;
    }
}
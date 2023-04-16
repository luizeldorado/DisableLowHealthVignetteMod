using System.Collections.Generic;
using Modding;
using UnityEngine;

namespace DisableLowHealthVignette {
    public class DisableLowHealthVignette : Mod, ITogglableMod {
        new public string GetName() => "DisableLowHealthVignette";
        public override string GetVersion() => "1.0.0.0";

        public GameObject vignette;

        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects) {
            On.HUDCamera.OnEnable += OnHUDCameraOnEnable;
            DisableVignette();
        }

        public void Unload() {
            On.HUDCamera.OnEnable -= OnHUDCameraOnEnable;
            EnableVignette();
            vignette = null;
        }

        private void OnHUDCameraOnEnable(On.HUDCamera.orig_OnEnable orig, HUDCamera self) {
            orig(self);

            DisableVignette();
        }

        public void DisableVignette() {
            vignette = GameObject.Find("Low Health Vignette");
            if (vignette) {
                vignette.SetActive(false);
            }
        }

        public void EnableVignette() {
            if (vignette) {
                vignette.SetActive(true);
            }
        }
    }
}
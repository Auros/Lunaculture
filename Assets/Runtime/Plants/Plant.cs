using System;
using Lunaculture.Items;
using UnityEngine;

namespace Lunaculture.Plants
{
    // Should always be a prefab
    public class Plant : MonoBehaviour
    {
        public bool Harvestable => GrowthPercent >= 1;

        // Read-only variable tracking the growth percent of this plant
        public float GrowthPercent { get; private set; }

        [field: Header("Measured in minutes assuming default game speed.")]
        [field: SerializeField]
        public float GrowTime { get; private set; } = 1f;

        [field: Header("Amount of water that must be used to grow this crop")]
        public float WaterUsage { get; private set; }

        [field: Header("All item drops must have an associated drop percentage [0-1]")]
        [field: SerializeField]
        public Item[] Drops { get; private set; } = Array.Empty<Item>();

        [field: SerializeField]
        public float[] DropPercentages { get; private set; } = Array.Empty<float>();

        [field: Header("Contributes oxygen to the network (reserved for trees)")]
        [field: SerializeField]
        public float OxygenProduction { get; private set; } = 0f;

        // TODO(Caeden): Minimum water requirement
        private void Update()
        {
            var previousGrowth = GrowthPercent;

            GrowthPercent = Mathf.Clamp01(GrowthPercent + (Time.deltaTime * Time.timeScale / GrowTime / 60f));
        
            // TODO(Caeden): Tween?
            if (!Mathf.Approximately(GrowthPercent, previousGrowth))
            {
                transform.localScale = GrowthPercent * Vector3.one;
            }
        }
    }
}

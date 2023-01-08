using System;
using System.Collections.Generic;
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
        
        [field: Header("All item drops must have an associated drop percentage [0-1]")]
        [field: SerializeField]
        public Item[] Drops { get; private set; } = Array.Empty<Item>();

        [field: SerializeField]
        public float[] DropPercentages { get; private set; } = Array.Empty<float>();

        [field: Header("Objects that are randomly rotated upon Start()")]
        [field: SerializeField]
        public List<GameObject> RandomlyRotatedObjects { get; private set; }

        [field: Header("Contributes oxygen to the network (reserved for trees)")]
        [field: SerializeField]
        public float OxygenProduction { get; private set; } = 0f;
        
        [field: SerializeField]
        public Animator Animator { get; private set; }

        public event Action? OnPlantFinishedGrowing;

        public PlantGrowthStatus GrowthStatus = PlantGrowthStatus.NotWatered;
        private void Start()
        {
            foreach (var gameObject in RandomlyRotatedObjects)
            {
                gameObject.transform.localEulerAngles = new Vector3(gameObject.transform.localEulerAngles.x, UnityEngine.Random.Range(-360f, 360f), gameObject.transform.localEulerAngles.z);
            }

            // may need to redo stuff when auto watering is developed
            GrowthStatus = PlantGrowthStatus.NotWatered;
        }

        private void Update()
        {
            // right now visuals are handled by animators that may need to be adjusted for time
            // this is also not hooked up to faster timecontroller values
            if (GrowthStatus == PlantGrowthStatus.NotWatered)
            {
                Animator.speed = 0;
                // wait for water. show icon
            }
            else if (GrowthStatus == PlantGrowthStatus.Growing)
            {
                GrowthPercent = Mathf.Clamp01(GrowthPercent + (Time.deltaTime * Time.timeScale / GrowTime / 60f));
                if (GrowthPercent >= 1)
                {
                    Debug.Log("Plant finished growing");
                    //TODO: properly handle trees 
                    GrowthStatus = PlantGrowthStatus.GrownAndReadyToPermaHarvest;
                    
                    OnPlantFinishedGrowing?.Invoke();
                }
            }
        }
        
        // water the plant. used by whatever watering can
        public void Water()
        {
            Animator.speed = Time.timeScale;
            GrowthStatus = PlantGrowthStatus.Growing;
        }
    }
}

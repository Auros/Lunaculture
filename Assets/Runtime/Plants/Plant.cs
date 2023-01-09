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

        [field: SerializeField]
        public bool IsTree { get; private set; } = false;

        [field: Header("Objects that are randomly rotated upon Start()")]
        [field: SerializeField]
        public List<GameObject> RandomlyRotatedObjects { get; private set; }

        [field: Header("Contributes oxygen to the network (reserved for trees)")]
        [field: SerializeField]
        public float OxygenProduction { get; private set; } = 0f;
        
        [field: Header("Animation")]
        [field: SerializeField]
        public Animator Animator { get; private set; }
        
        /*[field: SerializeField]
        public AnimationClip? Animation { get; private set; }*/

        [field: SerializeField]
        public GameObject NeedsWaterIcon { get; private set; }

        private GameObject? DoneGrowingIcon = null;
        
        [field: SerializeField]
        public MeshRenderer CropBottom { get; private set; }
        
        [field: SerializeField]
        public Material UnwateredDirt { get; private set; }
        
        [field: SerializeField]
        public Material WateredDirt { get; private set; }
        
        [field: SerializeField]
        public Item Item { get; private set; }


        public event Action<PlantStatusEvent>? PlantStatusUpdated;
        
        private int _cachedGrowthPropertyID = 0;

        public PlantGrowthStatus GrowthStatus
        {
            get { return _growthStatus; }
            set
            {
                PlantStatusUpdated?.Invoke(new (value));
                _growthStatus = value;
            }
        }

        private PlantGrowthStatus _growthStatus = PlantGrowthStatus.NotWatered;
        
        private void Start()
        {
            DoneGrowingIcon = Instantiate(NeedsWaterIcon);
            
            DoneGrowingIcon.transform.parent = transform;
            DoneGrowingIcon.transform.position = NeedsWaterIcon.transform.position;
            
            DoneGrowingIcon.GetComponent<SpriteRenderer>().sprite = Item.Icon;
            if(IsTree) _cachedGrowthPropertyID = Animator.StringToHash("SecondGrowth");

            if (!Animator.enabled) Animator.enabled = true;
                
            foreach (var gameObject in RandomlyRotatedObjects)
            {
                gameObject.transform.localEulerAngles = new Vector3(gameObject.transform.localEulerAngles.x, UnityEngine.Random.Range(-360f, 360f), gameObject.transform.localEulerAngles.z);
            }

            // may need to redo stuff when auto watering is developed
            PlantStatusUpdated += PlantStatusUpdatedInternal;
            
            GrowthStatus = PlantGrowthStatus.NotWatered;
        }

        private void PlantStatusUpdatedInternal(PlantStatusEvent statusEvent)
        {
            var unwatered = statusEvent.GrowthStatus == PlantGrowthStatus.NotWatered || statusEvent.GrowthStatus == PlantGrowthStatus.GrownButNotWatered;
            var grown = statusEvent.GrowthStatus == PlantGrowthStatus.GrownAndReadyToNonPermaHarvest || statusEvent.GrowthStatus == PlantGrowthStatus.GrownAndReadyToPermaHarvest;
            
            NeedsWaterIcon.SetActive(unwatered);
            DoneGrowingIcon?.SetActive(grown);
            
            CropBottom.sharedMaterial = unwatered ? UnwateredDirt : WateredDirt;
        }

        private void Update()
        {
            // right now visuals are handled by animators that may need to be adjusted for time
            // this is also not hooked up to faster timecontroller values
            if (GrowthStatus == PlantGrowthStatus.NotWatered || GrowthStatus == PlantGrowthStatus.GrownButNotWatered)
            {
                Animator.speed = 0;
                // wait for water. show icon
            }
            else if (GrowthStatus == PlantGrowthStatus.Growing || GrowthStatus == PlantGrowthStatus.GrownButGrowingAgain)
            {
                GrowthPercent = Mathf.Clamp01(GrowthPercent + (Time.deltaTime * Time.timeScale / GrowTime / 60f));
                if (GrowthPercent >= 1)
                {
                    Debug.Log("Plant finished growing");
                    //TODO: properly handle trees 
                    GrowthStatus = IsTree ? PlantGrowthStatus.GrownAndReadyToNonPermaHarvest : PlantGrowthStatus.GrownAndReadyToPermaHarvest;
                    
                    //OnPlantFinishedGrowing?.Invoke();
                }
            }
        }
        
        // water the plant. used by whatever watering can
        public void Water()
        {
            var animationClipSpeed = (120) / 60;
            var animationSpeed = (animationClipSpeed / GrowTime) * Time.timeScale;

            if (GrowthStatus == PlantGrowthStatus.NotWatered)
            {
                Debug.Log("Plant started growing");

                Animator.speed = animationSpeed;
                GrowthStatus = PlantGrowthStatus.Growing;
            }
            else if (GrowthStatus == PlantGrowthStatus.GrownButNotWatered)
            {
                Debug.Log("Plant started growing again");
                
                Animator.speed = animationSpeed;
                GrowthStatus = PlantGrowthStatus.GrownButGrowingAgain;
                //_cachedGrowthPropertyID
            }
        }

        public void NonPermaHarvest()
        {
            GrowthPercent = 0;
            
            Animator.SetBool(_cachedGrowthPropertyID, true);
            Animator.SetBool(_cachedGrowthPropertyID, false);
            Animator.SetBool(_cachedGrowthPropertyID, true);
            
            Debug.Log("Player harvested tree. begin the regrowth.");
            GrowthStatus = PlantGrowthStatus.GrownButNotWatered;
        }
    }
}

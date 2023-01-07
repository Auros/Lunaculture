using System.Collections.Generic;
using UnityEngine;

namespace Lunaculture.Placement
{
    // checker? i hardly know her!
    public class PlaceableObjectOverlapChecker : MonoBehaviour
    {
        private int _layer;
        private readonly List<Collider> _collidingWith = new(4);

        public bool IsOverlapping() => isActiveAndEnabled && _collidingWith.Count != 0;

        private void Start()
        {
            _layer = LayerMask.NameToLayer("Physical Object");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != _layer)
                return;

            _collidingWith.Add(other);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer != _layer)
                return;
            
            _collidingWith.Remove(other);
        }
    }
}
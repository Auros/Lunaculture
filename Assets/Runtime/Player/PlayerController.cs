using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Lunaculture.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float movementSpeed;
        [SerializeField, FormerlySerializedAs("rigidbody")] private Rigidbody _rigidbody = null!;

        private Vector3 intendedVelocity = Vector3.zero;

        public void OnMovement(InputAction.CallbackContext context)
        {
            var velocity2d = context.ReadValue<Vector2>();

            var normalizedVelocity = velocity2d.normalized;

            intendedVelocity = movementSpeed * new Vector3(normalizedVelocity.x, 0, normalizedVelocity.y);
        }

        private void FixedUpdate()
        {
            _rigidbody.velocity = intendedVelocity;
        }

        // TODO(Caeden): Perhaps rethink how to lock player rotation when we add a model
        private void LateUpdate()
        {
            transform.rotation = Quaternion.identity;
        }
    }
}

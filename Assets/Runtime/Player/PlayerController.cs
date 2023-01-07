using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Lunaculture.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float movementSpeed;
        [SerializeField] [FormerlySerializedAs("rigidbody")] private Rigidbody _rigidbody = null!;
        [SerializeField] private CapsuleCollider outsideCollider;

        private Vector3 intendedVelocity = Vector3.zero;

        private bool fixedUpdateMovement = false;

        public void OnMovement(InputAction.CallbackContext context)
        {
            var velocity2d = context.ReadValue<Vector2>();

            var normalizedVelocity = velocity2d.normalized;

            intendedVelocity = movementSpeed * new Vector3(normalizedVelocity.x, 0, normalizedVelocity.y);
        }

        private void Update()
        {
            if (!fixedUpdateMovement) UpdateMovement(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            if (fixedUpdateMovement) UpdateMovement(Time.fixedDeltaTime);
        }

        private void UpdateMovement(float deltaTime)
        {
            transform.position += deltaTime * intendedVelocity;
            outsideCollider.radius = (deltaTime / Time.fixedDeltaTime) + 0.5f;
            _rigidbody.velocity = Vector3.zero;
        }

        // TODO(Caeden): Perhaps rethink how to lock player rotation when we add a model
        private void LateUpdate()
        {
            transform.rotation = Quaternion.identity;
        }

        /* Swapping between FixedUpdate and Update for movement gives us the best of two worlds:
         *   - Moving on FixedUpdate causes jitter in general movement, because the framerate is almost always faster than the physics rate
         *   - Moving on Update causes jitter on collisions, because the player is almost always able to move inside objects before the Physics system corrects that
         */
        private void OnTriggerEnter(Collider other) => fixedUpdateMovement = true;

        private void OnTriggerStay(Collider other) => fixedUpdateMovement = true;

        private void OnTriggerExit(Collider other) => fixedUpdateMovement = false;
    }
}

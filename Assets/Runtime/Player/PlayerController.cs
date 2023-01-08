using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Lunaculture.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float movementSpeed;
        
        [SerializeField]
        [FormerlySerializedAs("rigidbody")]
        private Rigidbody _rigidbody = null!;
        
        [SerializeField] private CapsuleCollider outsideCollider = null!;

        private Vector3 intendedVelocity = Vector3.zero;

        [SerializeField]
        private Animator _animator = null!;

        [SerializeField]
        private Transform RotatedObject = null!;

        private bool walking = false;

        private int _cachedWalkingPropertyId = 0;

        public void OnMovement(InputAction.CallbackContext context)
        {
            _ = outsideCollider;
            var velocity2d = context.ReadValue<Vector2>();

            var normalizedVelocity = velocity2d.normalized;

            intendedVelocity = movementSpeed * new Vector3(normalizedVelocity.x, 0, normalizedVelocity.y);

            if (intendedVelocity != Vector3.zero)
            {
                _animator.SetBool("Walking", true);
            }
        }

        private void FixedUpdate()
        {
            _rigidbody.velocity = intendedVelocity;

            var currentlyWalking = _rigidbody.velocity != Vector3.zero;
            if (currentlyWalking != walking)
            {
                walking = currentlyWalking;
                _animator.SetBool(_cachedWalkingPropertyId, walking);
            }

            if (_rigidbody.velocity.magnitude > .01)
            {
                RotatedObject.rotation = Quaternion.LookRotation(_rigidbody.velocity, Vector3.up);
            }
        }

        // TODO(Caeden): Perhaps rethink how to lock player rotation when we add a model
        private void LateUpdate()
        {
            transform.rotation = Quaternion.identity;
        }

        private void Start()
        {
            _cachedWalkingPropertyId = Animator.StringToHash("Walking");
        }
    }
}

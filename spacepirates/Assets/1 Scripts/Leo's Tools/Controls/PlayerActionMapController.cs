using System.Collections.Generic;
using _1_Scripts.Leo_s_Tools.GameManagers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _1_Scripts.Leo_s_Tools.Controls
{
    public class PlayerActionMapController : MonoBehaviour
    {
        private Rigidbody2D _rigidbody2D;
        private Vector2 _movement;

        private bool _isGrounded = false;
        private bool _isClimbing = false;
        
        [SerializeField] private float _movementSpeed = 20f;
        [SerializeField] private float _jumpSpeed = 10f;
        [SerializeField] private float _gravityScale = 1.7f;

        public Animator _animator;

        bool left;

        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            // Ensure the instance is available and inputActions is initialized
            if (InputMapEnabler.Instance != null && InputMapEnabler.Instance.inputActions != null)
            {
                // Subscribe to input events
                InputMapEnabler.Instance.inputActions.Player.Move.performed += OnMovePerformed;
                InputMapEnabler.Instance.inputActions.Player.Move.canceled += OnMoveCanceled;
                InputMapEnabler.Instance.inputActions.Player.Interact.performed += OnInteractPerformed;
                
                InputMapEnabler.Instance.inputActions.Player.Jump.performed += OnJumpPerformed;

                InputMapEnabler.Instance.inputActions.Player.Crouch.performed += OnCrouchPerformed;
                // InputMapEnabler.Instance.inputActions.Player.Jump.canceled += OnJumpCanceled;
            }
            else
            {
                Debug.LogError("InputMapEnabler or inputActions is not properly initialized!");
            }
        }

        private void FixedUpdate()
        {
            if(_rigidbody2D.linearVelocity.x < -0.25f && !left)
            {
                left = true;
                Flip();   
            }
            else if(_rigidbody2D.linearVelocity.x > 0.25f && left)
            {
                left = false;
                Flip();
            }
            _animator.SetFloat("Speed", Mathf.Abs(_rigidbody2D.linearVelocity.x));
            var force = _movement * _movementSpeed * Time.fixedDeltaTime;
            
            if (!_isClimbing)
            {
                force *= Vector2.right;
            }
            
            _rigidbody2D.AddForce(force, ForceMode2D.Impulse);
            
            if (_rigidbody2D.linearVelocity.magnitude > _movementSpeed)
            {
                _rigidbody2D.linearVelocity = _rigidbody2D.linearVelocity.normalized * _movementSpeed;
            }
            if (!_isGrounded)
            {
                _animator.SetBool("Aerial", true);
            }
        }

        private void OnCrouchPerformed(InputAction.CallbackContext context)
        {
            // TODO make this interact with the wall
        }

        private void OnInteractPerformed(InputAction.CallbackContext context)
        {
            EventManager.TriggerEvent("OnBubbleCast", new Dictionary<string, object>
            {
                { "PlayerPosition", transform},
                { "PlayerDirection", _rigidbody2D}
            } );
        }

        private void OnMovePerformed(InputAction.CallbackContext context)
        {
            _movement = context.ReadValue<Vector2>().normalized;
        }

        private void OnMoveCanceled(InputAction.CallbackContext context)
        {
            _movement = Vector2.zero;
        }

        private void OnJumpPerformed(InputAction.CallbackContext context)
        {
            /*if (!_isGrounded) return;
            var a = Vector2.up * _jumpSpeed;
            _rigidbody2D.AddForce(a, ForceMode2D.Impulse);
            _isGrounded = false;*/
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Platform"))
            {
                _animator.SetBool("Aerial", false);
                _isGrounded = true;
                EventManager.TriggerEvent("OnPlayerGrounded", null);
            }
        }
        private void OnCollisionStay2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Platform"))
            {
                _animator.SetBool("Aerial", false);
                _isGrounded = true;
                EventManager.TriggerEvent("OnPlayerGrounded", null);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("ClimbableWall"))
            {
                Debug.Log("Climbable Wall");
                _rigidbody2D.gravityScale = 0;
                _isClimbing = true;
                EventManager.TriggerEvent("OnPlayerClimbing", null);
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Platform"))
            {
                _isGrounded = false;
                EventManager.TriggerEvent("OnPlayerNoLongerGrounded", null);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("ClimbableWall"))
            {
                _isClimbing = false;
                _rigidbody2D.gravityScale = _gravityScale;
                EventManager.TriggerEvent("OnPlayerClimbing", null);
            }
            
        }

        void Flip()
        {
            this.transform.Rotate(0f, 180f, 0f);
        }

        // void OnJumpCanceled(InputAction.CallbackContext context)
        // {
            // _rigidbody2D.AddForce(Vector2.up * _jumpSpeed, ForceMode2D.Impulse);
            // Debug.Log("Jump performed");
        // }
    }
}
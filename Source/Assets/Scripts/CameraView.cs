using System;
using gRaFFit.Agar.Controllers.InputSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace gRaFFit.Agar.Views.CameraControls {
    /// <summary>
    /// Скрипт игрока
    /// </summary>
    public class CameraView : MonoBehaviour {

        #region MonoSingleton

        private CameraView() {

        }

        private void Awake() {
            if (Instance != null && Instance != this) {
                Destroy(Instance);
            }

            _cachedCameraZPosition = transform.position.z;

            _startCameraOrtho = _camera.orthographicSize;
            _startCameraPos = _camera.transform.position;
            
            Instance = this;
        }

        public static CameraView Instance { get; private set; }

        #endregion

#pragma warning disable 649

        private float _startCameraOrtho;
        private Vector3 _startCameraPos;
        
        [SerializeField] private float _cameraSpeed;
        [SerializeField] private float _cameraOffsetMultiplier;
        [SerializeField] private Camera _camera;
        [SerializeField] private float _basicOrtho;
        [SerializeField] private float _weightOrthoCost;

#pragma warning restore 649

        private PlayerView _player;

        private float _cachedCameraZPosition;

        private float _targetOrtho;

        public void ResetCamera() {
            transform.position = _startCameraPos;
            _camera.orthographicSize = _targetOrtho = _startCameraOrtho;
        }

        public void Update() {
            if (_player == null) return;

            var cameraTargetPosition =
                new Vector3(_player.transform.position.x, _player.transform.position.y, _cachedCameraZPosition);

            if (InputController.Instance.IsTouch() && !EventSystem.current.IsPointerOverGameObject()) {
                cameraTargetPosition += (Vector3) _player.GetTouchNormalizedOffset() * _cameraOffsetMultiplier;
            }

            SetCameraPosition(Vector3.Lerp(transform.position, cameraTargetPosition, _cameraSpeed * Time.deltaTime));
            _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, _targetOrtho, Time.deltaTime);

            CollideCamera();
        }


        public void SetToPlayer(PlayerView playerTransform) {
            _player = playerTransform;
            SetCameraPosition(_player.transform.position);
        }

        private void SetCameraPosition(Vector3 position) {
            transform.position = new Vector3(position.x, position.y, _cachedCameraZPosition);
        }

        public void SetTargetOrthoAccordingWithWeight(float weight) {
            _targetOrtho = _basicOrtho + weight * _weightOrthoCost;
        }


        private float _currentCollideRotate;

        [SerializeField] private float _cameraCollideRotateSpeed;

        [SerializeField] private float _afterCollideCameraFixSpeed;
        
        [SerializeField] private float _collideCameraRotateMultiplier;

        /// <summary>
        /// Эффект удара камеры с её выворачиванием по оси Z
        /// </summary>
        private void CollideCamera() {
            transform.rotation = Quaternion.Lerp(transform.rotation,
                Quaternion.Euler(0, 0, _currentCollideRotate), _cameraCollideRotateSpeed);

            _currentCollideRotate = Mathf.Lerp(_currentCollideRotate, 0, _afterCollideCameraFixSpeed);
        }


        /// <summary>
        /// Отклик на столкновение игрока с препятствием
        /// </summary>
        /// <param name="collideForce">Сила столкновения</param>
        public void DoCollide(float collideForce) {
            collideForce *= _collideCameraRotateMultiplier;

            if (Mathf.Abs(_currentCollideRotate) >= collideForce * 0.5f)
                return;

            _currentCollideRotate = Random.Range(0, 100) < 50
                ? collideForce
                : -collideForce;
        }
    }
}
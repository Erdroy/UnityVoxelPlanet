﻿
using System.Diagnostics;
using MyHalp;
using UnityEngine;

namespace UnityVoxelPlanet
{
    /// <summary>
    /// CameraController class.
    /// Implements simple camera which can orbit the planet.
    /// </summary>
    public class CameraController : MyComponent
    {
        public float Sensitivity = 2.5f;

        public float InitialAltitude = 5000.0f;

        public AnimationCurve SpeedCurve;

        public Transform Camera;

        public float MinimumX = -360F;
        public float MaximumX = 360F;

        public float MinimumY = -60F;
        public float MaximumY = 60F;

        private float _rotationX;
        private float _rotationY;

        private float _dtSum;
        private int _frames;
        private float _fps;

        private float _maxDt;
        private float _currMaxDt;

        private Quaternion _initialQ;
        
        public CameraController()
        {
            _rotationY = 0F;
        }

        // override `OnInit`
        protected override void OnInit()
        {
            // set current camera instance
            Current = this;

            _initialQ = Camera.localRotation;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            Application.targetFrameRate = int.MaxValue;
            QualitySettings.vSyncCount = 0;
        }

        // override `OnTick`
        protected override void OnTick()
        {
            UpdateMovement();
            UpdateLook();
            
            if (Time.deltaTime > _maxDt)
                _maxDt = Time.deltaTime;

            if (_dtSum >= 1.0f)
            {
                _fps = _frames;
                _currMaxDt = _maxDt;

                _maxDt = 0;
                _frames = 0;
                _dtSum = 0.0f;
            }

            _dtSum += Time.deltaTime;
            _frames++;
        }

        // private
        private void UpdateLook()
        {
            // Read the mouse input axis
            _rotationX += Input.GetAxis("Mouse X") * Sensitivity;
            _rotationY += Input.GetAxis("Mouse Y") * Sensitivity;

            var xQuaternion = Quaternion.AngleAxis(_rotationX, Vector3.up);
            var yQuaternion = Quaternion.AngleAxis(_rotationY, -Vector3.right);

            Camera.localRotation = _initialQ * xQuaternion * yQuaternion;
        }

        // private
        private void UpdateMovement()
        {  
            // go to desired altitude when camera is too low
            if (VoxelManager.Instance && GetAltitude() <= float.Epsilon)
                ToAltitude(InitialAltitude);

            var axisH = Input.GetAxisRaw("Horizontal");
            var axisV = Input.GetAxisRaw("Vertical");

            transform.up = GetNormal();

            var dir = Vector3.zero;

            dir += Camera.forward * axisV;
            dir += Camera.right * axisH;

            if (Input.GetKey(KeyCode.Space))
                dir += GetNormal();

            if (Input.GetKey(KeyCode.LeftControl))
                dir -= GetNormal();

            dir.Normalize();

            var speed = SpeedCurve.Evaluate(GetAltitude()) * 10.0f;

            if (Input.GetKey(KeyCode.LeftShift))
                speed *= 2;

            transform.position += dir * speed * Time.deltaTime;
        }

        private void OnGUI()
        {
            var mem = Process.GetCurrentProcess().WorkingSet64;

            GUILayout.BeginVertical("box");
            GUILayout.Label("-- info --");
            GUILayout.Label("FPS: " + _fps + " max: " + _currMaxDt * 1000.0f + "ms");
            GUILayout.Label("Altitude: " + GetAltitude().ToString("f1"));
            GUILayout.Label("Planet radius: " + GetOrbitingPlanet().Radius);
            GUILayout.Label("RAM usage: " + mem);
            GUILayout.EndVertical();
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying)
                return;

            Gizmos.DrawLine(GetPosition(), GetOrbitingPlanet().Position);
        }

        public VoxelPlanet GetOrbitingPlanet()
        {
            // TODO: implement planet finding (check which is closest - including the gravity affect, if there is none, use world up axis)

            return VoxelManager.Instance.VoxelPlanets[0];
        }

        public Vector3 GetNormal()
        {
            var result = GetPosition() - GetOrbitingPlanet().Position;
            result.Normalize();
            return result;
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public float GetAltitude()
        {
            var planet = GetOrbitingPlanet();
            var distanceToNucleus = Vector3.Distance(planet.Position, GetPosition());

            var altitude = distanceToNucleus - planet.Radius;

            if (altitude < 0)
                altitude = 0;

            return altitude;
        }

        public void ToAltitude(float altitude)
        {
            var planet = GetOrbitingPlanet();
            var normal = GetNormal();
            transform.position = planet.Position + normal * (planet.Radius + altitude);
        }
        
        public static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360F)
                angle += 360F;
            if (angle > 360F)
                angle -= 360F;
            return Mathf.Clamp(angle, min, max);
        }

        public static CameraController Current { get; private set; }
    }
}

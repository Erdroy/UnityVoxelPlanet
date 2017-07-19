
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

        public float InitialAltitude = 1000.0f;

        public AnimationCurve SpeedCurve;

        // override `OnInit`
        protected override void OnInit()
        {
            // set current camera instance
            Current = this;
        }

        // override `OnTick`
        protected override void OnTick()
        {
            // go to desired altitude when camera is too low
            if (VoxelManager.Instance && GetAltitude() <= float.Epsilon)
                ToAltitude(InitialAltitude);
            
            transform.up = GetNormal();

            var axisH = Input.GetAxisRaw("Horizontal");
            var axisV = Input.GetAxisRaw("Vertical");

            var dir = Vector3.zero;

            dir += transform.forward * axisV;
            dir += transform.right * axisH;
            
            if (Input.GetKey(KeyCode.Space))
                dir += transform.up;

            if (Input.GetKey(KeyCode.LeftControl))
                dir -= transform.up;

            dir.Normalize();

            var speed = SpeedCurve.Evaluate(GetAltitude());

            if (Input.GetKey(KeyCode.LeftShift))
                speed *= 2;

            transform.position += dir * speed * Time.deltaTime;
        }

        private void OnGUI()
        {
            GUILayout.Label("Altitude: " + GetAltitude().ToString("f1"));
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying)
                return;

            Gizmos.DrawLine(GetPosition(), GetOrbitingPlanet().Position);
        }

        public VoxelPlanet GetOrbitingPlanet()
        {
            // TODO: implement planet finding

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

        public static CameraController Current { get; private set; }
    }
}

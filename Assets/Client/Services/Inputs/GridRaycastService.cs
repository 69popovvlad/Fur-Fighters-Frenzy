using Core.Application;
using UnityEngine;

namespace Client.Services.Inputs
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class GridRaycastService: IApplicationResource
    {
        private Plane _plane;
        private readonly Camera _camera;

        public GridRaycastService()
        {
            _camera = Camera.main;
            _plane = new Plane(Vector3.up, 0);
            
            Input.simulateMouseWithTouches = true;
        }

        public bool TryGetWorldTouchPosition(out Vector3 worldPosition)
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (_plane.Raycast(ray, out var distance))
            {
                worldPosition = ray.GetPoint(distance);
                return true;
            }

            worldPosition = default;
            return false;
        }
    }
}
using UnityEngine;

namespace Core.Utilities
{
    public static class TransformUtilities
    {
        public static BoundingSphere GetBoundingSphere(this Transform transform)
        {
            var bounds = transform.GetBounds();
            if (bounds.size == Vector3.zero)
            {
                return new BoundingSphere(transform.position, 0.5f);
            }

            return new BoundingSphere(bounds.center, (bounds.max - bounds.center).magnitude);
        }
        
        public static Bounds GetBounds(this Transform transform)
        {
            var bounds = transform.CalculateBounds();
            if (bounds.size == Vector3.zero)
            {
                return new Bounds(transform.position, Vector3.one);
            }

            return bounds;
        }
        
        public static Bounds GetRectWorldBounds(this RectTransform rectTransform, Vector3 boundsOffset) {
            var min = Vector3.positiveInfinity;
            var max = Vector3.negativeInfinity;

            var vertices = new Vector3[4];
            rectTransform.GetWorldCorners(vertices);

            foreach (var vector3 in vertices) {
                min = Vector3.Min(min, vector3);
                max = Vector3.Max(max, vector3);
            }

            var bounds = new Bounds();
            bounds.SetMinMax(min - boundsOffset, max + boundsOffset);
            
            return bounds;
        }
        
        public static bool IsInnerBound(this Vector3 position, Bounds bounds) {
            if (bounds.min.x < position.x || position.x < bounds.max.x
                && bounds.min.y < position.y || position.y < bounds.max.y) {
                return true;
            }

            return false;
        }
        
        public static bool IsInnerBound(this Bounds innerBounds, Bounds bounds) {
            if (bounds.min.x < innerBounds.min.x
                && bounds.min.y < innerBounds.min.y
                && bounds.max.x > innerBounds.max.x
                && bounds.max.y > innerBounds.max.y) {
                return true;
            }

            return false;
        }

        private static Bounds CalculateBounds(this Transform transform)
        {
            var renderer = transform.GetComponent<Renderer>();
            return renderer == null ? default : renderer.bounds;
        }
    }
}
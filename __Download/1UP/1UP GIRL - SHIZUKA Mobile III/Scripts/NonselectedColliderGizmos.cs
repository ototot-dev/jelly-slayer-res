#if UNITY_EDITOR

using UnityEngine;

namespace G1UP
{
    public class NonselectedColliderGizmos : MonoBehaviour
    {
        public static bool DrawingDisabled;
        public static Color WireframeColor;

        public BoneColliderGroup[] colliders;



        void OnDrawGizmos()
        {
            if (DrawingDisabled) return;


            foreach (var c in colliders)
            {
                if (null == c) continue;

                Gizmos.color = WireframeColor;
                Matrix4x4 mat = c.transform.localToWorldMatrix;
                Gizmos.matrix = mat * Matrix4x4.Scale(new Vector3(
                    1.0f / c.transform.lossyScale.x,
                    1.0f / c.transform.lossyScale.y,
                    1.0f / c.transform.lossyScale.z
                    ));
                foreach (var y in c.Colliders)
                {
                    Gizmos.DrawWireSphere(y.Offset, y.Radius);
                }

            }
        }




    }
}
#endif

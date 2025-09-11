using UnityEngine;

public class StickyPlatforms : MonoBehaviour
{
    public static GameObject leftplat = null;
    public static GameObject rightplat = null;

    private struct HandData
    {
        public Vector3 Position;
        public Quaternion Rotation;
    }

    public static void StickyPlatforms1()
    {
        HandleHand( ref leftplat, ControllerInputPoller.instance.leftGrab, GorillaTagger.Instance.leftHandTransform, GorillaLocomotion.GTPlayer.Instance.leftHandOffset, GorillaLocomotion.GTPlayer.Instance.leftHandRotOffset);
        HandleHand(ref rightplat, ControllerInputPoller.instance.rightGrab, GorillaTagger.Instance.rightHandTransform, GorillaLocomotion.GTPlayer.Instance.rightHandOffset, GorillaLocomotion.GTPlayer.Instance.rightHandRotOffset);
    }

    private static void HandleHand(ref GameObject platform, bool isGrabbing, Transform handTransform, Vector3 positionOffset, Quaternion rotationOffset)
    {
        if (isGrabbing)
        {
            if (platform == null)
            {
                platform = CreatePlatform();
                HandData handData = GetTrueHandData(handTransform, positionOffset, rotationOffset);
                platform.transform.position = handData.Position;
                platform.transform.rotation = handData.Rotation;
                platform.transform.SetParent(handTransform, true);
            }
        }
        else
        {
            if (platform != null)
            {
                platform.transform.SetParent(null);
                Object.Destroy(platform);
                platform = null;
            }
        }
    }

    private static HandData GetTrueHandData(Transform handTransform, Vector3 positionOffset, Quaternion rotationOffset)
    {
        Quaternion calculatedRotation = handTransform.rotation * rotationOffset;
        Vector3 calculatedPosition = handTransform.position + (handTransform.rotation * positionOffset);
        return new HandData
        {
            Position = calculatedPosition,
            Rotation = calculatedRotation
        };
    }

    private static GameObject CreatePlatform()
    {
        GameObject platform = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        platform.transform.localScale = new Vector3(0.3f, 0.05f, 0.3f);

        Renderer platformRenderer = platform.GetComponent<Renderer>();
        if (platformRenderer != null)
        {
            platformRenderer.material.color = Color.black;
        }

        Rigidbody rb = platform.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        FixStickyColliders(platform);
        return platform;
    }

    public static void FixStickyColliders(GameObject platform)
    {
        float platformScale = 0.2f;
        float colliderThickness = 0.025f;
        float platformHalfSize = platformScale / 2f;
        float localThickness = colliderThickness / platformScale;
        SphereCollider defaultCollider = platform.GetComponent<SphereCollider>();
        if (defaultCollider != null)
        {
            Object.Destroy(defaultCollider);
        }
        BoxCollider mainCollider = platform.AddComponent<BoxCollider>();
        mainCollider.size = new Vector3(1f, 0.1f, 1f);
        mainCollider.isTrigger = false;
        CreateSideColliders(platform, platformHalfSize, localThickness);
        CreateCornerColliders(platform, platformHalfSize, localThickness);
        CreateEdgeColliders(platform, platformHalfSize, localThickness);
        AddOverarchingSphereCollider(platform);
    }

    private static void AddOverarchingSphereCollider(GameObject platform)
    {
        SphereCollider overarchingCollider = platform.AddComponent<SphereCollider>();
        overarchingCollider.radius = 0.75f;
        overarchingCollider.isTrigger = false;
    }

    private static void CreateSideColliders(GameObject platform, float platformHalfSize, float localThickness)
    {
        Vector3[] localPositions = new Vector3[]
        {
            new Vector3(0, platformHalfSize + localThickness/2, 0),
            new Vector3(0, -platformHalfSize - localThickness/2, 0),
            new Vector3(platformHalfSize + localThickness/2, 0, 0),
            new Vector3(-platformHalfSize - localThickness/2, 0, 0),
            new Vector3(0, 0, platformHalfSize + localThickness/2),
            new Vector3(0, 0, -platformHalfSize - localThickness/2)
        };

        Vector3[] localScales = new Vector3[]
        {
            new Vector3(1f, localThickness, 1f),
            new Vector3(1f, localThickness, 1f),
            new Vector3(localThickness, 1f, 1f),
            new Vector3(localThickness, 1f, 1f),
            new Vector3(1f, 1f, localThickness),
            new Vector3(1f, 1f, localThickness)
        };

        for (int i = 0; i < localPositions.Length; i++)
        {
            GameObject side = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Object.Destroy(side.GetComponent<Renderer>());
            side.transform.SetParent(platform.transform, false);
            side.transform.localPosition = localPositions[i];
            side.transform.localScale = localScales[i];
        }
    }

    private static void CreateCornerColliders(GameObject platform, float platformHalfSize, float localThickness)
    {
        float cornerOffset = platformHalfSize + (localThickness / 2);
        Vector3[] cornerPositions = new Vector3[]
        {
            new Vector3(cornerOffset, cornerOffset, cornerOffset), new Vector3(-cornerOffset, cornerOffset, cornerOffset),
            new Vector3(cornerOffset, -cornerOffset, cornerOffset), new Vector3(-cornerOffset, -cornerOffset, cornerOffset),
            new Vector3(cornerOffset, cornerOffset, -cornerOffset), new Vector3(-cornerOffset, cornerOffset, -cornerOffset),
            new Vector3(cornerOffset, -cornerOffset, -cornerOffset), new Vector3(-cornerOffset, -cornerOffset, -cornerOffset)
        };

        foreach (Vector3 position in cornerPositions)
        {
            GameObject corner = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            Object.Destroy(corner.GetComponent<Renderer>());
            corner.transform.SetParent(platform.transform, false);
            corner.transform.localPosition = position;
            corner.transform.localScale = Vector3.one * localThickness;
        }
    }

    private static void CreateEdgeColliders(GameObject platform, float platformHalfSize, float localThickness)
    {
        Vector3[] edgeDirections = new Vector3[]
        {
        Vector3.right,
        Vector3.up,
        Vector3.forward
        };
        foreach (Vector3 direction in edgeDirections)
        {
            for (int i = -1; i <= 1; i += 2)
            {
                for (int j = -1; j <= 1; j += 2)
                {
                    Vector3 edgeCenter = (platformHalfSize + localThickness / 2) * (direction + i * Vector3.Cross(direction, Vector3.up) + j * Vector3.Cross(direction, Vector3.right)).normalized;
                    GameObject edge = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                    edge.transform.SetParent(platform.transform);
                    edge.transform.localPosition = edgeCenter;
                    edge.transform.localRotation = Quaternion.LookRotation(direction);
                    edge.transform.localScale = new Vector3(localThickness, platformHalfSize, localThickness);
                    edge.GetComponent<Renderer>().enabled = false;
                }
            }
        }
    }
}
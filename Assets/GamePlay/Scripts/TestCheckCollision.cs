using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCheckCollision : MonoBehaviour
{
    [SerializeField] private BoxCollider box1;
    [SerializeField] private BoxCollider box2;

    [ContextMenu("check")]
    public void CheckCollide()
    {
        Box box_1 = new Box(transform.TransformPoint(box1.center) + box1.transform.position , box1.size,
            ConvertDegreeToRadian(box1.transform.eulerAngles));
        Box box_2 = new Box(transform.TransformPoint(box2.center) + box2.transform.position , box2.size,
            ConvertDegreeToRadian(box2.transform.eulerAngles));
        Debug.Log(box1.transform.eulerAngles + " " + box2.transform.eulerAngles);
        Debug.Log($"Box1 center {box_1.centerPosition}  size {box_1.size} rotation {box_1.rotation}");
        Debug.Log($"Box2 center {box_2.centerPosition}  size {box_2.size} rotation {box_2.rotation}");
        Debug.Log("Is collide method simplify" + CollisionDetection1.BoxesCollide(box_1, box_2));
        Debug.Log("Is collide method complex" + CollisionDetection.BoxesCollide(box_1, box_2));
    }

    public Vector3 ConvertDegreeToRadian(Vector3 angle)
    {
        return new Vector3(Mathf.Deg2Rad*angle.x, Mathf.Deg2Rad*angle.y, Mathf.Deg2Rad*angle.z);
    }
}


public class Box
{
    public Vector3 centerPosition;
    public Vector3 size;
    public Vector3 rotation; // Euler angles in radians

    public Box(Vector3 centerPosition, Vector3 size, Vector3 rotation)
    {
        this.centerPosition = centerPosition;
        this.size = size;
        this.rotation = rotation;
    }

    public Matrix4x4 GetRotationMatrix()
    {
        return Matrix4x4.Rotate(Quaternion.Euler(rotation * Mathf.Rad2Deg));
    }

    public Vector3[] GetVertices()
    {
        Matrix4x4 rotationMatrix = GetRotationMatrix();
        Vector3 halfSize = size / 2;

        Vector3[] vertices = new Vector3[8];
        vertices[0] = centerPosition + rotationMatrix.MultiplyPoint3x4(new Vector3(-halfSize.x, -halfSize.y, -halfSize.z));
        vertices[1] = centerPosition + rotationMatrix.MultiplyPoint3x4(new Vector3(halfSize.x, -halfSize.y, -halfSize.z));
        vertices[2] = centerPosition + rotationMatrix.MultiplyPoint3x4(new Vector3(halfSize.x, -halfSize.y, halfSize.z));
        vertices[3] = centerPosition + rotationMatrix.MultiplyPoint3x4(new Vector3(-halfSize.x, -halfSize.y, halfSize.z));
        vertices[4] = centerPosition + rotationMatrix.MultiplyPoint3x4(new Vector3(-halfSize.x, halfSize.y, -halfSize.z));
        vertices[5] = centerPosition + rotationMatrix.MultiplyPoint3x4(new Vector3(halfSize.x, halfSize.y, -halfSize.z));
        vertices[6] = centerPosition + rotationMatrix.MultiplyPoint3x4(new Vector3(halfSize.x, halfSize.y, halfSize.z));
        vertices[7] = centerPosition + rotationMatrix.MultiplyPoint3x4(new Vector3(-halfSize.x, halfSize.y, halfSize.z));

        return vertices;
    }

    public Vector3[] GetEdges()
    {
        Vector3[] vertices = GetVertices();

        return new Vector3[]
        {
            vertices[1] - vertices[0],
            vertices[2] - vertices[1],
            vertices[3] - vertices[2],
            vertices[0] - vertices[3],
            vertices[5] - vertices[4],
            vertices[6] - vertices[5],
            vertices[7] - vertices[6],
            vertices[4] - vertices[7],
            vertices[0] - vertices[4],
            vertices[1] - vertices[5],
            vertices[2] - vertices[6],
            vertices[3] - vertices[7],
        };
    }
}

public class CollisionDetection
{
    public static bool BoxesCollide(Box box1, Box box2)
    {
        Vector3[] vertices1 = box1.GetVertices();
        Vector3[] vertices2 = box2.GetVertices();

        Vector3[] edges1 = box1.GetEdges();
        Vector3[] edges2 = box2.GetEdges();

        List<Vector3> axes = new List<Vector3>();

        // Add axes from the faces of the boxes
        axes.Add(box1.GetRotationMatrix().MultiplyVector(Vector3.right));
        axes.Add(box1.GetRotationMatrix().MultiplyVector(Vector3.up));
        axes.Add(box1.GetRotationMatrix().MultiplyVector(Vector3.forward));
        axes.Add(box2.GetRotationMatrix().MultiplyVector(Vector3.right));
        axes.Add(box2.GetRotationMatrix().MultiplyVector(Vector3.up));
        axes.Add(box2.GetRotationMatrix().MultiplyVector(Vector3.forward));

        // Add axes from the cross products of the edges
        foreach (var edge1 in edges1)
        {
            foreach (var edge2 in edges2)
            {
                Vector3 axis = Vector3.Cross(edge1, edge2);
                if (axis != Vector3.zero)
                {
                    axis.Normalize();
                    axes.Add(axis);
                }
            }
        }

        foreach (Vector3 axis in axes)
        {
            if (!OverlapOnAxis(vertices1, vertices2, axis))
            {
                return false;
            }
        }

        return true;
    }

    private static bool OverlapOnAxis(Vector3[] vertices1, Vector3[] vertices2, Vector3 axis)
    {
        float min1, max1, min2, max2;
        ProjectVertices(vertices1, axis, out min1, out max1);
        ProjectVertices(vertices2, axis, out min2, out max2);

        return !(min1 > max2 || min2 > max1);
    }

    private static void ProjectVertices(Vector3[] vertices, Vector3 axis, out float min, out float max)
    {
        min = Vector3.Dot(vertices[0], axis);
        max = min;
        for (int i = 1; i < vertices.Length; i++)
        {
            float projection = Vector3.Dot(vertices[i], axis);
            if (projection < min)
            {
                min = projection;
            }
            if (projection > max)
            {
                max = projection;
            }
        }
    }
}

public class CollisionDetection1
{
    public static bool BoxesCollide(Box box1, Box box2)
    {
        // Convert Euler angles to rotation matrices
        float[] rotationMatrix1 = EulerToRotationMatrix(box1.rotation);
        float[] rotationMatrix2 = EulerToRotationMatrix(box2.rotation);

        // Get half sizes of the boxes
        Vector3 halfSize1 = new Vector3(box1.size.x / 2, box1.size.y / 2, box1.size.z / 2);
        Vector3 halfSize2 = new Vector3(box2.size.x / 2, box2.size.y / 2, box2.size.z / 2);

        // Calculate centers of the boxes
        Vector3 center1 = box1.centerPosition;
        Vector3 center2 = box2.centerPosition;

        // Transform box2 into box1's local space
        Vector3 relativeCenter = new Vector3(center2.x - center1.x, center2.y - center1.y, center2.z - center1.z);
        Vector3 transformedCenter = new Vector3(
            rotationMatrix1[0] * relativeCenter.x + rotationMatrix1[1] * relativeCenter.y +
            rotationMatrix1[2] * relativeCenter.z,
            rotationMatrix1[3] * relativeCenter.x + rotationMatrix1[4] * relativeCenter.y +
            rotationMatrix1[5] * relativeCenter.z,
            rotationMatrix1[6] * relativeCenter.x + rotationMatrix1[7] * relativeCenter.y +
            rotationMatrix1[8] * relativeCenter.z);

        // Check for overlap on each axis
        if (!AxisOverlaps(transformedCenter.x, halfSize1.x, halfSize2.x) ||
            !AxisOverlaps(transformedCenter.y, halfSize1.y, halfSize2.y) ||
            !AxisOverlaps(transformedCenter.z, halfSize1.z, halfSize2.z))
        {
            return false; // No overlap found on at least one axis
        }

        // Transform box1 into box2's local space
        // Inverse of rotationMatrix1 for box1's rotation
        float[] invRotationMatrix1 = InverseRotationMatrix(rotationMatrix1);

        // Transform relative center of box1 using box2's rotation matrix
        Vector3 relativeCenter2 = new Vector3(center1.x - center2.x, center1.y - center2.y, center1.z - center2.z);
        Vector3 transformedCenter2 = new Vector3(
            invRotationMatrix1[0] * relativeCenter2.x + invRotationMatrix1[1] * relativeCenter2.y +
            invRotationMatrix1[2] * relativeCenter2.z,
            invRotationMatrix1[3] * relativeCenter2.x + invRotationMatrix1[4] * relativeCenter2.y +
            invRotationMatrix1[5] * relativeCenter2.z,
            invRotationMatrix1[6] * relativeCenter2.x + invRotationMatrix1[7] * relativeCenter2.y +
            invRotationMatrix1[8] * relativeCenter2.z);

        // Check for overlap on each axis
        if (!AxisOverlaps(transformedCenter2.x, halfSize2.x, halfSize1.x) ||
            !AxisOverlaps(transformedCenter2.y, halfSize2.y, halfSize1.y) ||
            !AxisOverlaps(transformedCenter2.z, halfSize2.z, halfSize1.z))
        {
            return false; // No overlap found on at least one axis
        }

        return true; // Overlap found on all axes, boxes collide
    }

    private static float[] EulerToRotationMatrix(Vector3 rotation)
    {
        float x = rotation.x;
        float y = rotation.y;
        float z = rotation.z;

        float cx = Mathf.Cos(x);
        float sx = Mathf.Sin(x);
        float cy = Mathf.Cos(y);
        float sy = Mathf.Sin(y);
        float cz = Mathf.Cos(z);
        float sz = Mathf.Sin(z);

        // Order of multiplication: Rx * Ry * Rz
        return new float[]
        {
            cy * cz, -cy * sz, sy,
            sx * sy * cz + cx * sz, -sx * sy * sz + cx * cz, -sx * cy,
            -cx * sy * cz + sx * sz, cx * sy * sz + sx * cz, cx * cy
        };
    }

    private static bool AxisOverlaps(float distance, float halfSize1, float halfSize2)
    {
        return Mathf.Abs(distance) <= halfSize1 + halfSize2;
    }

    private static float[] InverseRotationMatrix(float[] matrix)
    {
        // Inverse of 3x3 rotation matrix is its transpose (since rotation matrices are orthogonal)
        return new float[]
        {
            matrix[0], matrix[3], matrix[6],
            matrix[1], matrix[4], matrix[7],
            matrix[2], matrix[5], matrix[8]
        };
    }
}
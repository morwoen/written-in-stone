using UnityEngine;

public static class ExtensionMethods
{
    public static float Remap(this float value, float fromRangeStart, float fromRangeEnd, float toRangeStart, float toRangeEnd) {
        return toRangeStart + (value - fromRangeStart) * (toRangeEnd - toRangeStart) / (fromRangeEnd - fromRangeStart);
    }

    public static Vector3 RandomPointInCircle(this float maxDistance, float minimumDistance = 0, float y = 0) {
        return (maxDistance.RandomPointInCircle(minimumDistance)).ToV3(y);
    }

    public static Vector2 RandomPointInCircle(this float maxDistance, float minimumDistance = 0) {
        var randomPointInUnitCircle = (Random.insideUnitCircle * (maxDistance - minimumDistance));
        return randomPointInUnitCircle + (randomPointInUnitCircle / randomPointInUnitCircle.Abs()) * minimumDistance;
    }

    public static Vector3 ToV3(this Vector2 value, float y = 0) {
        return new Vector3(value.x, y, value.y);
    }

    public static Vector2 Abs(this Vector2 value) {
        return new Vector2(Mathf.Abs(value.x), Mathf.Abs(value.y));
    }
}

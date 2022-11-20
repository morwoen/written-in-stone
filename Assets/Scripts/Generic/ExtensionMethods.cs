public static class ExtensionMethods
{
    public static float Remap(this float value, float fromRangeStart, float fromRangeEnd, float toRangeStart, float toRangeEnd) {
        return toRangeStart + (value - fromRangeStart) * (toRangeEnd - toRangeStart) / (fromRangeEnd - fromRangeStart);
    }
}

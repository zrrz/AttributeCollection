public enum AttributeType
{
    //
    // Summary:
    //     Represents an array, list, struct or class.
    Generic = -1,
    //
    // Summary:
    //     Represents an integer property, for example int, byte, short, uint and long.
    Integer = 0,
    //
    // Summary:
    //     Represents a boolean property.
    Boolean = 1,
    //
    // Summary:
    //     Represents a single or double precision floating point property.
    Float = 2,
    //
    // Summary:
    //     Represents a string property.
    String = 3,
    //
    // Summary:
    //     Represents a color property.
    Color = 4,
    //
    // Summary:
    //     Provides a reference to an object that derives from UnityEngine.Object.
    ObjectReference = 5,
    //
    // Summary:
    //     Represents a LayerMask property.
    LayerMask = 6,
    //
    // Summary:
    //     Represents an enumeration property.
    Enum = 7,
    //
    // Summary:
    //     Represents a 2D vector property.
    Vector2 = 8,
    //
    // Summary:
    //     Represents a 3D vector property.
    Vector3 = 9,
    //
    // Summary:
    //     Represents a 4D vector property.
    Vector4 = 10,
    //
    // Summary:
    //     Represents a rectangle property.
    Rect = 11,
    //
    // Summary:
    //     Represents an array size property.
    ArraySize = 12,
    //
    // Summary:
    //     Represents a character property.
    Character = 13,
    //
    // Summary:
    //     Represents an AnimationCurve property.
    AnimationCurve = 14,
    //
    // Summary:
    //     Represents a bounds property.
    Bounds = 15,
    //
    // Summary:
    //     Represents a gradient property.
    Gradient = 16,
    //
    // Summary:
    //     Represents a quaternion property.
    Quaternion = 17,
    //
    // Summary:
    //     Provides a reference to another Object in the Scene.
    ExposedReference = 18,
    //
    // Summary:
    //     Represents a fixed buffer size property.
    FixedBufferSize = 19,
    //
    // Summary:
    //     Represents a 2D integer vector property.
    Vector2Int = 20,
    //
    // Summary:
    //     Represents a 3D integer vector property.
    Vector3Int = 21,
    //
    // Summary:
    //     Represents a rectangle with Integer values property.
    RectInt = 22,
    //
    // Summary:
    //     Represents a bounds with Integer values property.
    BoundsInt = 23,
    //
    // Summary:
    //     Represents a property that references an object that does not derive from UnityEngine.Object.
    ManagedReference = 24,
    //
    // Summary:
    //     Represents a Hash128 property.
    Hash128 = 25
}

namespace FavobeanGames.Components;

/// <summary>
/// Projection type of the camera.
/// (Orthographic, Perspective, etc)
/// </summary>
public enum CameraProjectionType
{
    Orthographic,
    Perspective,
}

/// <summary>
/// Class containing the Camera options (Projection type, View type, etc.)
/// </summary>
public class CameraOptions
{
    public CameraProjectionType ProjectionType { get; }
    public CameraOptions(CameraProjectionType projectionType)
    {
        ProjectionType = projectionType;
    }

    /// <summary>
    /// CameraOptions for basic Perspective camera
    /// </summary>
    public static readonly CameraOptions PerspectiveCameraOptions = new(CameraProjectionType.Perspective);

    /// <summary>
    /// CameraOptions for basic Orthographic camera
    /// </summary>
    public static readonly CameraOptions OrthographicCameraOptions = new(CameraProjectionType.Orthographic);
}
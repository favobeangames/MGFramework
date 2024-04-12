using FavobeanGames.MGFramework.Graphics.Primitives;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Rectangle = FavobeanGames.MGFramework.Graphics.Primitives.Rectangle;
using Transform2 = FavobeanGames.MGFramework.Transform2;

namespace RacketRivalsCC.Match;

public class Court
{
    // Middle boxes of the court
    public Polygon TopLeftServiceBox;
    public Polygon TopRightServiceBox;
    public Polygon BottomLeftServiceBox;
    public Polygon BottomRightServiceBox;

    // Furthest strip of court on the left and right
    public Polygon LeftDoublesAlley;
    public Polygon RightDoublesAlley;

    // Furthest top or bottom section of the court
    public Polygon TopBackCourt;
    public Polygon BottomBackCourt;

    // Serving zones
    public RectangleF BottomLeftServingBounds;
    public RectangleF BottomRightServingBounds;
    public RectangleF TopLeftServingBounds;
    public RectangleF TopRightServingBounds;

    // 5x5 matrix of points on the court to
    // derive the court sections out of
    private Vector2[][] courtPoints;

    public Court() { }

    public Court(Vector2[][] courtPoints, Vector2 position)
    {
        InitializeCourt(courtPoints, position);
    }

    private bool InitializeCourt(Vector2[][] courtPoints, Vector2 position)
    {
        // Validate court points
        if (!ValidatePoints(courtPoints))
        {
            System.Diagnostics.Debug.WriteLine("Court.InitializeCourt(): Court points was invalid");
            return false;
        }

        this.courtPoints = courtPoints;
        CreateCourtPolygons();

        return true;
    }

    private bool ValidatePoints(Vector2[][] courtPoints)
    {
        return courtPoints is not null &&
               courtPoints.Length == 5 &&
               courtPoints[0].Length == 4 &&
               courtPoints[1].Length == 5 &&
               courtPoints[2].Length == 5 &&
               courtPoints[3].Length == 5 &&
               courtPoints[4].Length == 4;
    }

    private void FindCenterPoint(out Vector2 center)
    {
        var width = courtPoints[4][3].X - courtPoints[4][0].X;
        var height = courtPoints[0][0].Y - courtPoints[4][0].Y;
        center = new Vector2(width / 2, height / 2);
    }

    private void CreateCourtPolygons()
    {
        // var tbCourtTransform = new Transform2()
        // {
        //     Position = Vector2.Zero,
        //     Vertices = new []
        //     {
        //         courtPoints[0][1],
        //         courtPoints[0][2],
        //         courtPoints[1][1],
        //         courtPoints[1][3],
        //     }
        // };
        // TopBackCourt = new Polygon(tbCourtTransform, Color.Transparent);
    }
}
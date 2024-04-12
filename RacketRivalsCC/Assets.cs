using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RacketRivalsCC;

public static class Assets
{
    private static Dictionary<string, Texture2D> textures;
    private static Dictionary<string, Effect> shaders;

    /// <summary>
    /// Initialize all textures into collection for use
    /// </summary>
    /// <param name="content"></param>
    public static void Initialize(ContentManager content)
    {
        textures = new Dictionary<string, Texture2D>
        {
            { "basicCourt", content.Load<Texture2D>("courts/Basic") },
            { "basicCourtV2", content.Load<Texture2D>("courts/BasicV2") },
            { "basicBall", content.Load<Texture2D>("avatars/match/Ball") },
            { "basicBall2", content.Load<Texture2D>("avatars/match/Ball2") },
            { "testMatchAvatar", content.Load<Texture2D>("avatars/match/TestAvatar") },
            { "testMatchAvatarSheet", content.Load<Texture2D>("avatars/match/TestAvatarSheet") }
        };

        shaders = new Dictionary<string, Effect>
        {
            {"blinkColor", content.Load<Effect>("shaders/BlinkColor")},
            {"pixelOutline", content.Load<Effect>("shaders/PixelOutline")}
        };
    }

    /// <summary>
    /// Retrieves the texture for the specific key
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static Texture2D Get(string key)
    {
        return textures[key];
    }

    /// <summary>
    /// Retrieves the shader for the specific key
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static Effect GetShader(string key)
    {
        return shaders[key];
    }
}
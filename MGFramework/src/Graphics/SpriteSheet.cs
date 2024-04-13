using System.Collections.Generic;
using FavobeanGames.MGFramework.Util;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Collections;

namespace FavobeanGames.MGFramework.Graphics;

public class AnimationFrame
{
    public Rectangle Rectangle;
    public float Duration;

    public AnimationFrame(Rectangle rectangle, float frameDuration = 0f)
    {
        Rectangle = rectangle;
        Duration = frameDuration;
    }
}
public class Animation
{
    private readonly bool loopsInfinitely;
    private Timer timer;

    private int currentFrameIndex;
    private Bag<AnimationFrame> frames;
    private float baseSpeed = .5f;

    public bool Finished;
    public Rectangle CurrentFrame => frames[currentFrameIndex].Rectangle;

    public Animation(bool loopsInfinitely, params AnimationFrame[] frames)
    {
        timer = new Timer();

        var duration = frames[0]?.Duration is > 0 ? frames[0].Duration : baseSpeed;
        timer.Start(duration);

        this.frames = new Bag<AnimationFrame>();
        this.loopsInfinitely = loopsInfinitely;

        foreach (var frame in frames)
        {
            if (frame.Duration <= 0)
                frame.Duration = baseSpeed;

            this.frames.Add(frame);
        }
    }

    public void Update(float elapsed)
    {
        timer.Update(elapsed);

        if (timer.Finished && !Finished)
        {
            NextFrame();
        }
    }

    public void Reset()
    {
        currentFrameIndex = 0;
        timer.Reset();
    }

    private void NextFrame()
    {
        if (currentFrameIndex + 1 >= frames.Count)
        {
            if (!loopsInfinitely)
                Finished = true;
            else
            {
                currentFrameIndex = 0;
            }
        }
        else
        {
            currentFrameIndex++;
        }

        if (frames[currentFrameIndex].Duration > 0)
            timer.Reset(frames[currentFrameIndex].Duration);
        else
        {
            timer.Reset();
        }
    }
}

public class SpriteSheet
{
    private string currentAnimationKey = "";
    private Dictionary<string, Animation> animations;

    public Animation CurrentAnimation => animations.TryGetValue(currentAnimationKey, out var animation) ? animation : null;

    public SpriteSheet()
    {
        animations = new Dictionary<string, Animation>();
    }

    public void Update(float elapsed)
    {
        CurrentAnimation?.Update(elapsed);
    }

    public void AddAnimation(string key, params AnimationFrame[] frames)
    {
        animations.Add(key, new Animation(false, frames));
    }

    public void AddAnimation(string key, bool loopsInfinitely, params AnimationFrame[] frames)
    {
        animations.Add(key, new Animation(loopsInfinitely, frames));
    }

    public void SetAnimation(string key)
    {
        if (currentAnimationKey == key) return;

        CurrentAnimation?.Reset();

        if (!animations.TryGetValue(key, out _))
        {
            throw new KeyNotFoundException($"Animations do not contain value for key {key}");
        }
        currentAnimationKey = key;
    }
}
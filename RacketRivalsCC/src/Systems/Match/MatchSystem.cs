using System.Linq;
using FavobeanGames.MGFramework;
using FavobeanGames.MGFramework.ECS;
using FavobeanGames.MGFramework.Graphics;
using FavobeanGames.MGFramework.Graphics.Primitives;
using FavobeanGames.MGFramework.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using RacketRivalsCC.Match;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Transform2 = FavobeanGames.MGFramework.Transform2;

namespace RacketRivalsCC.Systems;

public class MatchSystem : EntityUpdateSystem
{
    private ComponentMapper<Player> playerMapper;
    private ComponentMapper<Ball> ballMapper;
    private ComponentMapper<Stats> statsMapper;
    private ComponentMapper<MatchObject> matchMapper;
    private ComponentMapper<MatchPlayer> matchPlayerMapper;
    private ComponentMapper<MatchPlayerAI> matchPlayerAIMapper;
    private ComponentMapper<ServingPlayer> servingPlayerMapper;
    private ComponentMapper<GameObject> gameObjectMapper;

    private bool matchActive;
    private bool matchInitialized;

    private MatchObject currentMatch;
    private int matchEntityId;

    private Entity activeBall;

    public MatchSystem()
        : base(Aspect.Any(typeof(MatchPlayer), typeof(MatchObject), typeof(Ball), typeof(MatchPlayerAI), typeof(GameObject)))
    {
    }

    protected override void Initialize(IComponentService componentService)
    {
        playerMapper = componentService.GetMapper<Player>();
        ballMapper = componentService.GetMapper<Ball>();
        statsMapper = componentService.GetMapper<Stats>();
        matchMapper = componentService.GetMapper<MatchObject>();
        matchPlayerMapper = componentService.GetMapper<MatchPlayer>();
        matchPlayerAIMapper = componentService.GetMapper<MatchPlayerAI>();
        servingPlayerMapper = componentService.GetMapper<ServingPlayer>();
        gameObjectMapper = componentService.GetMapper<GameObject>();
    }

    public override void Update(GameTime gameTime)
    {
        if (!matchInitialized)
        {
            foreach (var entityId in ActiveEntities)
            {
                var match = matchMapper.Get(entityId);
                if (match == null)
                    continue;

                InitializeMatch(match, entityId);
                break;
            }

            // If we made it this far without a match, then we just return
            // because something is wrong...
            return;
        }

        UpdateMatch();
    }

    private void InitializeMatch(MatchObject match, int matchId)
    {
        currentMatch = match;
        matchEntityId = matchId;
        matchInitialized = true;

        // Determine which team should serve
        // For now, it'll be random
        var teamStarting = RandomHelper.RandomInt(0, 1);
        currentMatch.TeamCurrentServing = teamStarting;

        var players = ActiveEntities
            .Select(id => new {id, matchPlayer = matchPlayerMapper.Get(id)})
            .Where(o => o.matchPlayer?.Team == teamStarting)
            .ToList();

        if (players.Count == 1)
        {
            players[0].matchPlayer.Serving = true;
            players[0].matchPlayer.HasServed = false;
            var entity = gameWorld.GetEntity(players[0].id);

            var b = currentMatch.Court.BottomBackCourt.GetAABB();
            var servingBounds = new RectangleF(b.Center.X, b.Top - 1, b.Width / 2, 1);

            entity?.AttachComponent(new ServingPlayer(ServingSide.Right, servingBounds));
        }

        ResetServingTeam();
    }

    private void ResetServingTeam()
    {
        var players = ActiveEntities
            .Select(id => new {id, matchPlayer = matchPlayerMapper.Get(id)})
            .Where(o => o.matchPlayer != null)
            .ToList();

        foreach (var o in players)
        {
            if (o.matchPlayer.Team == currentMatch?.TeamCurrentServing)
            {

            }
        }
    }

    private void FinishMatch()
    {
        currentMatch = null;
        matchInitialized = false;
        DestroyEntity(matchEntityId);
    }

    private void UpdateMatch()
    {
        var players = ActiveEntities
            .Select(entityId => new { entityId, matchPlayer = matchPlayerMapper.Get(entityId) })
            .ToList();

        // Are any players currently serving?
        var playerServing = players
            .Select(p => p)
            .Where(p => p.matchPlayer is not null)
            .FirstOrDefault(p => p.matchPlayer.Serving);

        if (playerServing is not null)
        {
            UpdateServingPlayer(playerServing.entityId, playerServing.matchPlayer);
        }

        foreach (var matchPlayer in players)
        {

        }
    }

    private void UpdateServingPlayer(int entityId, MatchPlayer player)
    {
        if (player.ThrownServe && activeBall is null)
        {
            var playerGameObj = gameObjectMapper.Get(entityId);
            activeBall = CreateNewBall(playerGameObj.Position);
        }
        return;
    }

    private Entity CreateNewBall(Vector2 position)
    {
        var newBall = gameWorld.CreateEntity();
        var ballOrigin = new Vector2(16, 16);
        var ballTransform = Transform2.Empty;
        // var ballBodyTransform = new Transform2()
        // {
        //     Position = new Vector2(0, 2.5f),
        //     Radius = 2f
        // };

        var ballObj = new GameObject(7, 5, ballTransform, ballOrigin)
        {
            HeightOffGround = 22
        };
        var ballSprite = new Sprite(Assets.Get("basicBall2"), ballTransform, new Rectangle(0, 0, 32, 32), ballOrigin)
        {
            LayerDepth = 1
        };
        //ballObj.CreateGameObjectBody(newBall.Id, ballBodyTransform, newBall);
        ballObj.CreateGameObjectShadow(ballTransform, newBall);

        ballObj.SetPosition(position);

        newBall.AttachComponent(new Ball());
        newBall.AttachComponent(ballObj);
        newBall.AttachComponent(ballSprite);
        //newBall.AttachComponent(new Circle(ballBodyTransform, 16, Color.Aqua));

        return newBall;
    }
}
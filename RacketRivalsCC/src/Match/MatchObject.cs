using System.Collections.Generic;
using System.Linq;

namespace RacketRivalsCC.Match;

public class MatchObject
{
    private class TeamScore
    {
        private enum AllowedPoints
        {
            _0,
            _15,
            _30,
            _40,
            Deuce,
            Adv
        }

        private AllowedPoints pointValue;

        public string Points => pointValue.ToString().Replace("_", "");
        public int GameScore;
        public int SetScore;

        public TeamScore()
        {
            pointValue = AllowedPoints._0;
            GameScore = 0;
            SetScore = 0;
        }

        public bool IsScoreDeuce()
        {
            return pointValue is AllowedPoints._40 or AllowedPoints.Deuce;
        }

        public void AddPoints()
        {
            switch (pointValue)
            {
                case AllowedPoints._0:
                case AllowedPoints._15:
                case AllowedPoints._30:
                case AllowedPoints._40:
                case AllowedPoints.Deuce:
                    pointValue += 1;
                    break;
            }
        }

        public void RemovePoints()
        {
            switch (pointValue)
            {
                case AllowedPoints._15:
                case AllowedPoints._30:
                case AllowedPoints._40:
                case AllowedPoints.Deuce:
                case AllowedPoints.Adv:
                    pointValue -= 1;
                    break;
            }
        }
    }

    private TeamScore[] teamScores;
    private int setsRequired;
    private bool tieBreakSet;

    public int TeamCurrentServing;

    public readonly Court Court;
    public readonly bool Finished;

    public MatchObject(Court court, int setsRequired)
    {
        teamScores = new TeamScore[]
        {
            new(),
            new()
        };

        Finished = false;

        Court = court;
        this.setsRequired = setsRequired;
    }

    /// <summary>
    /// Adds score to the teams total. If team has won a game/set, then returns true
    /// Otherwise, returns false
    /// </summary>
    /// <param name="team">Index of the team that added a score (0 is bottom started team, 1 is top)</param>
    /// <returns></returns>
    public bool AddScore(int team)
    {
        if (team > teamScores.Length - 1 || team < teamScores.Length)
        {
            System.Diagnostics.Debug.WriteLine("Match.AddScore() error: team index out of bounds of teamScores array");
            return false;
        }

        var score = teamScores[team];
        var otherScore = teamScores[team == 0 ? 1 : 0];

        if (score == null)
            return false;

        // Check game/set win first
        if (score.Points == "Adv" || (score.Points == "40" && otherScore.IsScoreDeuce()))
        {
            // Has won a game
            return true;

            // TODO: Update game and set score logic
        }

        score.AddPoints();
        switch (score.Points)
        {
            case "40" when otherScore.Points == "40":
                // Set both scores to deuce
                score.AddPoints();
                otherScore.AddPoints();
                break;
            case "Adv" when otherScore.Points == "Deuce":
                // Update other score back to 40
                otherScore.RemovePoints();
                break;
        }

        return false;
    }
}
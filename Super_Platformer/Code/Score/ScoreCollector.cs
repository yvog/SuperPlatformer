namespace Super_Platformer.Code.Score
{
    /// <summary>
    /// Score collector keeps track of the score.
    /// </summary>
    public class ScoreCollector
    {
        /// <summary> Total coins. </summary>
        public int TotalCoins
        {
            get;
            private set;
        }

        /// <summary> Total score. </summary>
        public int TotalScore
        {
            get;
            private set;
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ScoreCollector()
        {
            // Set score to 0.
            TotalScore = 0;

            // Set coins to 0.
            TotalCoins = 0;
        }


        /// <summary>
        /// Increase the total coins.
        /// </summary>
        /// <param name="value"> Increase by value.</param>
        public void IncreaseCoins(int value)
        {
            // check if value is more than 0, you can't lose coins.
            if (value > 0)
            {
                // Add value to the coins.
                TotalCoins += value;
            }
        }

        /// <summary>
        /// Increase the total score.
        /// </summary>
        /// <param name="value"> Increase by value.</param>
        public void IncreaseScore(int value)
        {
            // check if value is more than 0, you can't lose points.
            if (value > 0)
            {
                // Add value to the score.
                TotalScore += value;
            }
        }

        /// <summary>
        /// Reset the score.
        /// </summary>
        public void Reset()
        {
            TotalScore = 0;
            TotalCoins = 0;
        }
    }
}

namespace TeamBallGame.Model
{
    public partial class Player
    {
        public enum State
        {
            UserControl,
            ReturnToPosition,
            AIControl,
            Waiting,
            Tackled
        }
    }
}
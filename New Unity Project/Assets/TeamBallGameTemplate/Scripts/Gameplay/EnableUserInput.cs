namespace TeamBallGame.Gameplay
{
    /// <summary>
    /// This event is fired when user input has been reenabled
    /// for gameplay reasons (eg Ball up contest started)
    /// </summary>
    /// <typeparam name="DisableUserInput"></typeparam>
    public class EnableUserInput : Simulation.Event<EnableUserInput>
    {
        public override void Execute()
        {

        }
    }
}
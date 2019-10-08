namespace TeamBallGame.Gameplay
{
    /// <summary>
    /// This event is fired when user input has been disabled 
    /// for gameplay reasons (eg referee suspends play, goal is scored)
    /// </summary>
    /// <typeparam name="DisableUserInput"></typeparam>
    public class DisableUserInput : Simulation.Event<DisableUserInput>
    {
        public override void Execute()
        {

        }
    }
}
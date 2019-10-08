using System.Collections.Generic;

namespace TeamBallGame
{
    public static partial class Simulation
    {
        public abstract class Event : System.IComparable<Event>
        {
            internal float tick;

            public int CompareTo(Event other)
            {
                return tick.CompareTo(other.tick);
            }

            public abstract void Execute();

            internal virtual bool CheckPrecondition() => true;

            internal virtual void ExecuteEvent()
            {
                if (CheckPrecondition())
                    Execute();
            }

            internal virtual void Cleanup()
            {

            }
        }

        public abstract class Event<T> : Event where T : Event<T>
        {
            public static System.Action<T> OnExecute;

            internal override void ExecuteEvent()
            {
                if (CheckPrecondition())
                {
                    Execute();
                    OnExecute?.Invoke((T)this);
                }
            }
        }
    }
}
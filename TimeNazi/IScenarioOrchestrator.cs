using System;
namespace TimeNazi
{
    public class ElapsedEventArgs : EventArgs
    {
        public long ElapsedTime { get; set; }

        public ElapsedEventArgs()
        {
        }

        public ElapsedEventArgs(long elapsedTime)
        {
            this.ElapsedTime = elapsedTime;
        }

        public ElapsedEventArgs(double elapsedTime)
        {
            this.ElapsedTime = (long)elapsedTime;
        }
    }

    public delegate void ElapsedEventHandler(object sender, ElapsedEventArgs e);

    interface IScenarioOrchestrator
    {
        void Initialize();
        void Pause();
        event ElapsedEventHandler RestElapsed;
        void Resume();
        event ElapsedEventHandler SnoozeElapsed;
        void StartRest();
        bool StartSnooze();
        void StartWork();
        event ElapsedEventHandler WorkElapsed;
    }
}

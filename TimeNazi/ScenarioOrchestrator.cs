using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Timers;
using System.Windows.Forms;

namespace TimeNazi
{
    public class ScenarioOrchestrator : TimeNazi.IScenarioOrchestrator
    {
        public int WorkTime { get; set; }
        public int SnoozeTime { get; set; }
        public int RestTime { get; set; }
        public int PauseTime { get; set; }
        public int NumberOfSnoozes { get; set; }
        public States CurrentState { get; set; }
        public bool TimerIsRunning { get { return _tTimer.Enabled; } }
        public bool IsPaused { get { return _bIsPaused; } }
        public int NumberOfSnoozesRemaining { get { return _iActualNumberOfSnoozes; } }
        public bool Enabled { get { return _tTimer.Enabled; } set { _tTimer.Enabled = value; } }

        public event ElapsedEventHandler WorkElapsed;
        public event ElapsedEventHandler SnoozeElapsed;
        public event ElapsedEventHandler RestElapsed;
        public event ElapsedEventHandler Tick;
        public event EventHandler MinuteWarning;

        private TimeSpan _tsWorkTimer;
        private TimeSpan _tsSnoozeTimer;
        private TimeSpan _tsRestTimer;
        private TimeSpan _tsBaseTime;
        private Timer _tTimer;
        private int _iActualNumberOfSnoozes;
        private DateTime _dtTimerStartTime;
        private DateTime _dtPauseStartTime;
        private bool _bIsInitialized = false;
        private bool _bIsPaused = false;
        private bool _bMinuteWarningRaised = false;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public ScenarioOrchestrator()
        {
            _tTimer = new Timer();
            //_tTimer.AutoReset = true;
            _tTimer.Interval = 1000;
            //_tTimer.Elapsed += _tTimer_Elapsed;
            _tTimer.Tick += _tTimer_Tick;
            CurrentState = States.Startup;
        }

        public void Initialize()
        {
            // the timer should be off
            //_tTimer.SynchronizingObject = (System.ComponentModel.ISynchronizeInvoke)syncObject;
            _tTimer.Enabled = false;
            _resetSnoozes();
            // reset the timers
            _tsRestTimer = _getTimeSpan(RestTime);
            _tsSnoozeTimer = _getTimeSpan(SnoozeTime);
            _tsWorkTimer = _getTimeSpan(WorkTime);
            _bIsInitialized = true;
            _bMinuteWarningRaised = false;
        }

        public TimeSpan GetCurrentTimer()
        {
            TimeSpan tsRet = TimeSpan.Zero;
            switch (CurrentState)
            {
                case States.Startup:
                    tsRet = _tsWorkTimer;
                    break;
                case States.Working:
                    tsRet = _tsWorkTimer;
                    break;
                case States.Snooze:
                    tsRet = _tsSnoozeTimer;
                    break;
                case States.Resting:
                    tsRet = _tsRestTimer;
                    break;
                default:
                    break;
            }
            return tsRet;
        }

        private void _resetSnoozes()
        {
            // reset the number of snoozes
            _iActualNumberOfSnoozes = NumberOfSnoozes;
        }

        private void _checkInitialized()
        {
            if (!_bIsInitialized)
            {
                throw new ApplicationException("Call Initialize() first!");
            }
        }

        public void StartWork()
        {
            _checkInitialized();
            _tsBaseTime = (_tsWorkTimer.TotalMilliseconds > 0) ? _tsWorkTimer : _getTimeSpan(WorkTime);
            CurrentState = States.Working;
            _startTimer();
        }

        public bool StartSnooze()
        {
            _checkInitialized();
            if (CurrentState != States.Resting && !_bIsPaused)
            {
                return false;
            }
            if (_iActualNumberOfSnoozes <= 0)
            {
                return false;
            }
            _iActualNumberOfSnoozes--;
            _tsBaseTime = _getTimeSpan(SnoozeTime);
            CurrentState = States.Snooze;
            _startTimer();

            return true;
        }

        public void StartRest()
        {
            _checkInitialized();
            _tsBaseTime = (_tsRestTimer.TotalMilliseconds > 0) ? _tsRestTimer : _getTimeSpan(RestTime);
            CurrentState = States.Resting;
            _startTimer();
            _bMinuteWarningRaised = false;
        }

        public void Pause()
        {
            if (_bIsPaused)
            {
                return;
            }
            _checkInitialized();
            logger.Trace("Paused");
            _bIsPaused = true;
            _dtPauseStartTime = DateTime.Now;
            if (States.Working == CurrentState)
            {
                _stopTimer();
            }
        }

        public void Resume()
        {
            if (!_bIsPaused)
            {
                return;
            }
            _checkInitialized();
            logger.Trace("Resumed");
            _bIsPaused = false;
            TimeSpan tsTotalTimeInPause = DateTime.Now - _dtPauseStartTime;
            if (tsTotalTimeInPause >= _getTimeSpan(PauseTime))
            {
                if (CurrentState == States.Working)
                {
                    _tsWorkTimer = _getTimeSpan(WorkTime);
                    StartWork();
                }
            }
            else
            {
                if (CurrentState == States.Working)
                {
                    StartWork();
                }
            }
            //_startTimer();
        }

        private void _startTimer()
        {
            if (_bIsPaused)
            {
                return;
            }
            _dtTimerStartTime = DateTime.Now;
            _tTimer.Start();
        }

        private void _stopTimer()
        {
            _tTimer.Stop();
        }

        private void _riseMinuteWarning()
        {
            if (!_bMinuteWarningRaised && null != MinuteWarning)
            {
                MinuteWarning(this, EventArgs.Empty);
                _bMinuteWarningRaised = true;
            }
        }

        private void _handleTimerElapsed()
        {
            // reduce the timers
            TimeSpan _tsElapsedTime = DateTime.Now - _dtTimerStartTime;
            switch (CurrentState)
            {
                case States.Startup:
                    break;
                case States.Working:
                    _tsWorkTimer = _tsBaseTime - _tsElapsedTime;
                    if (_tsWorkTimer < _getTimeSpan(1))
                    {
                        _riseMinuteWarning();
                    }
                    if (_tsWorkTimer < TimeSpan.FromMilliseconds(1))
                    {
                        logger.Trace("Work timer elapsed {0}", null != WorkElapsed);
                        _stopTimer();
                        //CurrentState = States.Resting;
                        _tsWorkTimer = _getTimeSpan(WorkTime);
                        //_startTimer();
                        StartRest();
                        if (null != WorkElapsed)
                        {
                            WorkElapsed(this, new ElapsedEventArgs(_tsElapsedTime.TotalSeconds));
                        }
                    }
                    break;
                case States.Snooze:
                    _tsSnoozeTimer = _tsBaseTime - _tsElapsedTime;
                    if (_tsSnoozeTimer < _getTimeSpan(1))
                    {
                        _riseMinuteWarning();
                    }
                    // raise the apropriate event when a timer reached zero
                    if (_tsSnoozeTimer < TimeSpan.FromMilliseconds(1))
                    {
                        logger.Trace("Snooze timer elapsed {0}", null != SnoozeElapsed);
                        _stopTimer();
                        //CurrentState = States.Resting;
                        _tsSnoozeTimer = _getTimeSpan(SnoozeTime);
                        //_startTimer();
                        StartRest();
                        if (null != SnoozeElapsed)
                        {
                            SnoozeElapsed(this, new ElapsedEventArgs(_tsElapsedTime.TotalSeconds));
                        }
                    }
                    break;
                case States.Resting:
                    _tsRestTimer = _tsBaseTime - _tsElapsedTime;
                    if (_tsRestTimer < TimeSpan.FromMilliseconds(1))
                    { // rest time is over, start work time again
                        logger.Trace("Rest timer elapsed {0}", null != RestElapsed);
                        // reset the counters
                        _stopTimer();
                        //CurrentState = States.Working;
                        _tsRestTimer = _getTimeSpan(RestTime);
                        //_startTimer();
                        _resetSnoozes();
                        //StartWork();
                        if (null != RestElapsed)
                        {
                            RestElapsed(this, new ElapsedEventArgs(_tsElapsedTime.TotalSeconds));
                        }
                    }
                    break;
                default:
                    break;
            }
            logger.Trace("state: {4}, p: {7}, dT: {3:0}, _tsWorkTimer: {0:0}, _tsSnoozeTimer: {1:0} ({5}/{6}), _tsRestTimer: {2:0}", _tsWorkTimer.TotalSeconds, _tsSnoozeTimer.TotalSeconds, _tsRestTimer.TotalSeconds, _tsElapsedTime.TotalSeconds, CurrentState, NumberOfSnoozes, _iActualNumberOfSnoozes, _bIsPaused);
            if (null != Tick)
            {
                Tick(this, new ElapsedEventArgs(_tsElapsedTime.TotalSeconds));
            }
        }

        private TimeSpan _getTimeSpan(int minutes)
        {
            return TimeSpan.FromMinutes(minutes);
        }

        //void _tTimer_Elapsed(object sender, ElapsedEventArgs e)
        //{
        //    _handleTimerElapsed();
        //}

        void _tTimer_Tick(object sender, EventArgs e)
        {
            _handleTimerElapsed();
        }
    }
}

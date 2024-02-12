using System;

namespace Tzipory.Tools.TimeSystem
{
    public interface ITimer
    {
        public event Action<ITimer,bool>  OnTimerComplete;
        
        public string TimerName { get; }

        public string CompleteMethodName { get; }
        
        public bool IsDone { get; }

        public float TimeRemaining { get; }
        
        public void TickTimer();
        public void StopTimer(bool executeOnComplete = false);
    }

    public class Timer : ITimer
    {
        public event Action<ITimer,bool> OnTimerComplete;

        private readonly Action _onComplete;

        private bool _isTimerStopped;
        
        public string TimerName { get; }
        
        public string CompleteMethodName { get; }

        public float TimeRemaining { get; private set; }

        public bool IsDone => TimeRemaining <= 0 || _isTimerStopped;

        public Timer(string timerName,float time,Action onComplete = null)
        {
            TimerName = timerName;
            TimeRemaining = time;
            _onComplete = onComplete;

            _isTimerStopped = false;
            
#if UNITY_EDITOR
            if (onComplete != null)
                CompleteMethodName = onComplete.Method.Name;
#endif
        }

        public void TickTimer()
        {
            if (_isTimerStopped)
                return;
            
            TimeRemaining -= GAME_TIME.GameDeltaTime;

            if (TimeRemaining <= 0)
            {
                _onComplete?.Invoke();
                OnTimerComplete?.Invoke(this,false);
            }
        }

        public void StopTimer(bool executeOnComplete = false)
        {
            _isTimerStopped = true;
            
            if (executeOnComplete)
                _onComplete?.Invoke();
            
            OnTimerComplete?.Invoke(this,true);
        }
    }
    
    public class Timer<T> : ITimer
    {
        public event Action<ITimer,bool> OnTimerComplete;
        
        private readonly Action<T> _onComplete;
        
        private bool _isTimerStopped;

        private T _parameter;
        
        public string TimerName { get; }
        
        public string CompleteMethodName { get; }

        public float TimeRemaining { get; private set; }
        
        public bool IsDone => TimeRemaining <= 0 || _isTimerStopped;

        public Timer(string timerName,float time,Action<T> onComplete,ref T parameter)
        {
            TimerName = timerName;
            TimeRemaining = time;
            _onComplete = onComplete;
            
            _isTimerStopped = false;
            
            CompleteMethodName  = onComplete.Method.Name;

            _parameter = parameter;
        }

        public void TickTimer()
        {
            if (_isTimerStopped)
                return;
            
            TimeRemaining -= GAME_TIME.GameDeltaTime;

            if (TimeRemaining <= 0)
            {
                _onComplete?.Invoke(_parameter);
                OnTimerComplete?.Invoke(this,false);
            }
        }

        public void StopTimer(bool executeOnComplete = false)
        {
            _isTimerStopped = true;
            
            if (executeOnComplete)
                _onComplete?.Invoke(_parameter);
            
            OnTimerComplete?.Invoke(this,true);
        }
    }
    
    public class Timer<T1,T2> : ITimer
    {
        public event Action<ITimer,bool>  OnTimerComplete;
        
        private readonly Action<T1,T2> _onComplete;
        
        private bool _isTimerStopped;

        private T1 _parameter1;
        private T2 _parameter2;
        
        public string TimerName { get; }
        
        public string CompleteMethodName { get; }

        public float TimeRemaining { get; private set; }
        
        public bool IsDone => TimeRemaining <= 0 || _isTimerStopped;

        public Timer(string timerName,float time,Action<T1,T2> onComplete,ref T1 parameter1,ref T2 parameter2)
        {
            TimerName = timerName;
            TimeRemaining = time;
            _onComplete = onComplete;
            
            _isTimerStopped = false;
            
            CompleteMethodName  = onComplete.Method.Name;

            _parameter1 = parameter1;
            _parameter2 = parameter2;
        }

        public void TickTimer()
        {
            if (_isTimerStopped)
                return;
            
            TimeRemaining -= GAME_TIME.GameDeltaTime;

            if (TimeRemaining <= 0)
            {
                _onComplete?.Invoke(_parameter1,_parameter2);
                OnTimerComplete?.Invoke(this,false);
            }
        }

        public void StopTimer(bool executeOnComplete = false)
        {
            _isTimerStopped = true;
            
            if (executeOnComplete)
                _onComplete?.Invoke(_parameter1,_parameter2);
            
            OnTimerComplete?.Invoke(this,true);
        }
    }
    
    public class Timer<T1,T2,T3> : ITimer
    {
        public event Action<ITimer,bool> OnTimerComplete;
        
        private readonly Action<T1,T2,T3> _onComplete;
        
        private bool _isTimerStopped;

        private T1 _parameter1;
        private T2 _parameter2;
        private T3 _parameter3;
        
        public string TimerName { get; }
        
        public string CompleteMethodName { get; }

        public float TimeRemaining { get; private set; }
        
        public bool IsDone => TimeRemaining <= 0 || _isTimerStopped;

        public Timer(string timerName,float time,Action<T1,T2,T3> onComplete,ref T1 parameter1,ref T2 parameter2,ref T3 parameter3)
        {
            TimerName = timerName;
            TimeRemaining = time;
            _onComplete = onComplete;
            
            _isTimerStopped = false;
            
            CompleteMethodName  = onComplete.Method.Name;

            _parameter1 = parameter1;
            _parameter2 = parameter2;
            _parameter3 = parameter3;
        }

        public void TickTimer()
        {
            if (_isTimerStopped)
                return;
            
            TimeRemaining -= GAME_TIME.GameDeltaTime;

            if (TimeRemaining <= 0)
            {
                _onComplete?.Invoke(_parameter1,_parameter2,_parameter3);
                OnTimerComplete?.Invoke(this,false);
            }
        }

        public void StopTimer(bool executeOnComplete = false)
        {
            _isTimerStopped = true;
            
            if (executeOnComplete)
                _onComplete?.Invoke(_parameter1,_parameter2,_parameter3);
            
            OnTimerComplete?.Invoke(this,true);
        }
    }
    
    public class Timer<T1,T2,T3,T4> : ITimer
    {
        public event Action<ITimer,bool> OnTimerComplete;
        
        private readonly Action<T1,T2,T3,T4> _onComplete;
        
        private bool _isTimerStopped;

        private T1 _parameter1;
        private T2 _parameter2;
        private T3 _parameter3;
        private T4 _parameter4;
        
        public string TimerName { get; }
        
        public string CompleteMethodName { get; }

        public float TimeRemaining { get; private set; }
        
        public bool IsDone => TimeRemaining <= 0 || _isTimerStopped;

        public Timer(string timerName,float time,Action<T1,T2,T3,T4> onComplete,ref T1 parameter1,ref T2 parameter2,ref T3 parameter3,ref T4 parameter4)
        {
            TimerName = timerName;
            TimeRemaining = time;
            _onComplete = onComplete;
            
            _isTimerStopped = false;
            
            CompleteMethodName  = onComplete.Method.Name;

            _parameter1 = parameter1;
            _parameter2 = parameter2;
            _parameter3 = parameter3;
            _parameter4 = parameter4;
        }

        public void TickTimer()
        {
            if (_isTimerStopped)
                return;
            
            TimeRemaining -= GAME_TIME.GameDeltaTime;

            if (TimeRemaining <= 0)
            {
                _onComplete?.Invoke(_parameter1,_parameter2,_parameter3,_parameter4);
                OnTimerComplete?.Invoke(this,false);
            }
        }

        public void StopTimer(bool executeOnComplete = false)
        {
            _isTimerStopped = true;
            
            if (executeOnComplete)
                _onComplete?.Invoke(_parameter1,_parameter2,_parameter3,_parameter4);
            
            OnTimerComplete?.Invoke(this,true);
        }
    }
    
    public class Timer<T1,T2,T3,T4,T5> : ITimer
    {
        public event Action<ITimer,bool> OnTimerComplete;
        
        private readonly Action<T1,T2,T3,T4,T5> _onComplete;
        
        private bool _isTimerStopped;

        private T1 _parameter1;
        private T2 _parameter2;
        private T3 _parameter3;
        private T4 _parameter4;
        private T5 _parameter5;
        
        public string TimerName { get; }
        
        public string CompleteMethodName { get; }

        public float TimeRemaining { get; private set; }
        
        public bool IsDone => TimeRemaining <= 0 || _isTimerStopped;

        public Timer(string timerName,float time,Action<T1,T2,T3,T4,T5> onComplete,ref T1 parameter1,ref T2 parameter2,ref T3 parameter3, ref T4 parameter4,ref T5 parameter5)
        {
            TimerName = timerName;
            TimeRemaining = time;
            _onComplete = onComplete;
            
            _isTimerStopped = false;
            
            CompleteMethodName  = onComplete.Method.Name;

            _parameter1 = parameter1;
            _parameter2 = parameter2;
            _parameter3 = parameter3;
            _parameter4 = parameter4;
            _parameter5 = parameter5;
        }

        public void TickTimer()
        {
            if (_isTimerStopped)
                return;
            
            TimeRemaining -= GAME_TIME.GameDeltaTime;

            if (TimeRemaining <= 0)
            {
                _onComplete?.Invoke(_parameter1,_parameter2,_parameter3,_parameter4,_parameter5);
                OnTimerComplete?.Invoke(this,false);
            }
        }

        public void StopTimer(bool executeOnComplete = false)
        {
            _isTimerStopped = true;
            
            if (executeOnComplete)
                _onComplete?.Invoke(_parameter1,_parameter2,_parameter3,_parameter4,_parameter5);
            
            OnTimerComplete?.Invoke(this,true);
        }
    }
}
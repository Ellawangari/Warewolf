﻿using System.Collections.Generic;
using Dev2.Common;
using Dev2.Common.Interfaces.Monitoring;
using System;

namespace Dev2.PerformanceCounters.Counters
{
    public class WarewolfServicesNotFoundCounter : MyPerfCounter, IPerformanceCounter
    {
        bool _started;
        readonly WarewolfPerfCounterType _perfCounterType;

        public WarewolfServicesNotFoundCounter(IRealPerformanceCounterFactory performanceCounterFactory)
            :base(performanceCounterFactory)
        {
            _started = false;
            IsActive = true;
            _perfCounterType = WarewolfPerfCounterType.ServicesNotFound;
        }

        public WarewolfPerfCounterType PerfCounterType => _perfCounterType;

        public IEnumerable<(string CounterName, string CounterHelp, PerformanceCounterType CounterType)> CreationData()
        {

            yield return (
                Name,
                Name,
                PerformanceCounterType.NumberOfItems32
            );
        }

        #region Implementation of IPerformanceCounter

        public void Increment()
        {
 
    
                if (IsActive)
            {
                _counter.Increment();
            }
        }

        public void IncrementBy(long ticks)
        {

                _counter.IncrementBy(ticks);

        }

        public void Setup()
        {
            if (!_started)
            {
                _counter = _counterFactory.New(GlobalConstants.Warewolf, Name, GlobalConstants.GlobalCounterName);
                _started = true;
            }

        }

        public void Decrement()
        {
            if (IsActive)
            {

                _counter.Decrement();
            }
  
        }

        public string Category => "Warewolf";

        public string Name => "Count of requests for workflows which don't exist";

        public void Reset()
        {
            if (_counter != null)
            {
                _counter.RawValue = 0;
            }
        }

        new public void Dispose()
        {
            if (_counter != null)
            {
                _counter.Dispose();
            }
        }
        #endregion
    }
}
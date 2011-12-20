//-----------------------------------------------------------------------------
// GameTimer.cs
//
// Copyright 2011, BinaryConstruct.
// Licensed under the terms of the Ms-PL: http://www.microsoft.com/opensource/licenses.mspx#Ms-PL
//-----------------------------------------------------------------------------

using System;
using System.Diagnostics;

namespace BCCL.UI.Xaml.XnaContentHost
{
    public class GameTimer : Stopwatch
    {
        public void Update()
        {
            TimeSpan mark = this.Elapsed;
            _elapsedGameTime = mark - _lastUpdate;
            _lastUpdate = mark;
        }

        private TimeSpan _lastUpdate = TimeSpan.FromSeconds(0);
        private TimeSpan _elapsedGameTime = TimeSpan.FromSeconds(0);

        public TimeSpan ElapsedGameTime
        {
            get { return _elapsedGameTime; }
        }
    }
}
﻿// Copyright 2015 ThoughtWorks, Inc.
//
// This file is part of Gauge-CSharp.
//
// Gauge-CSharp is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Gauge-CSharp is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with Gauge-CSharp.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Gauge.CSharp.Runner
{
    [Serializable]
    public class StepRegistry : IStepRegistry
    {
        private readonly Dictionary<string, MethodInfo> _scannedSteps = new Dictionary<string, MethodInfo>();
        private readonly Dictionary<string, string> _stepTextMap;
        private readonly Dictionary<string, bool> _aliases;

        public StepRegistry(IEnumerable<KeyValuePair<string, MethodInfo>> scannedSteps, Dictionary<string, string> stepTextMap, Dictionary<string, bool> aliases)
        {
            _stepTextMap = stepTextMap;
            _aliases = aliases;
            foreach (var stepMap in scannedSteps)
            {
                if (_scannedSteps.ContainsKey(stepMap.Key))
                {
                    _scannedSteps[stepMap.Key] = stepMap.Value;
                }
                else
                {
                    _scannedSteps.Add(stepMap.Key, stepMap.Value);
                }
            }
        }

        public bool ContainsStep(string parsedStepText)
        {
            return _scannedSteps.ContainsKey(parsedStepText);
        }

        public MethodInfo MethodFor(string parsedStepText)
        {
            return _scannedSteps[parsedStepText];
        }

        public IEnumerable<string> AllSteps()
        {
            return _scannedSteps.Keys;
        }

        public bool HasAlias(string parsedStepText)
        {
            return _aliases.ContainsKey(parsedStepText) && _aliases[parsedStepText];
        }

        public string GetStepText(string parameterizedStepText)
        {
            return _stepTextMap.ContainsKey(parameterizedStepText) ? _stepTextMap[parameterizedStepText] : string.Empty;
        }
    }
}
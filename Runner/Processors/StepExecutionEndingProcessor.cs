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

using System.Collections.Generic;
using System.Linq;
using Gauge.Messages;

namespace Gauge.CSharp.Runner.Processors
{
    public class StepExecutionEndingProcessor : TaggedHooksFirstExecutionProcessor
    {
        public StepExecutionEndingProcessor(IHookRegistry hookRegistry, IMethodExecutor methodExecutor)
            : base(hookRegistry, methodExecutor)
        {
        }

        protected override HashSet<HookMethod> GetHooks()
        {
            return Hooks.AfterStepHooks;
        }

        protected override ProtoExecutionResult.Builder ExecuteHooks(Message request)
        {
            var protoExecutionResultBuilder = base.ExecuteHooks(request);
            var allPendingMessages = MethodExecutor.GetAllPendingMessages();
            protoExecutionResultBuilder.AddRangeMessage(allPendingMessages);
            return protoExecutionResultBuilder;
        }

        protected override ExecutionInfo GetExecutionInfo(Message request)
        {
            return request.StepExecutionEndingRequest.CurrentExecutionInfo;
        }

        protected override IEnumerable<string> GetApplicableTags(Message request)
        {
            return GetExecutionInfo(request).CurrentScenario.TagsList
                .Union(GetExecutionInfo(request).CurrentSpec.TagsList);
        }
    }
}
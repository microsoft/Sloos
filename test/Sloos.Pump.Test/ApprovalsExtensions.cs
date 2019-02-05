// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Text;
using ApprovalTests;
using Sloos.Pump.TextTemplate;

namespace Sloos.Pump.Test
{
    public static class ApprovalsExtensions
    {
        public static void Verify(IFactory factory)
        {
            var sb = new StringBuilder();
            factory.WriteTo(sb);

            Approvals.Verify(sb.ToString());
        }
    }
}

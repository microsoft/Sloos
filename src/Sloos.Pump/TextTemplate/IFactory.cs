// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Text;

namespace Sloos.Pump.TextTemplate
{
    public interface IFactory
    {
        void WriteTo(StringBuilder sb);
    }
}

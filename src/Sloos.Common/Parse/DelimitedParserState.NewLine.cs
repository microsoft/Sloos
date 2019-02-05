// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Globalization;

namespace Sloos.Common
{
    public sealed class DelimitedParserStateNewLine :
            DelimitedParserStateContext
    {
        public DelimitedParserStateNewLine(IDelimitedParser parser) :
            base(parser)
        {
        }

        public override bool Push(char c)
        {
            switch (c)
            {
                // This state transitions when it finds the \r character, which
                // means this case should never be hit in a well-formed document.
                // (The previous state eats the \r before transitioning.)
                case '\r':
                    return true;
                case '\n':
                    this.ChangeState(new DelimitedParserStateCapture(this.Parser));
                    return false;
                default:
                    throw new ArgumentOutOfRangeException(c.ToString(CultureInfo.InvariantCulture));
            }
        }
    }
}

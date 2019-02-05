// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace Sloos.Common
{
    public sealed class DelimitedParserStateDelimit :
        DelimitedParserStateContext
    {
        public DelimitedParserStateDelimit(IDelimitedParser parser) :
            base(parser)
        {
        }

        public override bool Push(char c)
        {
            if (c == this.Delimiter)
            {
                this.ChangeState(new DelimitedParserStateCapture(this.Parser));
            }

            return true;
        }
    }
}

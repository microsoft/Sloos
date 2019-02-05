// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace Sloos.Common
{
    public sealed class DelimitedParserStateCapture :
        DelimitedParserStateContext
    {
        private bool isFirstChar;

        public DelimitedParserStateCapture(IDelimitedParser parser) :
            base(parser)
        {
            this.isFirstChar = true;
        }

        public override void Complete()
        {
            if (!this.isFirstChar)
            {
                this.Append();
            }
        }

        public override bool Push(char c)
        {
            if (this.isFirstChar && DelimitedParserStateQuote.IsQuote(c))
            {
                this.ChangeState(new DelimitedParserStateQuote(this.Parser));
            }
            else if (c == this.Delimiter)
            {
                this.Append();
                this.ChangeState(new DelimitedParserStateCapture(this.Parser));
            }
            else if (c == '\r')
            {
                this.Append();
                this.ChangeState(new DelimitedParserStateNewLine(this.Parser));
            }
            else if (c == '\n')
            {
                this.Append();
                this.ChangeState(new DelimitedParserStateCapture(this.Parser));
            }
            else
            {
                this.Accumulate(c);
            }

            this.isFirstChar = false;
            return true;
        }
    }
}

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace Sloos.Common
{
    public sealed class DelimitedParserStateQuote :
        DelimitedParserStateContext
    {
        private bool isPrevCharQuote;

        public DelimitedParserStateQuote(IDelimitedParser parser)
            : base(parser)
        {
        }

        public override void Complete()
        {
            if (this.isPrevCharQuote)
            {
                this.Append();
            }
        }

        public override bool Push(char c)
        {
            if (this.Delimiter == c && this.isPrevCharQuote)
            {
                this.Append();
                this.ChangeState(new DelimitedParserStateCapture(this.Parser));
            }
            else if (DelimitedParserStateQuote.IsQuote(c) && !this.isPrevCharQuote)
            {
                this.isPrevCharQuote = true;
            }
            else if (DelimitedParserStateQuote.IsQuote(c) && this.isPrevCharQuote)
            {
                this.Accumulate(c);
                this.isPrevCharQuote = false;
            }
            else 
            {
                this.Accumulate(c);
            }

            return true;
        }

        public static bool IsQuote(char c)
        {
            return c == '"';
        }
    }

}

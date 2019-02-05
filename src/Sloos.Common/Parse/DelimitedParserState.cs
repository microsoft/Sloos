// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Text;

namespace Sloos.Common
{
    public abstract class DelimitedParserStateContext
    {
        private readonly IDelimitedParser parser;
        private readonly StringBuilder stringBuilder;
        
        protected internal DelimitedParserStateContext(
            IDelimitedParser parser)
        {
            this.parser = parser;
            this.stringBuilder = new StringBuilder();
        }

        internal IDelimitedParser Parser => this.parser;
        protected char Delimiter => this.parser.Delimiter;
        public string Data => this.stringBuilder.ToString();

        /// <summary>
        /// Push a character into the state machine.
        /// </summary>
        /// <param name="c">
        /// Char to process.
        /// </param>
        /// <returns>
        /// True if the state machine can process more characters, else false.
        /// </returns>
        /// <remarks>
        /// I need to think about this more, it feel a little _off_.
        /// </remarks>
        public abstract bool Push(char c);

        /// <summary>
        /// Complete processing.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method is called when streaming characters and after
        /// exhausting the stream.  If there are no more data to process
        /// this is a clue to the state machine to complete processing.
        /// </para>
        /// </remarks>
        public virtual void Complete() {}

        /// <summary>
        /// Update the current state.
        /// </summary>
        protected void ChangeState(DelimitedParserStateContext context)
        {
            this.parser.ChangeState(context);
        }

        /// <summary>
        /// Accumulate the specified character into the current field being
        /// processed.
        /// </summary>
        protected void Accumulate(char c)
        {
            this.stringBuilder.Append(c);
        }

        /// <summary>
        /// Append the current field to the current record.
        /// </summary>
        protected void Append()
        {
            this.parser.Append(this.Data);
            this.stringBuilder.Clear();
        }
    }
}

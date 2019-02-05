// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace Sloos.Common
{
    public interface IDelimitedParser
    {
        char Delimiter { get; }
        void Append(string field);

        void ChangeState(DelimitedParserStateContext context);
    }
}

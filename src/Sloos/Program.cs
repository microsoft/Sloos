// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;

namespace Sloos
{
    public class Program
    {
        private static int Main(string[] args)
        {
            Options options;

            try
            {
                options = ArgsParser<Options>.Parse(args);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
                return 1;
            }

            var application = new Application(options.BatchSize);
            return application.Pump(options.Table, options.Source, options.Destination);

        }
    }
}

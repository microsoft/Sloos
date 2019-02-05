// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Sloos
{
    public class ArgsParser<T>
        where T : new()
    {
        // ReSharper disable once StaticMemberInGenericType
        private static readonly Dictionary<Type, Func<string, object>> CoercionMap;

        private readonly string[] args;
        private readonly Dictionary<string, string> collectedArgs;

        private readonly ParsedArgFactory factory;
        private readonly T opts;
        private readonly Dictionary<string, ParsedArg> parsedArgs;
        private readonly Dictionary<string, Action<T, object>> propertySetters;

        static ArgsParser()
        {
            ArgsParser<T>.CoercionMap = CoercionHelpers.CreateCoercionMap();
        }

        public ArgsParser(string[] args)
        {
            this.args = args;

            Type type = typeof(T);
            this.opts = (T)Activator.CreateInstance(type);

            this.factory = new ParsedArgFactory(type);
            this.parsedArgs = this.factory.Create();

            this.propertySetters = this.parsedArgs
                .ToDictionary(x => x.Key, x => CoercionHelpers.CreatePropertySetter<T>(x.Value.PropertyInfo));

            this.collectedArgs = new Dictionary<string, string>();
            this.SetDefaultValues();
        }

        public T Parse()
        {
            var qargs = new Queue<string>(
                this.CanonicalizeArgs());

            var unparsedArgs = new List<string>();

            while (qargs.Any())
            {
                var arg = qargs.Dequeue();
                if (arg.StartsWith("--") || arg.StartsWith("-"))
                {
                    this.ProcessArg(arg.Trim('-'), qargs);
                }
                else
                {
                    unparsedArgs.Add(arg);
                }
            }

            this.VerifyRequiredArgumentsAreSet();
            this.SetUnparsedArgs(unparsedArgs);
            this.SetOptionProperties();

            return this.opts;
        }

        private IEnumerable<string> CanonicalizeArgs()
        {
            var quotes = new[] { '"', '\'' };
            var trimmedArgs = this.args
                .Select(x => x.TrimStart(quotes).TrimEnd(quotes))
                .ToArray();

            var breakEqualSigns = this.ExpandEqualSigns(trimmedArgs);
            return this.ExpandShortGroupOptions(breakEqualSigns);
        }

        private IEnumerable<string> ExpandShortGroupOptions(string[] theArgs)
        {
            // EXPAND: -xvfz into -x -v -f -z
            // IGNORE: --file log.txt
            foreach (var arg in theArgs)
            {
                if (!arg.StartsWith("-") || arg.StartsWith("--"))
                {
                    yield return arg;
                    continue;
                }

                foreach (var shortArg in arg.TrimStart('-').ToCharArray())
                {
                    yield return $"-{shortArg}"; 
                }
            }
        }

        private string[] ExpandEqualSigns(string[] theArgs)
        {
            return theArgs
                .Select(x =>
                    // Split arguments if the user specified:
                    //   --value=something
                    // Do not split arguments for these cases:
                    //   --connection-string "Server=localhost;Database=My;Trusted_Connection=True;"
                    //
                    // Short options are *not* allowed to have a equals sign.  This is invalid
                    //   -i=1
                    x.StartsWith("--") ?
                               x.Split(new[] { '=' }, 2, StringSplitOptions.RemoveEmptyEntries) :
                               new[] { x })
                .Flatten()
                .ToArray();
        }

        private void VerifyRequiredArgumentsAreSet()
        {
            var requiredArgsByLongName = new HashSet<string>(
                this.parsedArgs.Where(x => x.Value.IsRequired).Select(x => x.Value.LongName));

            var translatedArgsToLongName = this.collectedArgs.Select(
                x => this.parsedArgs.ContainsKey(x.Key) ? this.parsedArgs[x.Key].LongName : x.Key);

            var specifiedArgsByLongName = new HashSet<string>(
                translatedArgsToLongName);

            var intersect = requiredArgsByLongName.Intersect(specifiedArgsByLongName);
            if (intersect.Count() != requiredArgsByLongName.Count())
            {
                var missingRequiredArgs = requiredArgsByLongName.Except(specifiedArgsByLongName)
                    .ToArray();

                string message = $"The argument(s) {string.Join(", ", missingRequiredArgs.Select(x => "--" + x))} are required, but were not specified!";

                throw new ArgumentException(message);
            }
        }

        private void SetUnparsedArgs(IEnumerable<string> unparsedArgs)
        {
            if (!unparsedArgs.Any())
            {
                return;
            }

            if (this.factory.UnparsedArgs == null)
            {
                string message = "There were unparsed arguments, but no property supports unparsed arguments!";
                throw new NullReferenceException(message);
            }

            // XXX: Is there a better way to handle this?
            if (typeof(string[]) == this.factory.UnparsedArgs.PropertyType)
            {
                this.factory.UnparsedArgs.SetMethod.Invoke(this.opts, new object[] { unparsedArgs.ToArray() });
            }
            else
            {
                var enumerable = Activator.CreateInstance(this.factory.UnparsedArgs.PropertyType, new object[] { unparsedArgs.ToArray() });
                this.factory.UnparsedArgs.SetMethod.Invoke(this.opts, new object[] { enumerable });
            }
        }

        private void SetDefaultValues()
        {
            var argsWithDefaultValues = this.parsedArgs.Where(x => !string.IsNullOrWhiteSpace(x.Value.DefaultValue));

            foreach (var kvp in argsWithDefaultValues)
            {
                this.SetPropertyValue(kvp.Key, kvp.Value.DefaultValue);
            }
        }

        private void ProcessArg(string arg, Queue<string> qargs)
        {
            bool isNegatedArg = this.TryGetUnnegatedArgument(arg, out string unnegatedArgumentName);

            string theArgName = isNegatedArg ? unnegatedArgumentName : arg;

            if (!this.parsedArgs.ContainsKey(theArgName))
            {
                throw new ArgumentException(theArgName);
            }

            string booleanString = isNegatedArg ? Boolean.FalseString : Boolean.TrueString;
            this.collectedArgs[theArgName] = this.parsedArgs[theArgName].HasValue ?
                qargs.Dequeue() :
                booleanString;
        }

        private bool TryGetUnnegatedArgument(string arg, out string unnegatedArgumentName)
        {
            unnegatedArgumentName = string.Empty;

            if (!arg.StartsWith("no-"))
            {
                return false;
            }

            unnegatedArgumentName = arg.Remove(0, 3);
            return this.parsedArgs.ContainsKey(unnegatedArgumentName);
        }

        private void SetOptionProperties()
        {
            foreach (var kvp in this.collectedArgs)
            {
                this.SetPropertyValue(kvp.Key, kvp.Value);
            }
        }

        private void SetPropertyValue(string argName, string argValue)
        {
            var coercer = ArgsParser<T>.CoercionMap[this.parsedArgs[argName].PropertyInfo.PropertyType];
            var value = coercer(argValue);

            this.propertySetters[argName](this.opts, value);
        }

        public static T Parse(string[] args)
        {
            var parser = new ArgsParser<T>(args);
            return parser.Parse();
        }
    }
}

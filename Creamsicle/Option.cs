using System;
using System.Collections.Generic;

namespace Creamsicle
{
    class Option
    {
        internal string Name { get; }
        internal HashSet<string> Aliases { get; }
        internal bool IsRequired { get; }
        internal string HelpMsg { get; }

        internal dynamic Value { get; private set; }
        internal Type Type { get; }


        internal Option(string Name, HashSet<string> Aliases, bool IsRequired, string HelpMsg, dynamic Value, Type Type) : this(Name, Aliases, IsRequired, HelpMsg, Type)
        {
            this.Value = Value;
        }

        internal Option(string Name, HashSet<string> Aliases, bool IsRequired, string HelpMsg, Type Type)
        {
            this.Name = Name;
            this.Aliases = Aliases;
            this.IsRequired = IsRequired;
            this.HelpMsg = HelpMsg;
            this.Type = Type;
        }

        internal void ValidateTypeSetValue(string arg)
        {
            if (Type == typeof(Boolean))
            {
                Value = true;
            }
            else if (!String.IsNullOrWhiteSpace(arg))
            {
                if (Type == typeof(Int32))
                {
                    Value = Int32.Parse(arg);
                }
                else if (Type == typeof(String))
                {
                    Value = arg;
                }
                else
                {
                    throw new ArgumentException($"Not valid type", arg);
                }
            }
            else
            {
                throw new ArgumentException($"Not valid arg (Null/WhiteSpace)", arg);
            }
        }

        internal static void ParseArgs(HashSet<Option> options, string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                bool found = false;
                foreach (Option option in options)
                {
                    foreach (string alias in option.Aliases)
                    {
                        if (args[i].Equals(alias, StringComparison.OrdinalIgnoreCase))
                        {
                            found = true;
                            break;
                        }
                    }
                    if (found)
                    {
                        string value = (++i < args.Length) ? args[i] : null;
                        option.ValidateTypeSetValue(value);
                        break;
                    }
                }
            }

            foreach (Option option in options)
            {
                if (option.IsRequired && option.Value is null)
                {
                    throw new ArgumentException("Required arg is null", option.Name);
                }
            }
        }
    }
}

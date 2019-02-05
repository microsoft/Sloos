// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;

[assembly: AssemblyCompany("Microsoft")]
[assembly: AssemblyProduct("Sloos")]
[assembly: AssemblyCopyright("Copyright Microsoft")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: NeutralResourcesLanguage("en-US")]

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif

[assembly: ComVisible(false)]

[assembly: AssemblyVersion("1.0.*")]
//[assembly: AssemblyInformationalVersion("1.0.*")]

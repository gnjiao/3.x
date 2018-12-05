using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Core")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

[assembly: Guid("29910a98-7f8c-4738-8b42-04affe637b0c")]

[assembly: ComVisible(false)]
[assembly: AssemblyCompany("ic#code")]
[assembly: AssemblyProduct("Core")]
[assembly: AssemblyCopyright("Copyright ©  2018-$INSERTYEAR$ AlphaSierraPapa for the Team")]
[assembly: AssemblyVersion("3.0.1")]
[assembly: AssemblyInformationalVersion(RevisionClass.FullVersion + "-$INSERTSHORTCOMMITHASH$")]
[assembly: NeutralResourcesLanguage("en-US")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2243:AttributeStringLiteralsShouldParseCorrectly",
    Justification = "AssemblyInformationalVersion does not need to be a parsable version")]

internal static class RevisionClass
{
    public const string Major = "5";
    public const string Minor = "1";
    public const string Build = "0";
    public const string Revision = "$INSERTREVISION$";
    public const string VersionName = null; // "" is not valid for no version name, you have to use null if you don't want a version name (eg "Beta 1")

    public const string FullVersion = Major + "." + Minor + "." + Build + ".$INSERTREVISION$$INSERTBRANCHPOSTFIX$$INSERTVERSIONNAMEPOSTFIX$";
}
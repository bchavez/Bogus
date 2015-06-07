using System.Collections.Generic;
using System.IO;
using System.Reflection;
using FluentBuild;
using FluentBuild.UtilitySupport;
using NUnit.Framework;

namespace Builder.Tasks
{
    [TestFixture]
    public class BuildDebugger
    {
        [TestFixtureSetUp]
        public void BeforeRunningTestSession()
        {
            Directory.SetCurrentDirectory( @"..\..\..\.." );
        }

        [Test]
        [Explicit]
        public void Test()
        {
            Properties.CommandLineProperties.Add( "Version", "0.0.0.0" );

            CompilerService.ExecuteBuildTask( Assembly.GetExecutingAssembly().Location,
                                              typeof(LocaleBuildTask).Name, new List<string>() );
        }
    }

}
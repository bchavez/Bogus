using System.Linq;
using FluentAssertions;
using Xunit;
using Z.ExtensionMethods;
using Z.ExtensionMethods.ObjectExtensions;

namespace Bogus.Tests.DataSetTests
{
    public class SystemTest : SeededTest
    {
        public SystemTest()
        {
            system = new DataSets.System();
        }

        private readonly DataSets.System system;

        [Fact]
        public void can_generate_random_semver()
        {
            var numbers = system.Semver().Split('.');

            numbers.TrueForAll(x => x.ToInt32() >= 0 && x.ToInt32() <= 9)
                .Should().BeTrue();
        }

        [Fact]
        public void can_get_a_few_common_file_exts()
        {
            system.CommonFileExt().Should().Be("gif");
            system.CommonFileExt().Should().Be("m1v");
            system.CommonFileExt().Should().Be("pdf");
        }

        [Fact]
        public void can_get_a_random_exception()
        {
            var exe = system.Exception();

            exe.Dump();

            var exe2 = system.Exception();

            exe2.Dump();
        }

        [Fact]
        public void can_get_a_random_extension_of_an_unknown_mimetype()
        {
            system.FileExt("aaa/bbb").Should().Be("pfb");
            system.FileExt("aaa/bbb").Should().Be("eps");
            system.FileExt("aaa/bbb").Should().Be("cmx");
        }

        [Fact]
        public void can_get_a_random_system_version()
        {
            var ver = system.Version();

            var numbers = ver.ToString().Split('.');

            numbers.TrueForAll(x => x.ToInt32() >= 0 && x.ToInt32() <= 9)
                .Should().BeTrue();
        }

        [Fact]
        public void can_get_random_file_type()
        {
            system.FileType().Should().Be("multipart");
            system.FileType().Should().Be("audio");
            system.FileType().Should().Be("video");
        }

        [Fact]
        public void can_get_soem_random_mime_types()
        {
            system.MimeType().Should().Be("application/vnd.uoml+xml");
            system.MimeType().Should().Be("application/nss");
            system.MimeType().Should().Be("audio/x-aiff");
        }

        [Fact]
        public void can_get_some_common_file_types()
        {
            system.CommonFileType().Should().Be("text");
            system.CommonFileType().Should().Be("video");
            system.CommonFileType().Should().Be("application");
        }

        [Fact]
        public void can_get_some_common_random_file_names()
        {
            system.CommonFileName().Should().Be("soft_deposit.gif");
            system.CommonFileName().Should().Be("bedfordshire_directives_pixel.pdf");
            system.CommonFileName().Should().Be("lead_transmitting_methodology.gif");
        }

        [Fact]
        public void can_get_some_random_extensions()
        {
            system.FileExt("image/jpeg").Should().Be("jpg");
            system.FileExt("image/jpeg").Should().Be("jpeg");
            system.FileExt("image/jpeg").Should().Be("jpe");
            system.FileExt("image/jpeg").Should().Be("jpg");
        }

        [Fact]
        public void can_get_some_random_file_names()
        {
            system.FileName().Should().Be("soft_deposit.prc");
            system.FileName().Should().Be("liberian_dollar.xpr");
            system.FileName().Should().Be("handmade_rubber_computer_handcrafted_frozen_chair_transmitting.kwd");
        }

        [Fact]
        public void merge_test()
        {
            system.GetObject("mimeTypes").Properties()
                .Select(p => p.Name.Substring(0, p.Name.IndexOf('/')))
                .Distinct()
                .ToArray().Dump();
        }
    }
}
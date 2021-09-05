using System.IO;

namespace EmailService.Web.Logic
{
    public class TestHTMLBuilder : HTMLBuilder
    {
        protected string Name { get; private set; }

        public TestHTMLBuilder(string relativePath)
            : base(relativePath)
        {
            HTMLContent = File.ReadAllText($@"{RelativePath}\files\email\test-email.html");
            HTMLStyle = File.ReadAllText($@"{RelativePath}\files\email\test-email-style.html");
        }

        protected override void ExecuteBuildContent()
        {
            HTMLContent = HTMLContent.Replace("<css-placeholder/>", HTMLStyle);
            HTMLContent = HTMLContent.Replace("<name-placeholder/>", Name);

            Content = HTMLContent;
        }

        public TestHTMLBuilder SetName(string name)
        {
            Name = name;

            return this;
        }
    }
}

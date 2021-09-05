using System.IO;

namespace EmailService.Web.Logic
{
    public class ProjectHTMLBuilder : HTMLBuilder
    {
        private readonly string _urlHttpHandler;

        protected string Name { get; private set; }

        public ProjectHTMLBuilder(string relativePath, string urlHttpHandler)
            : base(relativePath)
        {
            HTMLContent = File.ReadAllText($@"{RelativePath}\files\email\mailinglist\project-email.html");
            HTMLStyle = File.ReadAllText($@"{RelativePath}\files\email\mailinglist\project-email-style.html");
            _urlHttpHandler = urlHttpHandler;
        }

        protected override void ExecuteBuildContent()
        {
            HTMLContent = HTMLContent.Replace("<css-placeholder/>", HTMLStyle);
            HTMLContent = HTMLContent.Replace("{url_HttpHandler}", _urlHttpHandler);

            Content = HTMLContent;
        }

    }
}

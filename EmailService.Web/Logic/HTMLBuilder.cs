using System;
using System.Linq;

namespace EmailService.Web.Logic
{
    public abstract class HTMLBuilder
    {
        private string _relativePath;


        public string Content { get; set; }


        public string RelativePath { get => _relativePath; set { _relativePath = $@"{value}\wwwroot"; } }
        protected string HTMLContent { get; set; }
        protected string HTMLStyle { get; set; }


        public HTMLBuilder(string relativePath)
        {
            RelativePath = relativePath;
        }


        public void BuildContent()
        {
            // Check if all properties in base class are set except for the Content property
            if (GetType().GetProperties().All(p => p.GetValue(this) != null || p.Name == "Content"))
            {
                ExecuteBuildContent();
            }
            else
            {
                throw new MissingFieldException("Not all replacement properties of this HTMLBuilder have been set");
            }
        }

        protected abstract void ExecuteBuildContent();


    }
}

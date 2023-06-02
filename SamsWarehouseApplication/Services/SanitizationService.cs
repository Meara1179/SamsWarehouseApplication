using Ganss.Xss;

namespace SamsWarehouseApplication.Services
{
    public class SanitizationService
    {
        public HtmlSanitizer Sanitizer { get; set; }

        public SanitizationService()
        {
            if (Sanitizer == null)
            {
                Sanitizer = new HtmlSanitizer();
                Sanitizer.AllowDataAttributes = true;
                Sanitizer.AllowedAttributes.Add("class");
            }
        }
    }
}

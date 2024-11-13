using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blossom_Utilities
{
    public class EmailTemplateHelper
    {
        public static string GetEmailBody(string templatePath, Dictionary<string, string> placeholders)
        {
            if (!File.Exists(templatePath))
                throw new FileNotFoundException("Email template not found.");

            string body = File.ReadAllText(templatePath);

            foreach (var placeholder in placeholders)
            {
                body = body.Replace("[[${" + placeholder.Key + "}]]", placeholder.Value);
            }

            return body;
        }
    }
}

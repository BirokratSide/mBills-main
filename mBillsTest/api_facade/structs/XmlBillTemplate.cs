using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace mBillsTest.api_facade.structs
{
    public class XmlBillTemplate
    {
        public static StringContent BillToStringContent(string base64bill) {
            string stringContent = $@"
-----BOUNDARY
Content-Disposition: form-data; name=""document[file]""; filename=""racun1.xml""
Content-Type: application/xml
Content-Transfer-Encoding: base64

{base64bill}
-----BOUNDARY--
            ".Trim();
            StringContent content = new StringContent(stringContent, System.Text.Encoding.Default);
            content.Headers.Remove("Content-Type");
            content.Headers.TryAddWithoutValidation("Content-Type", "multipart/form-data; boundary=" + "---BOUNDARY");
            return content;
        }
    }
}

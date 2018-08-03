using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using TRex.Metadata;

namespace CSVtoJSON.Controllers
{
    public class CSVtoJSONController : ApiController
    {
        /// <summary>
        /// Convert CSV to JSON where the first row is headers
        /// </summary>
        /// <param name="body">CSV file to convert to JSON</param>
        /// <returns>JSON Result - the JArray of Objects generated from each row</returns>
        [Swashbuckle.Swagger.Annotations.SwaggerResponse(HttpStatusCode.OK, Type = typeof(List<object>))]
        [Metadata("CSV to JSON with header row", "Convert CSV to JSON")]
        public HttpResponseMessage Post([FromBody] string body)
        {
            List<object> resultSet = new List<object>();
            string[] csvLines = body.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            var headers = csvLines[0].Split(',').ToList<string>();
            foreach (var line in csvLines.Skip(1))
            {
                var lineObject = new JObject();
                if (!string.IsNullOrEmpty(line.Trim()))
                {
                    var lineAttr = line.Split(',');

                    for (int x = 0; x < Math.Min(headers.Count(), lineAttr.Count()); x++)
                    {
                        lineObject[headers[x].Trim()] = lineAttr[x].Trim();
                    }

                    resultSet.Add(lineObject);
                }
            }

            return Request.CreateResponse<List<object>>(resultSet);
        }

    }
}


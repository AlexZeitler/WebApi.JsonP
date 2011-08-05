using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using Microsoft.ApplicationServer.Http.Dispatcher;

namespace WebApi.JsonP {
	public class JsonpResponseHandler : HttpOperationHandler<HttpResponseMessage, HttpResponseMessage> {
		public JsonpResponseHandler()
			: base("response") {
		}

		public override HttpResponseMessage OnHandle(HttpResponseMessage response) {
			var accept = response.RequestMessage.Headers.Accept;
			if (accept.Contains(new MediaTypeWithQualityHeaderValue("application/json"))) {
				var sb = new System.Text.StringBuilder();
				var queryString = HttpUtility.ParseQueryString(response.RequestMessage.RequestUri.Query);
				if (!string.IsNullOrEmpty(queryString["callback"])) {
					var callback = queryString["callback"];
					sb.Append(callback + "(" + response.Content.ReadAsString() + ")");
					response.Content = new StringContent(sb.ToString());
					response.Content.Headers.Clear();
					response.Content.Headers.AddWithoutValidation("Content-Type", "application/json");
				}
			}

			return response;
		}
	}
}
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Routing;

namespace Subscription.API.Infrastructure.ActionResult
{
    public class PagedResponseActionResult: IHttpActionResult
    {
        private HttpRequestMessage _request;
        private long TotalCount { get; set; }
        private string NextPageUrl { get; set; }
        private string PrevPageUrl { get; set; }
        private dynamic Results { get; set; }
        private bool IsParentTranslated { get; set; }
        private UrlHelper _url;
        public PagedResponseActionResult(HttpRequestMessage request, string routeName, int currentPage, int pageSize, long totalCount, dynamic results, bool isParentTranslated)
        {
            _request = request;
            Results = results;

            _url = new UrlHelper(request);
            var routeValues = request.GetQueryNameValuePairs().ToDictionary(kv => kv.Key, kv => kv.Value, StringComparer.OrdinalIgnoreCase);

            if (!routeValues.Keys.Contains("page"))
            {
                routeValues.Add("page", "");
            }
            if ((currentPage * pageSize) < totalCount)
            {
                routeValues["page"] = (currentPage + 1).ToString();
                NextPageUrl = _url.Link(routeName, routeValues);

            }

            if (currentPage > 1)
            {
                routeValues["page"] = (currentPage - 1).ToString();
                PrevPageUrl = _url.Link(routeName, routeValues);
            }

            TotalCount = totalCount;
            IsParentTranslated = isParentTranslated;
        }
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {

            var response = new
            {
                TotalCount = TotalCount,
                NextPageURL = NextPageUrl,
                PrevPageURL = PrevPageUrl,
                Results = Results,
                IsParentTranslated = IsParentTranslated
            };
            return Task.FromResult(_request.CreateResponse(System.Net.HttpStatusCode.OK, response));
        }
    }
}
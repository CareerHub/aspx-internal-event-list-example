using Event_List_Example.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

namespace Event_List_Example {
    public partial class Default : System.Web.UI.Page {

        private const string clientId = "";
        private const string secret = "";
        protected const string baseUrl = "";
        private const string queryParams = "";

        protected IEnumerable<EventModel> events;
        private static string accessToken = null;
        private static DateTime? accessTokenExpiry = null;
        

        protected void Page_Load(object sender, EventArgs ea) {
            GetEvents();
        }

        private IEnumerable<EventModel> GetEvents(bool force = false) {
            RefreshTokens();
            var client = new RestClient(baseUrl + "api/integrations/v1/events/current" + (
                queryParams != null ? $"?{queryParams}" : ""
            ));
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("authorization", "Bearer " + accessToken);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode != System.Net.HttpStatusCode.OK) {
                if(!force) {
                    RefreshTokens(true);
                    return GetEvents(true);
                }
                throw new Exception("Could not get events from CareerHub: " + response.Content);
            }
            events = JsonConvert.DeserializeObject<IEnumerable<EventModel>>(response.Content)
                .Where(e => e.IsActive)
                .Where(e => !e.IsCancelled)
                .Where(e => e.Publish < DateTime.UtcNow)
                .Where(e => e.Expire > DateTime.UtcNow);

            return events;
        }

        private void RefreshTokens(bool force = false) {
            if(accessTokenExpiry == null || accessTokenExpiry <= DateTime.UtcNow || force) {
                var client = new RestClient(baseUrl + "oauth/token");
                var request = new RestRequest(Method.POST);
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddHeader("cache-control", "no-cache");
                request.AddParameter(
                    "application/x-www-form-urlencoded",
                    BuildQueryString(new Dictionary<string, string> {
                        { "grant_type", "client_credentials" },
                        { "client_id", clientId },
                        { "client_secret", secret },
                        { "scope", "Integrations.Events" }
                    }),
                    ParameterType.RequestBody
                );
                IRestResponse response = client.Execute(request);
                if(response.StatusCode != System.Net.HttpStatusCode.OK) {
                    throw new Exception("Could not authenticate with CareerHub: " + response.Content);
                }

                var responseBody = JsonConvert.DeserializeObject<OAuthResponse>(response.Content);
                accessToken = responseBody.access_token;

                if(responseBody.expires_in.HasValue) {
                    accessTokenExpiry = DateTime.UtcNow.AddSeconds(responseBody.expires_in.Value);
                } else {
                    accessTokenExpiry = null;
                }
            }
        }

        private string BuildQueryString(Dictionary<string, string> parameters) {
            var queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
            foreach(var param in parameters) {
                queryString[param.Key] = param.Value;
            }
            return queryString.ToString();
        }
    }
}
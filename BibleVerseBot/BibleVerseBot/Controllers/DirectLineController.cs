using BibleVerseBot.Models;
using BibleVerseBot.Models.DirectLine;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace BibleVerseBot.Controllers
{
    [RoutePrefix("api/directline")]
    public class DirectLineController : ApiController
    {
        [Route("{conversationId}")]
        [HttpGet]
        public async Task<Activity> GetMessage(string conversationId)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("BotConnector", Constants.DirectLineKey);
                var response = await httpClient.GetAsync($"https://directline.botframework.com/api/conversations/{conversationId}/messages");
                return await response.Content.ReadAsAsync<Activity>();
            }
            return null;
        }

        [Route("{conversationId}")]
        [HttpPost]
        public async Task PostMessage(string conversationId, Message message)
        {
            using (var httpClient = new HttpClient())
            {
                message.created = DateTime.Now;
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("BotConnector", Constants.DirectLineKey);
                var response = await httpClient.PostAsJsonAsync<Message>("https://directline.botframework.com/api/conversations/" + conversationId + "/messages", message);
                await response.Content.ReadAsAsync<Conversation>();
            }
        }

        [Route("")]
        [HttpPost]
        public async Task<Conversation> Post([FromBody]Activity activity)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("BotConnector", Constants.DirectLineKey);
                var response = await httpClient.PostAsync("https://directline.botframework.com/api/conversations", new StringContent(String.Empty));
                return await response.Content.ReadAsAsync<Conversation>();
            }
            return null;
        }
    }
}

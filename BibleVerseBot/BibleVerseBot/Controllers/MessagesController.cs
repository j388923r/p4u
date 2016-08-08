using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using Microsoft.Bot.Builder.Luis.Models;
using BibleVerseBot.Models;

namespace BibleVerseBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));

                LuisResult  result = JsonConvert.DeserializeObject<LuisResult>(await connector.HttpClient.GetStringAsync(""));

                BibleVerse verse = new BibleVerse();
                foreach (EntityRecommendation entity in result.Entities)
                {
                    if (entity.Entity.Equals("Book"))
                    {
                        verse.Book = verse.Book ?? String.Empty + entity.Entity;
                    }
                    else if (entity.Entity.Equals("Chapter"))
                    {
                        verse.Chapter = int.Parse(entity.Entity);
                    }
                    else if (entity.Entity.Equals("Verse"))
                    {
                        if (verse.StartVerse == 0)
                        {
                            verse.StartVerse = int.Parse(entity.Entity);
                        }
                        else
                        {
                            verse.EndVerse = int.Parse(entity.Entity);
                        }
                    }
                }

                //connector.HttpClient.

                await connector.Conversations.ReplyToActivityAsync(reply);
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}
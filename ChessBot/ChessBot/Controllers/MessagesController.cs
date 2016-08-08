using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using ChessBot.Models;
using System.Collections.Generic;

namespace ChessBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        protected ChessBoard board = new ChessBoard();

        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<Activity> Post([FromBody]Activity message)
        {
            if (message.Type == ActivityTypes.Message)
            {
                ConnectorClient connector = new ConnectorClient(new Uri(message.ServiceUrl));
                StateClient stateClient = message.GetStateClient();
                BotData storedData = await stateClient.BotState.GetConversationDataAsync(message.ChannelId, message.Conversation.Id);

                string[] locs = message.Text.Split(' ');
                if (locs.Length == 2 && ChessBoard.isValidChessLocation(locs[1]))
                {
                    string checkString = String.Empty;
                    GameState state = storedData.GetProperty<GameState>("gameState") ?? new GameState();
                    state.currentBoard = state.currentBoard ?? ChessBoard.InitialBoard;
                    state.lastTenMoves = state.lastTenMoves ?? new List<string>();
                    if (!ChessBoard.isOccupiedLocation(ChessBoard.convertAlgebraicToPoint(locs[0]), state))
                    {
                        Activity replyError = message.CreateReply("First space not occupied.");
                        await connector.Conversations.ReplyToActivityAsync(replyError);
                    } else if (!ChessBoard.isMoveYours(locs[0], state))
                    {
                        Activity replyError = message.CreateReply("It's not your turn.");
                        await connector.Conversations.ReplyToActivityAsync(replyError);
                    } else if (!ChessBoard.isMoveLegal(locs[0], locs[1], state))
                    {
                        Activity replyError = message.CreateReply("Move is not legal.");
                        await connector.Conversations.ReplyToActivityAsync(replyError);
                    } else
                    {
                        board.makeMove(locs[0], locs[1], state);
                        if (state.blackTurn && ChessBoard.isBlackInCheckBy(state, locs[1])) {
                            checkString = "Check!";
                        } else if (!state.blackTurn && ChessBoard.isWhiteInCheckBy(state, locs[1])) {
                            checkString = "Check!";
                        }
                    }

                    // return our reply to the user
                    Activity reply = message.CreateReply(state.ToString());
                    storedData.SetProperty<GameState>("gameState", state);
                    await connector.Conversations.ReplyToActivityAsync(reply);
                } else if (message.Text.EndsWith("board")) {
                    GameState state = storedData.GetProperty<GameState>("gameState") ?? new GameState();
                    state.currentBoard = state.currentBoard ?? ChessBoard.InitialBoard;
                    state.lastTenMoves = state.lastTenMoves ?? new List<string>();
                    
                    Activity reply = message.CreateReply(state.ToBoardString());
                    await connector.Conversations.ReplyToActivityAsync(reply);
                } else if (message.Text.EndsWith("list")) {
                    GameState state = storedData.GetProperty<GameState>("gameState") ?? new GameState();
                    state.currentBoard = state.currentBoard ?? ChessBoard.InitialBoard;
                    state.lastTenMoves = state.lastTenMoves ?? new List<string>();
                    
                    Activity reply = message.CreateReply(state.ToListString());
                    await connector.Conversations.ReplyToActivityAsync(reply);
                } else
                {
                    Activity replyError = message.CreateReply("Invalid chess move");
                    await connector.Conversations.ReplyToActivityAsync(replyError);
                }
                return null;
            }
            else
            {
                return HandleSystemMessage(message);
            }
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.Ping)
            {
                Activity reply = message.CreateReply();
                reply.Type = ActivityTypes.Ping;
                return reply;
            }
            else if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == "BotAddedToConversation")
            {
            }
            else if (message.Type == "BotRemovedFromConversation")
            {
            }
            else if (message.Type == "UserAddedToConversation")
            {
            }
            else if (message.Type == ActivityTypes.Typing)
            {
            }

            return null;
        }
    }
}
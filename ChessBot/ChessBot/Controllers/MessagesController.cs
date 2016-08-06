using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Utilities;
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
        public async Task<Message> Post([FromBody]Message message)
        {
            if (message.Type == "Message")
            {
                string[] locs = message.Text.Split(' ');
                if (locs.Length == 2 && ChessBoard.isValidChessLocation(locs[1]))
                {
                    string checkString = String.Empty;
                    string attachments = message.BotConversationData != null ? message.BotConversationData.ToString() : "{}";
                    GameState state = JsonConvert.DeserializeObject<GameState>(attachments);
                    state.currentBoard = state.currentBoard ?? ChessBoard.InitialBoard;
                    state.lastTenMoves = state.lastTenMoves ?? new List<string>();
                    if (!ChessBoard.isOccupiedLocation(ChessBoard.convertAlgebraicToPoint(locs[0]), state))
                    {
                        return message.CreateReplyMessage("First space not occupied.");
                    } else if (!ChessBoard.isMoveYours(locs[0], state))
                    {
                        return message.CreateReplyMessage("It's not your turn.");
                    } else if (!ChessBoard.isMoveLegal(locs[0], locs[1], state))
                    {
                        return message.CreateReplyMessage("Move is not legal.");
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
                    Message m = message.CreateReplyMessage(state.ToString());
                    m.BotConversationData = JsonConvert.SerializeObject(state);
                    return m;
                } else if (message.Text.EndsWith("board")) {
                    string attachments = message.BotConversationData != null ? message.BotConversationData.ToString() : "{}";

                    GameState state = JsonConvert.DeserializeObject<GameState>(attachments);
                    state.currentBoard = state.currentBoard ?? ChessBoard.InitialBoard;
                    state.lastTenMoves = state.lastTenMoves ?? new List<string>();

                    Message m = message.CreateReplyMessage(state.ToBoardString());
                    m.BotConversationData = JsonConvert.SerializeObject(state);
                    return m;
                } else if (message.Text.EndsWith("list")) {
                    string attachments = message.BotConversationData != null ? message.BotConversationData.ToString() : "{}";

                    GameState state = JsonConvert.DeserializeObject<GameState>(attachments);
                    state.currentBoard = state.currentBoard ?? ChessBoard.InitialBoard;
                    state.lastTenMoves = state.lastTenMoves ?? new List<string>();

                    Message m = message.CreateReplyMessage(state.ToListString());
                    m.BotConversationData = JsonConvert.SerializeObject(state);
                    return m;
                } else
                {
                    return message.CreateReplyMessage("Invalid chess move");
                }
            }
            else
            {
                return HandleSystemMessage(message);
            }
        }

        private Message HandleSystemMessage(Message message)
        {
            if (message.Type == "Ping")
            {
                Message reply = message.CreateReplyMessage();
                reply.Type = "Ping";
                return reply;
            }
            else if (message.Type == "DeleteUserData")
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
            else if (message.Type == "UserRemovedFromConversation")
            {
            }
            else if (message.Type == "EndOfConversation")
            {
            }

            return null;
        }
    }
}
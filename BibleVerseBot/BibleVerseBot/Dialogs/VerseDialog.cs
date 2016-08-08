using BibleVerseBot.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BibleVerseBot.Dialogs
{
    [LuisModel("221a829e-f8fa-46ae-a077-97b25b0a2671", "ee1e895d964348cf85d5fae036493173")]
    [Serializable]
    public class VerseDialog : LuisDialog<string>
    {


        [LuisIntent("BibleVerse")]
        public void getVerse(IDialogContext content, LuisResult result)
        {
            BibleVerse verse = new BibleVerse();
            foreach ( EntityRecommendation entity in result.Entities)
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
        }
    }
}
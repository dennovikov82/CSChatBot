﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSChatBot.Helpers;
using DB;
using DB.Extensions;
using DB.Models;
using ModuleFramework;
using Telegram.Bot;

namespace CSChatBot.Modules
{
    [Module(Author = "parabola949", Name = "Admin", Version = "1.0")]
    class Admin
    {
        public Admin(Instance instance, Setting setting, TelegramBotClient bot)
        {

        }

        [ChatCommand(Triggers = new[] {"sql"}, DevOnly = true)]
        public static CommandResponse RunSql(CommandEventArgs args)
        {
            return new CommandResponse($"{args.DatabaseInstance.ExecuteNonQuery(args.Parameters)} records changed");
        }

        [ChatCommand(Triggers = new[] { "cleandb", }, DevOnly = true, HelpText = "Cleans all users with UserID (0)")]
        public static CommandResponse CleanDatabase(CommandEventArgs args)
        {
            var start = args.DatabaseInstance.Users.Count();
            args.DatabaseInstance.ExecuteNonQuery("DELETE FROM USERS WHERE UserId = 0");
            var end = args.DatabaseInstance.Users.Count();
            return new CommandResponse($"Database cleaned. Removed {start - end} users.");
        }

        #region Chat Commands

        [ChatCommand(Triggers = new[] { "addbotadmin", "addadmin" }, DevOnly = true)]
        public static CommandResponse AddBotAdmin(CommandEventArgs args)
        {
            var target = UserHelper.GetTarget(args);
            if (target != null && target.ID != args.SourceUser.ID)
            {
                target.IsBotAdmin = true;
                return new CommandResponse($"{target.Name} is now a bot admin.");
            }
            return new CommandResponse(null);
        }
        
        [ChatCommand(Triggers = new[] { "rembotadmin", "remadmin" }, DevOnly = true)]
        public static CommandResponse RemoveBotAdmin(CommandEventArgs args)
        {
            var target = UserHelper.GetTarget(args);
            if (target != null && target.ID != args.SourceUser.ID)
            {
                target.IsBotAdmin = false;
                return new CommandResponse($"{target.Name} is no longer a bot admin.");
            }
            return new CommandResponse(null);
        }
        #endregion
    }
}
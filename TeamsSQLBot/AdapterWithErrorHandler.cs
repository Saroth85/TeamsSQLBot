﻿using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Extensions.Logging;

/// <summary>
/// AdapterWithErrorHandler
/// </summary>
public class AdapterWithErrorHandler : CloudAdapter
{
    public AdapterWithErrorHandler(BotFrameworkAuthentication auth, ILogger<IBotFrameworkHttpAdapter> logger, ConversationState conversationState = default)
        : base(auth, logger)
    {
        OnTurnError = async (turnContext, exception) =>
        {
             logger.LogError(exception, $"[OnTurnError] unhandled error : {exception.Message}");

            // Send a message to the user
            await turnContext.SendActivityAsync("The bot encountered an error or bug.");
            await turnContext.SendActivityAsync("To continue to run this bot, please fix the bot source code.");

            if (conversationState != null)
            {
                try
                {
                    await conversationState.DeleteAsync(turnContext);
                }
                catch (Exception e)
                {
                    logger.LogError(e, $"Exception caught on attempting to Delete ConversationState : {e.Message}");
                }
            }
        };
    }
}

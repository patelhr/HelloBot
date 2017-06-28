using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Google.Apis.Customsearch.v1;
using Google.Apis.Services;
using System.Collections;
using Google.Apis.Customsearch.v1.Data;
using System.Collections.Generic;
using StackExchange.StacMan;
using HelloBot.Utility;

namespace HelloBot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        List<string> apiKeys = new List<string>();
        int count = 0;
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            apiKeys.Add("AIzaSyCrAa9mqau347-VCwIq0gtYkxYAP0DKWZo");
            apiKeys.Add("AIzaSyBQTsyJNFu55dvE5IBs4J3J494EhatFjPw");
            apiKeys.Add("AIzaSyAj8DEaTYyejw1ALpT3Q0pY4T1IMAlYWpE");
            apiKeys.Add("AIzaSyDZQZu3QieNf-crAbNW_fHrcXBb3u0mKHM");
            apiKeys.Add("AIzaSyB5U7X2PHPak-kcgWw361KJ8MzsQQL0Spo");
            apiKeys.Add("AIzaSyAaDTGMi9lRbVpHLkyi8SaD8QimCgCSwB0");
            apiKeys.Add("AIzaSyBc7KR315h3mDVTPTr8vKSr-ejZDY-roko");
            apiKeys.Add("AIzaSyA_OVhGFLhcgx8VViBNRcF_sM1bZq1cvwE");
            apiKeys.Add("AIzaSyBlCPi4GEylOB7NyczytLbLES9l9fFrbRM");
            apiKeys.Add("AIzaSyCoL6YhY62qSj0JJWkTvYUEY2XlBoGBVAM");

            var key = new Keys();
            int linkCounter = 5;
            var activity = await result as Activity;
            activity.Text = activity.Text ?? string.Empty;
            try
            {
                if (count >= 10)
                {
                    count = 1;
                }
                string apiKey;
                apiKey = apiKeys[count++];
                string query = activity.Text;
                var customSearchService = new CustomsearchService(new BaseClientService.Initializer { ApiKey = apiKey });
                var listRequest = customSearchService.Cse.List(query);
                listRequest.Cx = key.searchID.ToString();
                IList<Result> pagging = new List<Result>();
                while (pagging != null)
                {
                    pagging = listRequest.Execute().Items;
                    if (pagging != null)
                    {
                        foreach (var item in pagging)

                            if (linkCounter > 0)
                            {
                                await context.SayAsync(item.Link);
                                linkCounter--;
                            }
                    }
                    else
                        await context.SayAsync("Please enter valid input");
                }
            }
            catch (Exception)
            {
                await context.SayAsync("Oops! I have fight with google please try again.");
                apiKeys.Reverse();
                count=0;
            }
        }
    }
}
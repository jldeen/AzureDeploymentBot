using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace TestBot2
{
    
    [Serializable]
    public class AzureChain
    {
        public static IEnumerable<string> Choices = new List<string>() { "Windows", "Linux" };


        public static readonly IDialog<string> dialog = Chain.PostToChain()
                    .Select(msg => msg.Text)
                    .Switch(
                        new Case<string, IDialog<string>>(text =>
                        {
                            var regex = new Regex("^build");
                            return regex.Match(text).Success;
                        }, (context, txt) =>
                        {
                            return Chain.From(() => new PromptDialog.PromptChoice<string>(Choices, "What type of VM would you want to spin up?",
                    "Didn't get that.", 1, PromptStyle.Auto)).ContinueWith(async (ctx, res) =>
                                {
                                    string reply = "complete ";
                                    var text = await res;
                                    context.UserData.SetValue("OS", text);
                                    return Chain.ContinueWith(new PromptDialog.PromptString("Name of ResourceGroup", "didn't get name", 1), 
                                        async (ctx2, res2) =>
                                    {
                                        var result = await res2;
                                        var OperatingSys = "none";
                                        //context.UserData.TryGetValue("OS", out OperatingSys);
                                        return Chain.Return(reply);
                                    });
                                    //var regex = new Regex("^Windows");
                                    //if (regex.Match(text).Success)
                                    //{
                                    //    context.UserData.SetValue("count", 0);
                                    //    reply = "Reset count.";
                                    //}
                                    //else
                                    //{
                                    //    if (regex.Match(text).Success)
                                    //    {
                                    //        reply = "Did not reset count.";
                                    //    }
                                    //    else
                                    //    {
                                    //        reply = "Did not get it.";
                                    //    }
                                    //}
                                    //return Chain.Return(reply);
                                });
                        }),
                        new RegexCase<IDialog<string>>(new Regex("^help", RegexOptions.IgnoreCase), (context, txt) =>
                        {
                            return Chain.Return("I am a simple echo dialog with a counter! Reset my counter by typing \"reset\"!");
                        }),
                        new DefaultCase<string, IDialog<string>>((context, txt) =>
                        {
                            int count;
                            context.UserData.TryGetValue("count", out count);
                            context.UserData.SetValue("count", ++count);
                            string reply = string.Format("{0}: You said {1}", count, txt);
                            return Chain.Return(reply);
                        }))
                    .Unwrap()
                    .PostToUser();
    }
}
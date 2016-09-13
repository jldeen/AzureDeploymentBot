using Autofac;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Connector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace TestBot2
{
    public class RequestOSClass
    {
        private string OSname;
        private string RGname;
        private string DeployName;
        private string Region; 

        // A read-write instance property:
        public string OSType
        {
            get { return OSname; }
            set { OSname = value; }
        }

        public string ResourceGroup
        {
            get { return RGname; }
            set { RGname = value; }
        }
        public string DeploymentName
        {
            get { return DeployName; }
            set { DeployName = value; }
        }

        public string RegionName
        {
            get { return Region; }
            set { Region = value; }
        }

    }
    [Serializable]
    public class AzureChain2
    {
        public static IEnumerable<string> Regions = new List<string>() { "West US", "Central US" };

        public static IEnumerable<string> Choices = new List<string>() { "Windows", "Linux" };

        [NonSerialized()]
        public static RequestOSClass test = new RequestOSClass();

        public static readonly IDialog<string> dialog = Chain.PostToChain()
                    .Select(msg => msg.Text)
                    .Switch(
                        new Case<string, IDialog<string>>(text =>
                        {
                            var regex = new Regex("^build");
                            return regex.Match(text).Success;
                        }, (context, txt) =>
                        {
                            return Chain.From(() => new PromptDialog.PromptChoice<string>(new[] { "West US", "Central US"}, "What type of VM would you want to spin up?",
                                "Didn't get that.", 1, PromptStyle.Auto)).ContinueWith(async (ctx, res) =>
                        {
                        string reply = "complete ";
                        var text = await res;

                            test.OSType = text; 
                            return Chain.ContinueWith(new PromptDialog.PromptString("Name of ResourceGroup", "didn't get name", 1),
                            async (ctx2, res2) =>
                            {
                                //var osdata = botData.GetProperty<string>("OS");
                                var result = await res2;
                                test.ResourceGroup = result;
                                return Chain.ContinueWith<string, string>(new PromptDialog.PromptChoice<string>(Regions, "Where would you like to spin up this VM?", "Didn't get that", 1, PromptStyle.None),
                                    async (ctx3, res3) => {
                                        var result2 = await res3;
                                        test.RegionName = result2; 
                                        return Chain.Return(test.OSType + " " + test.ResourceGroup + " " + test.RegionName);
                                    });
                            });
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
                            string reply = string.Format("{0}: You said {1} with {2}", count, txt, test.OSType);

                            return Chain.Return(reply);
                        }))
                    .Unwrap()
                    .PostToUser();
    }
}
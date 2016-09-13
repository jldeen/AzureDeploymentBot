using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace TestBot2
{
    [Serializable]
    public class AzureChat : IDialog<object>
    {
            protected int count = 1;

            public async Task StartAsync(IDialogContext context)
            {
                context.Wait(MessageReceivedAsync);
            }

            public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
            {
                var message = await argument;
                if (message.Text == "Build")
                {
                    
                    PromptDialog.Confirm(
                        context,
                        AfterResetAsync,
                        "Are you sure you want to reset the count?",
                        "Didn't get that!",
                        promptStyle: PromptStyle.None);
                }
                else
                {
                    await context.PostAsync(string.Format("{0}: You said {1}", this.count++, message.Text));
                    context.Wait(MessageReceivedAsync);
                }
            }

            public async Task AfterResetAsync(IDialogContext context, IAwaitable<bool> argument)
            {
                var confirm = await argument;
                if (confirm)
                {
                    this.count = 1;
                    await context.PostAsync("Reset count.");
                }
                else
                {
                    await context.PostAsync("Did not reset count.");
                }
                context.Wait(MessageReceivedAsync);
            }

        Task IDialog<object>.StartAsync(IDialogContext context)
        {
            throw new NotImplementedException();
        }
    }
}
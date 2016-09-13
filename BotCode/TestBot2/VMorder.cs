using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.FormFlow;

namespace TestBot2
{
    public enum OSOptions
    {
        Windows, Linux
    };
    public enum LocationOptions { West, Central };


    [Serializable]
    public class VMorder
    {
        public OSOptions? OS;
        public LocationOptions? Location;


        public static IForm<VMorder> BuildForm()
        {
            return new FormBuilder<VMorder>()
                    .Message("Welcome to the simple sandwich order bot!")
                    .Build();
        }
    };
}

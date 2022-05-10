using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AndroidApp.Helpers
{
    public class ReadTagHandler : Handler
    {
        private readonly ITagIdHandler _handler;
        [Obsolete]
        public ReadTagHandler(ITagIdHandler hander)
        {
            _handler = hander;
        }
        public override void HandleMessage(Message msg)
        {
            try
            {
                string result = msg.Obj.ToString();
                _handler.AddTagIdToList(result);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }
    }
}
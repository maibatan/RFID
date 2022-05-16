using Android.App;
using Android.OS;
using AndroidApp.Helpers;
using AndroidApp.Interfaces;
using Com.Rscja.Deviceapi;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AndroidApp.Services
{
    public class RFIDService
    {

        #region Properties
        public static bool IsRun => _loopFlag;
        #endregion
        #region Fields
        private static bool _loopFlag = false;
        [Obsolete]
        private static readonly RFIDWithUHF _mReader = RFIDWithUHF.Instance;
        #endregion
        #region Methods
        [Obsolete]
        public  static Task InitializeReader()
        {
           return Task.Factory.StartNew(() =>
            {
                for (var i = 1; i <= 3; i++)
                {
                    if (i != 3)
                    {
                        if (!_mReader.Init())
                        {
                            _mReader.Free();
                        }
                    }
                    else
                    {
                        var result = _mReader.Init();
                        if (!result)
                        {
                            Console.WriteLine("Init Fail");
                        }
                        else Console.WriteLine("Init Complete");
                        _mReader.SetEPCTIDMode(true);
                    }
                }
                
            });

        }
        [Obsolete]
        public static void StartScan(ITagIdHandler tagIdHandler)
        {
            byte q = 0;
            byte anti = 0;
            if (!_loopFlag)
            {
                
                _loopFlag = true;
                _mReader.StartInventoryTag(anti, q);
                ContinuousRead(tagIdHandler);
            }
            else
            {
                _loopFlag = false;
                _mReader.StopInventory();
                
            }
        }

        [Obsolete]
        public static void Stop()
        {
            _loopFlag = false;
            _mReader.StopInventory();           
        }
        [Obsolete]
        private static void ContinuousRead(ITagIdHandler tagIdHandler)
        {
            ReadTagHandler _handler = new ReadTagHandler(tagIdHandler);
            Thread th = new Thread(new ThreadStart(delegate
            {
                
                List<string> _handledTag = new List<string>();
                while (_loopFlag)
                {
                    string[] res = _mReader.ReadTagFromBuffer();
                    if (res != null)
                    {
                        Message msg = _handler.ObtainMessage();
                        StringBuilder sb = new StringBuilder();
                        sb.Append(res[0].Substring(res[0].Length - 9));                       
                        if (!_handledTag.Contains(sb.ToString()))
                        {
                            _handledTag.Add(sb.ToString());
                            msg.Obj = sb.ToString();
                            _handler.SendMessage(msg);
                        }
                    }
                }
            }));
            th.Start();
        }
        #endregion

    }
}
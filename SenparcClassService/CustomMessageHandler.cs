﻿using Senparc.NeuChar;
using Senparc.NeuChar.Entities;
using Senparc.NeuChar.Helpers;
using Senparc.Weixin;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AppStore;
//using Senparc.Weixin.MP.AppStore;
using Senparc.Weixin.MP.Containers;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.MessageHandlers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SenparcClassService
{
    public partial class CustomMessageHandler : MessageHandler<CustomMessageContext>
    {
        public CustomMessageHandler(Stream inputStream, PostModel postModel, int maxRecordCount = 0, DeveloperInfo developerInfo = null) : base(inputStream, postModel, maxRecordCount, developerInfo)
        {
        }

        public CustomMessageHandler(XDocument requestDocument, PostModel postModel, int maxRecordCount = 0, DeveloperInfo developerInfo = null) : base(requestDocument, postModel, maxRecordCount, developerInfo)
        {
        }

        public CustomMessageHandler(RequestMessageBase requestMessageBase, PostModel postModel, int maxRecordCount = 0, DeveloperInfo developerInfo = null) : base(requestMessageBase, postModel, maxRecordCount, developerInfo)
        {
        }

        public override IResponseMessageBase DefaultResponseMessage(IRequestMessageBase requestMessage)
        {
            var responseMessage = requestMessage.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "当前服务器时间:" + DateTime.Now;
            return responseMessage;
        }

        //文本消息
        public override IResponseMessageBase OnTextRequest(RequestMessageText requestMessage)
        {
            //var responseMessage = requestMessage.CreateResponseMessage<ResponseMessageText>();
            //responseMessage.Content = $"你输入的是：{requestMessage.Content}";
            //return responseMessage;
            var responseMessage = requestMessage.CreateResponseMessage<ResponseMessageNews>();
            var news = new Article
            {
                Title = $"知耻而后勇!",
                Description = $"天下没有免费午餐\r\n更没有人亲自送到你手里！",
                PicUrl = "https://b-ssl.duitang.com/uploads/item/201508/12/20150812193258_XfrHF.jpeg",
                Url = "https://b-ssl.duitang.com/uploads/item/201508/12/20150812193258_XfrHF.jpeg"
            };
            responseMessage.Articles.Add(news);
            return responseMessage;
        }

        //位置消息
        public override IResponseMessageBase OnLocationRequest(RequestMessageLocation requestMessage)
        {
            var responseMessage = requestMessage.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = $"发送的位置信息：Lat-{requestMessage.Location_X},Lon-{requestMessage.Location_Y}";
            return responseMessage;
            //return base.OnLocationRequest(requestMessage);
        }

        public override IResponseMessageBase OnEvent_ClickRequest(RequestMessageEvent_Click requestMessage)
        {
            var responseMessage = requestMessage.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = $"你点击按钮：{requestMessage.EventKey}";
            return responseMessage;
            //return base.OnEvent_ClickRequest(requestMessage);
        }

        public override IResponseMessageBase OnEvent_SubscribeRequest(RequestMessageEvent_Subscribe requestMessage)
        {
            var responseMessage = requestMessage.CreateResponseMessage<ResponseMessageText>();
            //var acessToken = AccessTokenContainer.GetAccessToken(Senparc.Weixin.Config.SenparcWeixinSetting.WeixinAppId);
            //var userInfo = Senparc.Weixin.MP.AdvancedAPIs.UserApi.Info(acessToken, base.OpenId);
            var userInfo = Senparc.Weixin.MP.AdvancedAPIs.UserApi.Info(Senparc.Weixin.Config.SenparcWeixinSetting.WeixinAppId, base.OpenId);
            var sex = userInfo.sex == 1 ? "先生" : (userInfo.sex == 2 ? "女生" : "");
            responseMessage.Content = $"{userInfo.nickname} {sex}，爱你哟,么么哒！";
            //responseMessage.Content = $"，爱你哟！";
            //var news = new Article
            //{
            //    Title = $"欢迎加入微信开发!",
            //    Description = $"天下没有免费午餐\r\n更没有人亲自送到你手里",
            //    PicUrl = "https://b-ssl.duitang.com/uploads/item/201508/12/20150812193258_XfrHF.jpeg",
            //    Url = "https://b-ssl.duitang.com/uploads/item/201508/12/20150812193258_XfrHF.jpeg"
            //};
            //responseMessage.Articles.Add(news);
            return responseMessage;
        }
        public DateTime StartTime { get; set; }

        public override void OnExecuted()
        {
            if(ResponseMessage is ResponseMessageText)
            {
                (ResponseMessage as ResponseMessageText).Content += "\r 【NEMO】";
            }
            base.OnExecuted();

            Thread.Sleep(5000);

            var runTime = (DateTime.Now - StartTime).TotalSeconds;
            if(runTime>4)
            {
                var queueHandler = new MessageQueueHandler();
                ResponseMessage = queueHandler.SendMessage(OpenId, ResponseMessage);
            }
        }

        

        public override XDocument Init(XDocument postDataDocument, IEncryptPostModel postModel)
        {
            StartTime = DateTime.Now;
            return base.Init(postDataDocument, postModel);
        }

    }
}

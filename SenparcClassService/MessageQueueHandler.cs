using Senparc.CO2NET.MessageQueue;
using Senparc.NeuChar.Entities;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SenparcClassService
{
    public class MessageQueueHandler
    {

        public IResponseMessageBase SendMessage(string openId,IResponseMessageBase responseMessage)
        {
            var messageQueue = new SenparcMessageQueue();
            if (responseMessage is ResponseMessageText)
            {
                {
                    var mqKey = SenparcMessageQueue.GenerateKey("Nemo", responseMessage.GetType(),
                    Guid.NewGuid().ToString(), "SendMessage");
                    messageQueue.Add(mqKey, () =>
                    {
                        var asyncResponseMessage = responseMessage as ResponseMessageText;
                        asyncResponseMessage.Content += "\r 这是来自客服的消息1";
                        //发送客服消息
                        CustomApi.SendText(Senparc.Weixin.Config.SenparcWeixinSetting.WeixinAppId,
                            openId, asyncResponseMessage.Content);
                    });

                }
                {
                    var mqKey = SenparcMessageQueue.GenerateKey("Nemo", responseMessage.GetType(),
                    Guid.NewGuid().ToString(), "SendMessage");
                    messageQueue.Add(mqKey, () =>
                    {
                        var asyncResponseMessage = responseMessage as ResponseMessageText;
                        asyncResponseMessage.Content += "\r 这是来自客服的消息2";
                        //发送客服消息
                        CustomApi.SendText(Senparc.Weixin.Config.SenparcWeixinSetting.WeixinAppId,
                            openId, asyncResponseMessage.Content);
                    });

                }

                return new ResponseMessageNoResponse();
            }

            return responseMessage;
        }

    }
}

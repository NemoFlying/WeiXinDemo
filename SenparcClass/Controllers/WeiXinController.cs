using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Senparc.Weixin;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP.Containers;
using Senparc.Weixin.MP.Entities.Menu;
using Senparc.Weixin.MP.Entities.Request;
using SenparcClassService;

namespace SenparcClass.Controllers
{
    public class WeiXinController : Controller
    {
        private ILog log;
        public WeiXinController(IHostingEnvironment hostingEnv)
        {
            this.log = LogManager.GetLogger(Startup.repository.Name, typeof(WeiXinController));
        }

        //public static readonly string Token = Config.SenparcWeixinSetting.Token;//与微信公众账号后台的Token设置保持一致，区分大小写。
        public static readonly string Token = Config.SenparcWeixinSetting.Token;

        /// <summary>
        /// 微信后台验证地址（使用Get），微信后台的“接口配置信息”的Url填写如：https://sdk.weixin.senparc.com/weixin
        /// </summary>
        [HttpGet]
        [ActionName("Index")]
        public ActionResult Get(PostModel postModel, string echostr)
        {
            log.Debug("---!!!!!!!!!!!!!----");
            if (CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, Token))
            {
                return Content(echostr); //返回随机字符串则表示验证通过
            }
            else
            {
                return Content("failed:" + postModel.Signature + "," + CheckSignature.GetSignature(postModel.Timestamp, postModel.Nonce, Token) + "。" +
                    "如果你在浏览器中看到这句话，说明此地址可以被作为微信公众账号后台的Url，请注意保持Token一致..。");
            }
        }

        /// <summary>
        /// 用户发送消息后，微信平台自动Post一个请求到这里，并等待响应XML。
        /// PS：此方法为简化方法，效果与OldPost一致。
        /// v0.8之后的版本可以结合Senparc.Weixin.MP.MvcExtension扩展包，使用WeixinResult，见MiniPost方法。
        /// </summary>
        [HttpPost]
        [ActionName("Index")]
        public ActionResult Post(PostModel postModel)
        {
            try
            {
                log.Debug("-------");
                //创建MessageHandler MessageHandler
                var messageHandler = new CustomMessageHandler(Request.Body, postModel);
                messageHandler.SaveRequestMessageLog();
                //执行
                messageHandler.Execute();
                messageHandler.SaveResponseMessageLog();

                //string accessToken = "Senparc20180909";
                //AccessTokenContainer.TryGetAccessToken("wxd43ee5d2b9d7873e", "8cd40916c73ba786c6682749bc5b33fe");
                //ButtonGroup bg = new ButtonGroup();

                ////单击
                //bg.button.Add(new SingleClickButton()
                //{
                //    name = "单击测试",
                //    key = "OneClick",
                //   // type = ButtonType.click.ToString(),//默认已经设为此类型，这里只作为演示
                //});

                ////二级菜单
                //var subButton = new SubButton()
                //{
                //    name = "二级菜单"
                //};
                //subButton.sub_button.Add(new SingleClickButton()
                //{
                //    key = "SubClickRoot_Text",
                //    name = "返回文本"
                //});
                //subButton.sub_button.Add(new SingleClickButton()
                //{
                //    key = "SubClickRoot_News",
                //    name = "返回图文"
                //});
                //subButton.sub_button.Add(new SingleClickButton()
                //{
                //    key = "SubClickRoot_Music",
                //    name = "返回音乐"
                //});
                //subButton.sub_button.Add(new SingleViewButton()
                //{
                //    url = "http://weixin.senparc.com",
                //    name = "Url跳转"
                //});
                //bg.button.Add(subButton);
                //var result = CommonApi.CreateMenu(accessToken, bg);

                return Content(messageHandler.FinalResponseDocument.ToString());

            }catch(Exception ex)
            {
                log.Error(ex);
                return null;
            }
            

        }






        }
}
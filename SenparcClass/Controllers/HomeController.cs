using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Senparc.CO2NET.Extensions;
using Senparc.Weixin;
using SenparcClass.Models;

namespace SenparcClass.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public ActionResult CustomMessage(string openid= "od4pQ0WIKNBPRC-to3F2ec_IRwqo")
        {
            for(var i=0;i<3;i++)
            {
                
                Senparc.Weixin.MP.AdvancedAPIs.CustomApi.SendText(
                Config.SenparcWeixinSetting.WeixinAppId, openid, "FromServer" + DateTime.Now
                );
                Thread.Sleep(500);
            }
            var result =Senparc.Weixin.MP.AdvancedAPIs.CustomApi.SendText(
                Config.SenparcWeixinSetting.WeixinAppId, openid, "FromServer" + DateTime.Now
                );

            return Content(result.ToJson());
        }

        public async  Task<ActionResult> CustomMessageAsync(string openid = "od4pQ0WIKNBPRC-to3F2ec_IRwqo")
        {
            return await Task.Factory.StartNew(async() =>
            {
                var result = await Senparc.Weixin.MP.AdvancedAPIs.CustomApi.SendTextAsync(
                   Config.SenparcWeixinSetting.WeixinAppId, openid, "FromServer" + DateTime.Now
                   );
                for (var i = 0; i < 3; i++)
                {

                    result = await Senparc.Weixin.MP.AdvancedAPIs.CustomApi.SendTextAsync(
                    Config.SenparcWeixinSetting.WeixinAppId, openid, "FromServer" + DateTime.Now
                    );
                }
                return Content(result.ToJson());
            }).Result;
            //for (var i = 0; i < 3; i++)
            //{

            //    Senparc.Weixin.MP.AdvancedAPIs.CustomApi.SendText(
            //    Config.SenparcWeixinSetting.WeixinAppId, openid, "FromServer" + DateTime.Now
            //    );
            //    Thread.Sleep(500);
            //}
            //var result = Senparc.Weixin.MP.AdvancedAPIs.CustomApi.SendText(
            //    Config.SenparcWeixinSetting.WeixinAppId, openid, "FromServer" + DateTime.Now
            //    );
        }

    }
}

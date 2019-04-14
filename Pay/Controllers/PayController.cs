using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pay.Controllers
{
    public class PayController : Controller
    {
        // GET: Pay
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult pays()
        {
            return View();
        }
        public JObject Pay(Paypayzhu payz)
        {
          
            Dictionary<string, string> remote = new Dictionary<string, string>();
            remote.Add("api_user", PayUtil.API_USER);
            remote.Add("price", payz.Price);
            remote.Add("type", payz.Type.ToString());
            remote.Add("redirect", payz.Redirect);
            //  remote.Add("order_id", PayUtil.getOrderIdByUUId());
            remote.Add("order_id", payz.Order_id);
            remote.Add("order_info", payz.Order_info);
            remote.Add("signature", PayUtil.getSignature(PayUtil.API_USER, remote));
            JObject jo = (JObject)JsonConvert.DeserializeObject(PayUtil.GetResponseString(PayUtil.CreatePostHttpResponse("https://www.paypayzhu.com/api/pay_json", remote)));

            return jo;
        }

        public string notifyPay(HttpRequest request, HttpResponse response, Paypayzhu paypayzhu)
        {
            // 保证密钥一致性
            if (PayUtil.checkPayKey(paypayzhu))
            {
                // TODO 做自己想做的
                return "成功了";
            }
            else
            {
                // TODO 该怎么做就怎么做
                return "失败了";
            }
        }

        public JObject order(string order_id)
        {
            Dictionary<string, string> paramMap = new Dictionary<string, string>();
            paramMap.Add("api_user", PayUtil.API_USER);
            paramMap.Add("order_id", order_id);
            string signature = PayUtil.getSignature(PayUtil.API_USER, paramMap);
            // System.out.println(signature);
            paramMap.Add("signature", signature);
            string idd = PayUtil.GetResponseString(PayUtil.CreatePostHttpResponse("https://www.paypayzhu.com/api/order_query", paramMap));
            JObject jo = (JObject)JsonConvert.DeserializeObject(PayUtil.GetResponseString(PayUtil.CreatePostHttpResponse("https://www.paypayzhu.com/api/order_query", paramMap)));
            //JSONObject result = PayUtil.post(API_URL + "order_query", paramMap);
            return jo;
        }
    }
}
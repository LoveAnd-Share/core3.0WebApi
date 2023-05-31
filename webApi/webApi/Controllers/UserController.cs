using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webApiCommon.Helper;
using webApiModel;

namespace webApi.Controllers
{
    /// <summary>
    /// 用户登录请求
    /// </summary>
    public class UserController : BaseController
    {
        /// <summary>
        /// 测试请求接口
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string Ret()
        {
            return "get";
        }
        /// <summary>
        /// 输出用户测试
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult User(UserModel user)
        {
            return Ok(user);
        }
        [HttpGet]
        public IActionResult Login(string role) 
        {
            string jwtStr = string.Empty;
            bool suc = false;

            if (role != null)
            {
                // 将用户id和角色名，作为单独的自定义变量封装进 token 字符串中。
                TokenModel tokenModel = new TokenModel { Uid = "abcde", Role = role };
                jwtStr = JwtHelper.IssueJwt(tokenModel);//登录，获取到一定规则的 Token 令牌
                suc = true;
            }
            else
            {
                jwtStr = "login fail!!!";
            }

            return Ok(new
            {
                success = suc,
                token = jwtStr
            });
        }
    }
}

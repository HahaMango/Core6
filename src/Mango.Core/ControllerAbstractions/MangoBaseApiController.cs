using Mango.Core.ApiResponse;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mango.Core.ControllerAbstractions
{
    public abstract class MangoBaseApiController : ControllerBase
    {
        /// <summary>
        /// 返回成功
        /// </summary>
        /// <returns></returns>
        protected virtual ApiResult OK()
        {
            return new ApiResult
            {
                Code = Enums.Code.Ok,
                Message = "成功"
            };
        }

        /// <summary>
        /// 返回成功 泛型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected virtual ApiResult<T> OK<T>()
        {
            return new ApiResult<T>
            {
                Code = Enums.Code.Ok,
                Message = "成功"
            };
        }

        /// <summary>
        /// 返回成功 泛型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected virtual ApiResult<T> OK<T>(T data)
        {
            return new ApiResult<T>
            {
                Code = Enums.Code.Ok,
                Message = "成功",
                Data = data
            };
        }

        /// <summary>
        /// 返回错误
        /// </summary>
        /// <returns></returns>
        protected virtual ApiResult Error()
        {
            return new ApiResult
            {
                Code = Enums.Code.Error,
                Message = "错误"
            };
        }

        /// <summary>
        /// 返回错误 泛型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected virtual ApiResult<T> Error<T>()
        {
            return new ApiResult<T>
            {
                Code = Enums.Code.Error,
                Message = "错误"
            };
        }

        /// <summary>
        /// 返回错误 泛型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected virtual ApiResult<T> Error<T>(string message)
        {
            return new ApiResult<T>
            {
                Code = Enums.Code.Error,
                Message = message
            };
        }

        /// <summary>
        /// 返回授权错误
        /// </summary>
        /// <returns></returns>
        protected virtual ApiResult AuthorizeError()
        {
            return new ApiResult
            {
                Code = Enums.Code.Unauthorized,
                Message = "该操作需要授权登录后才能继续进行"
            };
        }

        /// <summary>
        /// 返回授权错误 泛型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected virtual ApiResult<T> AuthorizeError<T>()
        {
            return new ApiResult<T>
            {
                Code = Enums.Code.Unauthorized,
                Message = "该操作需要授权登录后才能继续进行"
            };
        }

        /// <summary>
        /// 返回权限错误
        /// </summary>
        /// <returns></returns>
        protected virtual ApiResult ForbiddenError()
        {
            return new ApiResult
            {
                Code = Enums.Code.Forbidden,
                Message = "当前用户权限不足，不能继续执行"
            };
        }

        /// <summary>
        /// 返回权限错误 泛型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected virtual ApiResult<T> ForbiddenError<T>()
        {
            return new ApiResult<T>
            {
                Code = Enums.Code.Forbidden,
                Message = "当前用户权限不足，不能继续执行"
            };
        }

        /// <summary>
        /// 模型验证错误
        /// </summary>
        /// <returns></returns>
        protected virtual ApiResult InValidModelsError()
        {
            var msg = ModelsErrorMessage(ModelState);
            return new ApiResult
            {
                Code = Enums.Code.Error,
                Message = msg
            };
        }

        /// <summary>
        /// 模型验证错误
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected virtual ApiResult<T> InValidModelsError<T>()
        {
            var msg = ModelsErrorMessage(ModelState);
            return new ApiResult<T>
            {
                Code = Enums.Code.Error,
                Message = msg
            };
        }

        /// <summary>
        /// 模型验证错误信息
        /// </summary>
        /// <param name="modelState"></param>
        /// <returns></returns>
        private string ModelsErrorMessage(ModelStateDictionary modelState)
        {
            StringBuilder sb = new StringBuilder("");
            foreach(var m in modelState)
            {
                if (m.Value.ValidationState == ModelValidationState.Invalid)
                {
                    StringBuilder esb = new StringBuilder("");
                    foreach(var em in m.Value.Errors)
                    {
                        esb.Append($"{ em.ErrorMessage}\n");
                    }
                    sb.Append($"模型验证错误key:{m.Key},msg:{esb.ToString()}");
                }
            }
            return sb.ToString();
        }
    }
}

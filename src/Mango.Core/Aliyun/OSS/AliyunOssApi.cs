using Aliyun.OSS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mango.Core.Exceptions;
using Microsoft.Extensions.Logging;
using Mango.Core.Serialization.Extension;
using Microsoft.Extensions.Options;

namespace Mango.Core.Aliyun.OSS
{
    public class AliyunOssApi
    {
        private readonly AliyunOssOptions _options;
        private readonly OssClient _ossClient;

        private readonly ILogger<AliyunOssApi> _logger;

        public AliyunOssApi(IOptions<AliyunOssOptions> options, OssClient ossClient, ILogger<AliyunOssApi> logger)
        {
            _options = options.Value;
            _ossClient = ossClient;
            _logger = logger;
        }

        public Task<string> UploadFileAsync(string fileName, Stream stream)
        {
            //1.解析文件的key
            var timestamp = DateTime.Now.ToString("ddHHmmssfff");
            var month = DateTime.Now.ToString("yyyyMM");
            if (fileName.Contains('.'))
            {
                //如果有扩展名，取出名称部分加上随机数
                var fileNameArray = fileName.Split('.');
                fileName = $"{fileNameArray[0]}{timestamp}.{fileNameArray[1]}";
            }
            else
            {
                //如果不存在扩展名，直接在文件名后面加时间戳
                fileName = $"{fileName}{timestamp}";
            }
            //解析文件名，增加时间戳防止重复
            var key = $"{month}/{fileName}";

            //2.上传文件
            var ossResult = _ossClient.PutObject(_options.BucketName, key, stream);
            if (ossResult.HttpStatusCode != System.Net.HttpStatusCode.OK)
            {
                _logger.LogError($"文件上传失败：{ossResult.ToJson()}");
                throw new ServiceException("文件上传失败！");
            }
            return Task.FromResult($"https://{_options.BucketName}.{_options.Endpoint}/{key}");
        }
    }
}

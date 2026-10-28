using Microsoft.Extensions.Configuration;
using SpaceDealerService.API.Abstractions;
using System;
using System.Collections.Generic;

namespace SpaceDealerService
{
    public class AppSettings : IAppSettings
    {
        public Dictionary<string, string> Settings { get; set; }

        public string CONSUMER_ID { get; set; } = Guid.NewGuid().ToString();

        public AppSettings()
        {
            Settings = new Dictionary<string, string>
            {
                { "FileStorageRootPath", "" }
            };
        }

        public T GetValue<T>(string key)
        {
            return (T)Convert.ChangeType(Settings[key], typeof(T));
        }
        public void LoadSettings(IConfiguration configuration)
        {
            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("STORAGEROOT")))
                Settings["FileStorageRootPath"] = Environment.GetEnvironmentVariable("STORAGEROOT");
            else
                Settings["FileStorageRootPath"] = configuration["FileStorageRootPath"];
        }
    }

}
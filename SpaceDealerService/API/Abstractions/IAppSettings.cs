using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace SpaceDealerService.API.Abstractions
{
    public interface IAppSettings
    {
        public Dictionary<string, string> Settings { get; set; }
        public void LoadSettings(IConfiguration configuration);
        public T GetValue<T>(string key);
    }

}
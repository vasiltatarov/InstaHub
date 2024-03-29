﻿using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(InstaHub.Web.Areas.Identity.IdentityHostingStartup))]

namespace InstaHub.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
            => builder.ConfigureServices((context, services) => { });
    }
}

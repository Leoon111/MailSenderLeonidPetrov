using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using MailSender.lib.Interface;
using MailSender.lib.Service;
using MailSenderLeonidPetrov.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MailSenderLeonidPetrov
{
    public partial class App
    {
        private static IHost _Hosting;

        public static IHost Hosting => _Hosting
            ??= Host.CreateDefaultBuilder(Environment.GetCommandLineArgs())
                .ConfigureServices(ConfigureServices)
                .Build();

        public static IServiceProvider Services => Hosting.Services;

        private static void ConfigureServices(HostBuilderContext host, IServiceCollection service)
        {
            service.AddSingleton<MainWindowsViewModel>();
#if DEBUG
            service.AddTransient<IMailService, DebugMailService>();
#else
            service.AddTransient<IMailService, SmtpMailService>();
#endif
            service.AddSingleton<IEncryptorService, Rfc2898Encryptor>();
        }
    }
}

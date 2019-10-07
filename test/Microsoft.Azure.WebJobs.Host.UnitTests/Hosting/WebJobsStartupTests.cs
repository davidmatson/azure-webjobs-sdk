// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using Microsoft.Azure.WebJobs.Host.TestCommon;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Xunit;

[assembly: WebJobsStartup(typeof(Microsoft.Azure.WebJobs.Host.UnitTests.Hosting.WebJobsStartupTests.ExternalTestStartup))]

namespace Microsoft.Azure.WebJobs.Host.UnitTests.Hosting
{
    public class WebJobsStartupTests
    {
        private readonly TestLoggerProvider _provider = new TestLoggerProvider();
        private ILogger _logger;

        public WebJobsStartupTests()
        {
            ILoggerFactory loggerFactory = new LoggerFactory();
            loggerFactory.AddProvider(_provider);
            _logger = new TestLogger("WebJobsStartup"); // TODO - write logger & associated test logger for logging DI in this specific situation?
        }

        [Fact]
        public void WebJobsStartupAttribute_Constructor_InitializesAlias()
        {
            var attribute = new WebJobsStartupAttribute(typeof(FooStartup));
            Assert.Equal("Foo", attribute.Name);

            attribute = new WebJobsStartupAttribute(typeof(FooWebJobsStartup));
            Assert.Equal("Foo", attribute.Name);

            attribute = new WebJobsStartupAttribute(typeof(FooWebJobsStartup), "Bar");
            Assert.Equal("Bar", attribute.Name);
        }

        [Fact]
        public void GenericUseWebJobsStartup_CallsStartupMethods()
        {
            using (new StartupScope())
            {
                var builder = new HostBuilder()
                    .ConfigureWebJobs(webJobsBuilder =>
                    {
                        webJobsBuilder.UseWebJobsStartup<TestStartup>();
                    });

                IHost host = builder.Build();

                Assert.True(TestStartup.ConfigureInvoked);

                ITestService service = host.Services.GetService<ITestService>();

                Assert.NotNull(service);
            }
        }

        [Fact]
        public void UseWebJobsStartup_CallsStartupMethods()
        {
            using (new StartupScope())
            {
                var builder = new HostBuilder()
                    .ConfigureWebJobs(webJobsBuilder =>
                    {
                        webJobsBuilder.UseWebJobsStartup(typeof(TestStartup));
                    });


                IHost host = builder.Build();

                Assert.True(TestStartup.ConfigureInvoked);

                ITestService service = host.Services.GetService<ITestService>();

                Assert.NotNull(service);
            }
        }

        [Fact]
        public void UseWebJobsStartup_TestLogging()
        {
            using (new StartupScope())
            {
                var builder = new HostBuilder()
                    .ConfigureWebJobs(webJobsBuilder =>
                    {
                        webJobsBuilder.UseWebJobsStartup(typeof(TestStartup), _logger);
                    });


                IHost host = builder.Build();

                Assert.True(TestStartup.ConfigureInvoked);

                ITestService service = host.Services.GetService<ITestService>();

                Assert.NotNull(service);

                // TODO - check that diff was logged?


            }
        }


        [Fact]
        public void StartupTypes_FromAttributes_AreConfigured()
        {
            var builder = new HostBuilder()
                .ConfigureWebJobs(webJobsBuilder =>
                {
                    webJobsBuilder.UseExternalStartup(new DefaultStartupTypeLocator(GetType().Assembly));
                });

            IHost host = builder.Build();

            var service = host.Services.GetService<TestExternalService>();

            Assert.NotNull(service);
        }

        private class StartupScope : IDisposable
        {
            public StartupScope()
            {
                TestStartup.Reset();
            }

            public void Dispose()
            {
                TestStartup.Reset();
            }
        }

        private class TestStartup : IWebJobsStartup
        {
            [ThreadStatic]
            private static bool _configureInvoked;

            public static bool ConfigureInvoked => _configureInvoked;

            public void Configure(IWebJobsBuilder builder)
            {
                builder.Services.AddSingleton<ITestService, TestService>();

                _configureInvoked = true;
            }

            internal static void Reset()
            {
                _configureInvoked = false;
            }
        }


        public class ExternalTestStartup : IWebJobsStartup
        {
            public void Configure(IWebJobsBuilder builder)
            {
                builder.Services.AddSingleton<TestExternalService>();
            }
        }

        public class FooStartup : IWebJobsStartup
        {
            public void Configure(IWebJobsBuilder builder)
            {
                throw new NotImplementedException();
            }
        }

        public class FooWebJobsStartup : IWebJobsStartup
        {
            public void Configure(IWebJobsBuilder builder)
            {
                throw new NotImplementedException();
            }
        }

        private interface ITestService { }

        private class TestService : ITestService { }

        private class TestExternalService { }
    }
}

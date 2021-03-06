﻿using Microsoft.Extensions.Configuration;
using SteelToe.CloudFoundry.Connector;
using SteelToe.CloudFoundry.Connector.Services;
using SteelToe.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Pivotal.Discovery.Client.Test
{
    public class DiscoveryClientConfigurerTest : AbstractBaseTest
    {
        [Fact]
        public void Configure_NoServiceInfo_ConfiguresEurekaDiscovery_Correctly()
        {
            // Arrange
            var appsettings = @"
{
'eureka': {
    'client': {
        'eurekaServer': {
            'proxyHost': 'proxyHost',
            'proxyPort': 100,
            'proxyUserName': 'proxyUserName',
            'proxyPassword': 'proxyPassword',
            'shouldGZipContent': true,
            'connectTimeoutSeconds': 100
        },
        'allowRedirects': true,
        'shouldDisableDelta': true,
        'shouldFilterOnlyUpInstances': true,
        'shouldFetchRegistry': true,
        'registryRefreshSingleVipAddress':'registryRefreshSingleVipAddress',
        'shouldOnDemandUpdateStatusChange': true,
        'shouldRegisterWithEureka': true,
        'registryFetchIntervalSeconds': 100,
        'instanceInfoReplicationIntervalSeconds': 100,
        'serviceUrl': 'http://localhost:8761/eureka/'
    },
    'instance': {
        'instanceId': 'instanceId',
        'appName': 'appName',
        'appGroup': 'appGroup',
        'instanceEnabledOnInit': true,
        'port': 100,
        'securePort': 100,
        'nonSecurePortEnabled': true,
        'securePortEnabled': true,
        'leaseExpirationDurationInSeconds':100,
        'leaseRenewalIntervalInSeconds': 100,
        'secureVipAddress': 'secureVipAddress',
        'vipAddress': 'vipAddress',
        'asgName': 'asgName',
        'metadataMap': {
            'foo': 'bar',
            'bar': 'foo'
        },
        'statusPageUrlPath': 'statusPageUrlPath',
        'statusPageUrl': 'statusPageUrl',
        'homePageUrlPath':'homePageUrlPath',
        'homePageUrl': 'homePageUrl',
        'healthCheckUrlPath': 'healthCheckUrlPath',
        'healthCheckUrl':'healthCheckUrl',
        'secureHealthCheckUrl':'secureHealthCheckUrl'   
    }
    }
}";
            var basePath = Path.GetTempPath();
            var path = TestHelpers.CreateTempFile(appsettings);
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.SetBasePath(basePath);
            configurationBuilder.AddJsonFile(Path.GetFileName(path));
            var config = configurationBuilder.Build();
            var options = new DiscoveryOptions(config);

            DiscoveryClientConfigurer configurer = new DiscoveryClientConfigurer();
            configurer.Configure(null, options);

            Assert.Equal(DiscoveryClientType.EUREKA, options.ClientType);

            var co = options.ClientOptions as EurekaClientOptions;
            Assert.NotNull(co);
            Assert.Equal("proxyHost", co.ProxyHost);
            Assert.Equal(100, co.ProxyPort);
            Assert.Equal("proxyPassword", co.ProxyPassword);
            Assert.Equal("proxyUserName", co.ProxyUserName);
            Assert.True(co.AllowRedirects);
            Assert.Equal(100, co.InstanceInfoReplicationIntervalSeconds);
            Assert.Equal(100, co.EurekaServerConnectTimeoutSeconds);
            Assert.Equal("http://localhost:8761/eureka/", co.EurekaServerServiceUrls);
            Assert.Equal(100, co.RegistryFetchIntervalSeconds);
            Assert.Equal("registryRefreshSingleVipAddress", co.RegistryRefreshSingleVipAddress);
            Assert.True(co.ShouldDisableDelta);
            Assert.True(co.ShouldFetchRegistry);
            Assert.True(co.ShouldFilterOnlyUpInstances);
            Assert.True(co.ShouldGZipContent);
            Assert.True(co.ShouldOnDemandUpdateStatusChange);
            Assert.True(co.ShouldRegisterWithEureka);

            var ro = options.RegistrationOptions as EurekaInstanceOptions;
            Assert.NotNull(ro);


            Assert.Equal("instanceId", ro.InstanceId);
            Assert.Equal("appName", ro.AppName);
            Assert.Equal("appGroup", ro.AppGroupName);
            Assert.True(ro.IsInstanceEnabledOnInit);
            Assert.Equal(100, ro.NonSecurePort);
            Assert.Equal(100, ro.SecurePort);
            Assert.True(ro.IsNonSecurePortEnabled);
            Assert.True(ro.SecurePortEnabled);
            Assert.Equal(100, ro.LeaseExpirationDurationInSeconds);
            Assert.Equal(100, ro.LeaseRenewalIntervalInSeconds);
            Assert.Equal("secureVipAddress", ro.SecureVirtualHostName);
            Assert.Equal("vipAddress", ro.VirtualHostName);
            Assert.Equal("asgName", ro.ASGName);

            Assert.Equal("statusPageUrlPath", ro.StatusPageUrlPath);
            Assert.Equal("statusPageUrl", ro.StatusPageUrl);
            Assert.Equal("homePageUrlPath", ro.HomePageUrlPath);
            Assert.Equal("homePageUrl", ro.HomePageUrl);
            Assert.Equal("healthCheckUrlPath", ro.HealthCheckUrlPath);
            Assert.Equal("healthCheckUrl", ro.HealthCheckUrl);
            Assert.Equal("secureHealthCheckUrl", ro.SecureHealthCheckUrl);

            var map = ro.MetadataMap;
            Assert.NotNull(map);
            Assert.Equal(2, map.Count);
            Assert.Equal("bar", map["foo"]);
            Assert.Equal("foo", map["bar"]);
        }

        [Fact]
        public void Configure_WithVCAPEnvVariables_ConfiguresEurekaDiscovery_Correctly()
        {
            var vcap_application = @"
{
    'limits': {
    'fds': 16384,
    'mem': 512,
    'disk': 1024
    },
    'application_name': 'foo',
    'application_uris': [
    'foo.apps.testcloud.com'
    ],
    'name': 'foo',
    'space_name': 'test',
    'space_id': '98c627e7-f559-46a4-9032-88cab63f8249',
    'uris': [
    'foo.apps.testcloud.com'
    ],
    'users': null,
    'version': '4a439db9-4a82-47a3-aeea-8240465cff8e',
    'application_version': '4a439db9-4a82-47a3-aeea-8240465cff8e',
    'application_id': 'ac923014-93a5-4aee-b934-a043b241868b',
    'instance_id': 'instance_id'

}";
            var vcap_services = @"
{
'p-config-server': [
    {
    'credentials': {
        'uri': 'https://config-de211817-2e99-4c57-89e8-31fa7ca6a276.apps.testcloud.com',
        'client_id': 'p-config-server-8f49dd26-e6cd-47a6-b2a0-7655cea20333',
        'client_secret': 'vBDjqIf7XthT',
        'access_token_uri': 'https://p-spring-cloud-services.uaa.system.testcloud.com/oauth/token'
    },
    'syslog_drain_url': null,
    'label': 'p-config-server',
    'provider': null,
    'plan': 'standard',
    'name': 'myConfigServer',
    'tags': [
        'configuration',
        'spring-cloud'
    ]
    }
    ],
'p-service-registry': [
{
    'credentials': {
        'uri': 'https://eureka-6a1b81f5-79e2-4d14-a86b-ddf584635a60.apps.testcloud.com',
        'client_id': 'p-service-registry-06e28efd-24be-4ce3-9784-854ed8d2acbe',
        'client_secret': 'dCsdoiuklicS',
        'access_token_uri': 'https://p-spring-cloud-services.uaa.system.testcloud.com/oauth/token'
        },
    'syslog_drain_url': null,
    'label': 'p-service-registry',
    'provider': null,
    'plan': 'standard',
    'name': 'myDiscoveryService',
    'tags': [
    'eureka',
    'discovery',
    'registry',
    'spring-cloud'
    ]
}
]
}";

            var appsettings = @"
{
'eureka': {
    'client': {
        'eurekaServer': {
            'proxyHost': 'proxyHost',
            'proxyPort': 100,
            'proxyUserName': 'proxyUserName',
            'proxyPassword': 'proxyPassword',
            'shouldGZipContent': true,
            'connectTimeoutSeconds': 100
        },
        'allowRedirects': true,
        'shouldDisableDelta': true,
        'shouldFilterOnlyUpInstances': true,
        'shouldFetchRegistry': true,
        'registryRefreshSingleVipAddress':'registryRefreshSingleVipAddress',
        'shouldOnDemandUpdateStatusChange': true,
        'shouldRegisterWithEureka': true,
        'registryFetchIntervalSeconds': 100,
        'instanceInfoReplicationIntervalSeconds': 100,
        'serviceUrl': 'http://localhost:8761/eureka/'
    },
    'instance': {
        'instanceId': 'instanceId',
        'appName': 'appName',
        'appGroup': 'appGroup',
        'instanceEnabledOnInit': true,
        'port': 100,
        'securePort': 100,
        'nonSecurePortEnabled': true,
        'securePortEnabled': true,
        'leaseExpirationDurationInSeconds':100,
        'leaseRenewalIntervalInSeconds': 100,
        'secureVipAddress': 'secureVipAddress',
        'vipAddress': 'vipAddress',
        'asgName': 'asgName',
        'metadataMap': {
            'foo': 'bar',
            'bar': 'foo'
        },
        'statusPageUrlPath': 'statusPageUrlPath',
        'statusPageUrl': 'statusPageUrl',
        'homePageUrlPath':'homePageUrlPath',
        'homePageUrl': 'homePageUrl',
        'healthCheckUrlPath': 'healthCheckUrlPath',
        'healthCheckUrl':'healthCheckUrl',
        'secureHealthCheckUrl':'secureHealthCheckUrl'   
    }
    }
}";
            Environment.SetEnvironmentVariable("VCAP_APPLICATION", vcap_application);
            Environment.SetEnvironmentVariable("VCAP_SERVICES", vcap_services);
            var basePath = Path.GetTempPath();
            var path = TestHelpers.CreateTempFile(appsettings);
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.SetBasePath(basePath);
            configurationBuilder.AddJsonFile(Path.GetFileName(path));
            configurationBuilder.AddCloudFoundry();
            var config = configurationBuilder.Build();

            var sis = config.GetServiceInfos<EurekaServiceInfo>();
            Assert.Equal(1, sis.Count);
            IServiceInfo si = sis[0];
            var options = new DiscoveryOptions(config);

            DiscoveryClientConfigurer configurer = new DiscoveryClientConfigurer();
            configurer.Configure(si, options);

            Assert.Equal(DiscoveryClientType.EUREKA, options.ClientType);

            var co = options.ClientOptions as EurekaClientOptions;
            Assert.NotNull(co);
            Assert.Equal("proxyHost", co.ProxyHost);
            Assert.Equal(100, co.ProxyPort);
            Assert.Equal("proxyPassword", co.ProxyPassword);
            Assert.Equal("proxyUserName", co.ProxyUserName);
            Assert.True(co.AllowRedirects);
            Assert.Equal(100, co.InstanceInfoReplicationIntervalSeconds);
            Assert.Equal(100, co.EurekaServerConnectTimeoutSeconds);
            Assert.Equal("https://eureka-6a1b81f5-79e2-4d14-a86b-ddf584635a60.apps.testcloud.com/eureka/", co.EurekaServerServiceUrls);
            Assert.Equal(100, co.RegistryFetchIntervalSeconds);
            Assert.Equal("registryRefreshSingleVipAddress", co.RegistryRefreshSingleVipAddress);
            Assert.True(co.ShouldDisableDelta);
            Assert.True(co.ShouldFetchRegistry);
            Assert.True(co.ShouldFilterOnlyUpInstances);
            Assert.True(co.ShouldGZipContent);
            Assert.True(co.ShouldOnDemandUpdateStatusChange);
            Assert.True(co.ShouldRegisterWithEureka);
            Assert.Equal("https://p-spring-cloud-services.uaa.system.testcloud.com/oauth/token", co.AccessTokenUri);
            Assert.Equal("p-service-registry-06e28efd-24be-4ce3-9784-854ed8d2acbe", co.ClientId);
            Assert.Equal("dCsdoiuklicS", co.ClientSecret);


            var ro = options.RegistrationOptions as EurekaInstanceOptions;
            Assert.NotNull(ro);


            Assert.Equal("foo.apps.testcloud.com:instance_id", ro.InstanceId);
            Assert.Equal("foo", ro.AppName);
            Assert.Equal("appGroup", ro.AppGroupName);
            Assert.True(ro.IsInstanceEnabledOnInit);
            Assert.Equal(80, ro.NonSecurePort);
            Assert.Equal(100, ro.SecurePort);
            Assert.True(ro.IsNonSecurePortEnabled);
            Assert.True(ro.SecurePortEnabled);
            Assert.Equal(100, ro.LeaseExpirationDurationInSeconds);
            Assert.Equal(100, ro.LeaseRenewalIntervalInSeconds);
            Assert.Equal("secureVipAddress", ro.SecureVirtualHostName);
            Assert.Equal("vipAddress", ro.VirtualHostName);
            Assert.Equal("asgName", ro.ASGName);

            Assert.Equal("statusPageUrlPath", ro.StatusPageUrlPath);
            Assert.Equal("statusPageUrl", ro.StatusPageUrl);
            Assert.Equal("homePageUrlPath", ro.HomePageUrlPath);
            Assert.Equal("homePageUrl", ro.HomePageUrl);
            Assert.Equal("healthCheckUrlPath", ro.HealthCheckUrlPath);
            Assert.Equal("healthCheckUrl", ro.HealthCheckUrl);
            Assert.Equal("secureHealthCheckUrl", ro.SecureHealthCheckUrl);

            var map = ro.MetadataMap;
            Assert.NotNull(map);
            Assert.Equal(3, map.Count);
            Assert.Equal("bar", map["foo"]);
            Assert.Equal("foo", map["bar"]);
            Assert.Equal("instance_id", map["instanceId"]);
        }
    }
}


using Steeltoe.Common.Discovery;
using Steeltoe.Discovery;
using Steeltoe.Discovery.Client;
using Steeltoe.Discovery.Eureka;
using Yarp.ReverseProxy.Forwarder;

namespace TackyTacos.ApiGateway;
public class ServiceDiscoveryForwarderHttpClientFactory : IForwarderHttpClientFactory
{ 
	private IDiscoveryClient _discoveryClient;
	public ServiceDiscoveryForwarderHttpClientFactory(IDiscoveryClient discoveryClient)
	{
		_discoveryClient = discoveryClient;
	}

	public HttpMessageInvoker CreateClient(ForwarderHttpClientContext context)
	{
		var handler = new DiscoveryHttpClientHandler(_discoveryClient);

		var client = new HttpMessageInvoker(handler, false);

		return client;
	}
}

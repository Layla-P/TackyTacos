
using Steeltoe.Common.Discovery;
using Steeltoe.Discovery;
using Yarp.ReverseProxy.Forwarder;

namespace TackyTacos.ApiGateway;
public class ServiceDiscoveryForwarderHttpClientFactory : IForwarderHttpClientFactory
{
	DiscoveryHttpClientHandler _handler;

	public HttpMessageInvoker CreateClient(ForwarderHttpClientContext context)
	{
		IDiscoveryClient discoveryClient = new DiscoveryClient(context);

		_handler = new DiscoveryHttpClientHandler(discoveryClient);

		var client = new HttpMessageInvoker(_handler, false);

		return client;
	}
}

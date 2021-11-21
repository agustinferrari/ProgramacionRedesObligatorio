
using Microsoft.Extensions.DependencyInjection;
using ServerAdmin.ServicesGrpc;
using ServerAdmin.ServicesGrpcInterfaces;

namespace Factory.Factories
{
    public class ServiceFactory
    {
        private readonly IServiceCollection services;

        public ServiceFactory(IServiceCollection services)
        {
            this.services = services;
        }

        public void AddCustomServicesServerAdmin()
        {
            services.AddScoped<IGameGrpc, GameGrpc>();
            services.AddScoped<IUserGrpc, UserGrpc>();
        }
        
    }
}
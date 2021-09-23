using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Retention.Abstractions;

namespace Retention.Extensions
{
    /// <summary>
    ///     Extension methods for <see cref="IServiceCollection"/>
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        ///     Adds required Retention types to the IServiceCollection
        /// </summary>
        /// <param name="services">
        ///     The service collection.
        /// </param>
        /// <returns>
        ///     The service collection.
        /// </returns>
        public static IServiceCollection AddRetention(this IServiceCollection services)
        {
            services.AddLogging();
            
            services.TryAddScoped(typeof(IRetention<>), typeof(Retention<>));
            services.TryAddScoped(typeof(IRetentionPolicyManager<>), typeof(RetentionPolicyManager<>));
            
            return services;
        }
    }
}
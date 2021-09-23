using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ReleaseRetention.Abstractions;
using Retention.Extensions;

namespace ReleaseRetention.Extensions
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
        /// <param name="releaseSource">
        ///     The source of releases
        /// </param>
        /// <param name="policyManagerBuilder">
        ///     The default policy builder.
        /// </param>
        /// <returns>
        ///     The service collection.
        /// </returns>
        public static IServiceCollection AddReleaseRetention<TReleaseSource, TReleaseRemovalHandler>
        (this IServiceCollection services,
            Action<ReleaseRetentionPolicyManagerBuilder>? policyManagerBuilder = null)
            where TReleaseSource : class, IReleaseSource
            where TReleaseRemovalHandler : class, IReleaseRemovalHandler
        {
            services.AddRetention();

            services.TryAddSingleton(typeof(IReleaseRetention), typeof(ReleaseRetention));
            services.TryAddSingleton(typeof(IReleaseRetentionPolicyManager), typeof(ReleaseRetentionPolicyManager));
            services.TryAddScoped(typeof(IReleaseSource), typeof(TReleaseSource));
            services.TryAddScoped(typeof(IReleaseRemovalHandler), typeof(TReleaseRemovalHandler));

            // If the policy manager builder is not null then use to build the manager
            if (policyManagerBuilder is not null)
            {
                services.TryAddScoped(typeof(ReleaseRetentionPolicyManagerBuilder), s =>
                {
                    var builder = new ReleaseRetentionPolicyManagerBuilder();
                    policyManagerBuilder(builder);
                    return builder;
                });
            }
            else
            {
                services.TryAddScoped(typeof(ReleaseRetentionPolicyManagerBuilder),
                    typeof(ReleaseRetentionPolicyManagerBuilder));
            }

            return services;
        }
    }
}
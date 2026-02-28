using System;
using Codebelt.Extensions.AspNetCore.Text.Yaml.Formatters;
using Codebelt.Extensions.YamlDotNet.Formatters;
using Cuemon;
using Microsoft.Extensions.DependencyInjection;

namespace Codebelt.Extensions.AspNetCore.Text.Yaml
{
    /// <summary>
    /// Extension methods for the <see cref="IServiceCollection"/> interface.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds a <see cref="YamlFormatterOptions"/> service to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
        /// <param name="setup">The <see cref="YamlFormatterOptions"/> which may be configured.</param>
        /// <returns>An <see cref="IServiceCollection"/> that can be used to further configure other services.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="services"/> cannot be null.
        /// </exception>
        public static IServiceCollection AddMinimalYamlOptions(this IServiceCollection services, Action<YamlFormatterOptions> setup = null)
        {
            Validator.ThrowIfNull(services);
            return services.AddYamlExceptionResponseFormatter(setup);
        }
    }
}

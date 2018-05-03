using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace WebApi.POC.Swagger
{
    /// <summary>
    /// Filter for adding auth indicator at the swagger UI
    /// </summary>
    public class AuthorizationFilter : IOperationFilter
    {
        /// <summary>
        /// Applies the filter
        /// </summary>
        /// <param name="operation">Operation instance</param>
        /// <param name="context">OperationFilterContext instance</param>
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var controllerScopes = context.ApiDescription.ControllerAttributes()
            .OfType<AuthorizeAttribute>()
            .Select(attr => attr.Policy);

            var actionScopes = context.ApiDescription.ActionAttributes()
                .OfType<AuthorizeAttribute>()
                .Select(attr => attr.Policy);

            var requiredScopes = controllerScopes.Union(actionScopes).Distinct();

            if (requiredScopes.Any())
            {
                if (!operation.Responses.ContainsKey("401"))
                {
                    operation.Responses.Add("401", new Response { Description = "Unauthorized" });
                }

                if (!operation.Responses.ContainsKey("403"))
                {
                    operation.Responses.Add("403", new Response { Description = "Forbidden" });
                }

                operation.Security = new List<IDictionary<string, IEnumerable<string>>>
                {
                    new Dictionary<string, IEnumerable<string>>
                    {
                        { "Bearer", requiredScopes }
                    }
                };
            }
        }
    }
}

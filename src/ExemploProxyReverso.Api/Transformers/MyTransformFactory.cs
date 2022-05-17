using Yarp.ReverseProxy.Transforms.Builder;

namespace ExemploProxyReverso.Api.Transformers;

public class MyTransformFactory : ITransformFactory
{
    public bool Validate(TransformRouteValidationContext context, IReadOnlyDictionary<string, string> transformValues)
    {
        if (transformValues.TryGetValue("CustomTransformName", out var value))
        {
            if (string.IsNullOrEmpty(value))
            {
                context.Errors.Add(new ArgumentException("A non-empty CustomTransformName value is required"));
            }

            return true; // Matched
        }
        return false;
    }

    public bool Build(TransformBuilderContext transformBuildContext, IReadOnlyDictionary<string, string> transformValues)
    {
        // Check all routes for a custom property and add the associated transform.
        string? customName = null;

        transformBuildContext.Route.Transforms?.FirstOrDefault(x => x.TryGetValue("CustomTransformName", out customName));
        var hasTransforms = transformBuildContext.Route.Transforms != null;
        var hasCustomNameTransform = !string.IsNullOrWhiteSpace(customName);

        if (hasCustomNameTransform)
        {
            var transform = new CustomResponseTransform(customName!);
            transformBuildContext.ResponseTransforms.Add(transform);
        }

        return true;
    }
}
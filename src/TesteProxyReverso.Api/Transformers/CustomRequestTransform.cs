using System.Text;
using Yarp.ReverseProxy.Transforms;

namespace TesteProxyReverso.Api.Transformers;

/// <summary>
/// Custom request transformation
/// </summary>
public class CustomRequestTransform : RequestTransform
{
    private readonly string nome;

    public CustomRequestTransform(string nome) => this.nome = nome;

    public override ValueTask ApplyAsync(RequestTransformContext context)
    {
        var newContent = new StringContent("{\"nome\":\"" + nome + "\"}", Encoding.UTF8, "application/json");
        context.ProxyRequest.Content = newContent;
        return default;
    }
}
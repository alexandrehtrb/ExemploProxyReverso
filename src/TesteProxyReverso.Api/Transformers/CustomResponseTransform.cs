using System.Text;
using Yarp.ReverseProxy.Transforms;

namespace TesteProxyReverso.Api.Transformers;

/// <summary>
/// Custom request transformation
/// </summary>
public class CustomResponseTransform : ResponseTransform
{
    private readonly string nome;

    public CustomResponseTransform(string nome) => this.nome = nome;

    public override ValueTask ApplyAsync(ResponseTransformContext context)
    {
        var newContent = new StringContent("{\"nome\":\"" + nome + "\"}", Encoding.UTF8, "application/json");
        if (context.ProxyResponse != null)
        {
            context.ProxyResponse.Content = newContent;
        }
        return default;
    }
}
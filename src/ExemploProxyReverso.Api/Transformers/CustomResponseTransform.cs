using System.Text;
using Yarp.ReverseProxy.Transforms;

namespace ExemploProxyReverso.Api.Transformers;

/// <summary>
/// Custom request transformation
/// </summary>
public class CustomResponseTransform : ResponseTransform
{
    private readonly string nome;

    public CustomResponseTransform(string nome) => this.nome = nome;

    //https://github.com/microsoft/reverse-proxy/blob/main/docs/docfx/articles/transforms.md#response-body-transforms

    public override async ValueTask ApplyAsync(ResponseTransformContext responseContext)
    {
        string newBody = "{\"nome\":\"" + nome + "\"}";
        byte[] bytes = Encoding.UTF8.GetBytes(newBody);

        if (responseContext.ProxyResponse is not null)
        {
            responseContext.SuppressResponseBody = true;
            responseContext.HttpContext.Response.ContentType = "application/json";
            responseContext.HttpContext.Response.ContentLength = bytes.LongLength;
            await responseContext.HttpContext.Response.Body.WriteAsync(bytes);
        }
    }
}
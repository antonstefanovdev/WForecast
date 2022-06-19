using Castle.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.ObjectPool;
using sun.net.www.content.text;
using System.Text;
using System.Text.Encodings.Web;
using WApi.Repository;

public class CityMiddlewareResult<T,F> where T : ApplicationContext where F : ObjectPool<ApplicationContext>
{
    public T context { get; set; }
    public F pool { get; set; }
    public CityMiddlewareResult(T context, F pool)
    {
        this.context = context;
        this.pool = pool;
    }
}
public class CityMiddleware
{    
    public async Task<CityMiddlewareResult<ApplicationContext, ObjectPool<ApplicationContext>>> GetContextAsync(ObjectPool<ApplicationContext> contextPool)
    {
        return new CityMiddlewareResult<ApplicationContext, ObjectPool<ApplicationContext>>(contextPool.Get(), contextPool);
    }
}
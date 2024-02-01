using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace GSMF.Extensions
{
    public class DisplayName
    {
        public string Display<TProperty>(Expression<Func<TProperty>> f, bool getType = true)
        {
            var exp = f.Body as MemberExpression;
            var property = exp.Expression.Type.GetProperty(exp.Member.Name);
            var attr = property?.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault() as DisplayAttribute;
            if(getType == true)
            {
                return attr?.GetName() ?? exp.Member.Name;
            }
            else
            {
                return exp.Member.Name;
            }
        }
    }
}

using System;
using System.Linq;
using System.Linq.Expressions;

namespace Cinotam.AbpModuleZero.Tools.DatatablesJsModels.ReflectionHelpers
{
    /// <summary>
    /// Hasta esta parte del codigo me queria suicidar señores!!
    /// Sauce: http://stackoverflow.com/questions/18743976/get-iqueryablet-where-any-field-of-t-contains-a-given-string
    /// Sauce: http://reflection241.blogspot.mx/2015/07/c-dynamically-create-lambda-search-on.html
    /// </summary>
    public static class QueryableHelper
    {
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName)

        {

            return (IQueryable<T>)OrderBy((IQueryable)source, propertyName);

        }

        public static IQueryable OrderBy(this IQueryable source, string propertyName)

        {
            try
            {
                var x = Expression.Parameter(source.ElementType, "x");

                var selector = Expression.Lambda(Expression.PropertyOrField(x, propertyName), x);

                return source.Provider.CreateQuery(

                    Expression.Call(typeof(Queryable), "OrderBy", new[] { source.ElementType, selector.Body.Type },

                        source.Expression, selector
                    ));
            }
            catch (Exception)
            {
                return source.OrderBy("Id");
            }

        }

        public static IQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string propertyName)

        {

            return (IQueryable<T>)OrderByDescending((IQueryable)source, propertyName);

        }

        public static IQueryable OrderByDescending(this IQueryable source, string propertyName)

        {
            try
            {
                var x = Expression.Parameter(source.ElementType, "x");

                var selector = Expression.Lambda(Expression.PropertyOrField(x, propertyName), x);

                return source.Provider.CreateQuery(

                    Expression.Call(typeof(Queryable), "OrderByDescending", new[] { source.ElementType, selector.Body.Type },

                         source.Expression, selector

                         ));
            }
            catch (Exception)
            {

                return source.OrderByDescending("Id");
            }


        }
        public static IQueryable<T> Where<T>(this IQueryable<T> source, string propertyName, string valueToSearch)
        {

            var lambda = SearchAllFieldsWithPropertyDefined<T>(valueToSearch, propertyName);
            return source.Where(lambda);
        }
        public static Expression<Func<T, bool>> SearchAllFields<T>(string searchText)
        {
            var t = Expression.Parameter(typeof(T));
            Expression body = Expression.Constant(false);

            var containsMethod = typeof(string).GetMethod("Contains"
                , new[] { typeof(string) });
            var toStringMethod = typeof(object).GetMethod("ToString");

            var stringProperties = typeof(T).GetProperties()
                .Where(property => property.PropertyType == typeof(string));

            foreach (var property in stringProperties)
            {
                var stringValue = Expression.Call(Expression.Property(t, property.Name),
                    toStringMethod);
                var nextExpression = Expression.Call(stringValue,
                    containsMethod,
                    Expression.Constant(searchText));

                body = Expression.OrElse(body, nextExpression);
            }

            return Expression.Lambda<Func<T, bool>>(body, t);
        }
        public static Expression<Func<T, bool>> SearchAllFieldsWithPropertyDefined<T>(string searchText, string propery)
        {
            var t = Expression.Parameter(typeof(T));
            Expression body = Expression.Constant(false);

            var containsMethod = typeof(string).GetMethod("Contains"
                , new[] { typeof(string) });
            var toStringMethod = typeof(object).GetMethod("ToString");

            var stringProperties = typeof(T).GetProperties()
                .Where(property => property.PropertyType == typeof(string) && property.Name == propery);

            foreach (var property in stringProperties)
            {
                var stringValue = Expression.Call(Expression.Property(t, property.Name),
                    toStringMethod);
                var nextExpression = Expression.Call(stringValue,
                    containsMethod,
                    Expression.Constant(searchText));

                body = Expression.OrElse(body, nextExpression);
            }

            return Expression.Lambda<Func<T, bool>>(body, t);
        }
    }
}

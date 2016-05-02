using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Aragas.Network
{
    /// <summary>
    /// Cached implementation of <see cref="Activator"/>.
    /// </summary>
    public static class ActivatorCached
    {
        private delegate object ObjectActivator(params object[] args);

        private static readonly IDictionary<Type, ObjectActivator> Cache = new Dictionary<Type, ObjectActivator>();


        /// <summary>
        /// Faster implementation of <see cref="Activator.CreateInstance(Type, object[])"/>.
        /// <para/>
        /// First <see cref="Type"/> creation is slow.
        /// </summary>
        public static object CreateInstance(Type input, params object[] args)
        {
            ObjectActivator objectActivator;
            if (Cache.TryGetValue(input, out objectActivator))
                return objectActivator(args);

            var types = args.Select(p => p.GetType());

            var constructors = input.GetTypeInfo().DeclaredConstructors;
            var constructor = args.Length == 0
                ? constructors.First()
                : constructors.Single(constr => constr.GetParameters().Select(param => param.ParameterType).SequenceEqual(types));

            var paraminfo = constructor.GetParameters();

            var paramex = Expression.Parameter(typeof (object[]), "args");

            var argex = new Expression[paraminfo.Length];
            for (var i = 0; i < paraminfo.Length; i++)
            {
                var index = Expression.Constant(i);
                var paramType = paraminfo[i].ParameterType;
                var accessor = Expression.ArrayIndex(paramex, index);
                var cast = Expression.Convert(accessor, paramType);
                argex[i] = cast;
            }

            var newex = Expression.New(constructor, argex);
            var lambda = Expression.Lambda(typeof (ObjectActivator), newex, paramex);
            var result = (ObjectActivator) lambda.Compile();
            Cache.Add(input, result);
            return result(args);
        }


        /// <summary>
        /// Clear the cache.
        /// </summary>
        public static void ClearCache() { Cache.Clear(); }
    }
}
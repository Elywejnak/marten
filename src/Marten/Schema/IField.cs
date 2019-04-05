using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Marten.Schema
{
    public interface IField
    {
        MemberInfo[] Members { get; }

        /// <summary>
        /// Postgresql locator that also casts the raw string data to the proper Postgresql type
        /// </summary>
        string TypedLocator { get; }

        /// <summary>
        /// Postgresql locator that returns the raw string value within the JSONB document
        /// </summary>
        string RawLocator { get; }

        /// <summary>
        /// May "correct" the raw value as appropriate for the constant parameter value
        /// within a compiled query
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        object GetValueForCompiledQueryParameter(Expression valueExpression);

        /// <summary>
        /// The .Net type of this IField
        /// </summary>
        Type FieldType { get; }
        
        [Obsolete]
        bool ShouldUseContainmentOperator();
        
        
        string LocatorFor(string rootTableAlias);
    }

    

    public interface IFieldSource
    {
        bool TryResolve(string dataLocator, StoreOptions options, ISerializer serializer, Type documentType,
            MemberInfo[] members, out IField field);
    }

    public class DefaultFieldSource : IFieldSource
    {
        public bool TryResolve(string dataLocator, StoreOptions options, ISerializer serializer, Type documentType,
            MemberInfo[] members, out IField field)
        {
            if (members.Length == 1)
            {
                field = new JsonLocatorField(dataLocator, options,serializer.EnumStorage, serializer.Casing, members.Single());
            }
            else
            {
                field = new JsonLocatorField(dataLocator, serializer.EnumStorage, serializer.Casing, members);
            }
            
            

            return true;
        }
    }
}
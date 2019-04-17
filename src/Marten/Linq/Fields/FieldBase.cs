using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Marten.Util;

namespace Marten.Linq.Fields
{
    public abstract class FieldBase : IField
    {
        protected FieldBase(string dataLocator, string pgType, Casing casing, MemberInfo[] members)
        {
            if (members == null || members.Length == 0) throw new ArgumentNullException(nameof(members));

            Members = members;
            
            lastMember = members.Last();
            
            FieldType = lastMember.GetMemberType();
            
            var locator = dataLocator;

            for (int i = 0; i < members.Length - 1; i++)
            {
                locator += $" -> '{members[i].Name.FormatCase(casing)}'";
            }

            parentLocator = locator;
            lastMemberName = lastMember.Name.FormatCase(casing);
            
            RawLocator = TypedLocator = $"{parentLocator} ->> '{lastMemberName}'";

            PgType = pgType;
        }

        protected string lastMemberName { get;  }

        protected string parentLocator { get;  }

        protected MemberInfo lastMember { get; }

        public Type FieldType { get; }
        
        public MemberInfo[] Members { get; }
        
        public string RawLocator { get; protected set; }

        public string TypedLocator { get; protected set; }
        
        public string PgType { get; set; } // settable so it can be overidden by users

        [Obsolete("Eliminate soon")]
        public virtual bool ShouldUseContainmentOperator()
        {
            return false;
        }

        public virtual object GetValueForCompiledQueryParameter(Expression valueExpression)
        {
            return valueExpression.Value();
        }
        
        public string LocatorFor(string rootTableAlias)
        {
            // Super hokey.
            return TypedLocator.Replace("d.", rootTableAlias + ".");
        }
    }
}
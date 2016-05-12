using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Data.Entity;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace TDDUtils.EntityFramework
{
    public class EFMockUtils
    {
        public static DbSet<T> GenerateDBSetSubstitude<T>(IQueryable<T> source) where T:class
        {
            var entityDBSet = Substitute.For<DbSet<T>, IQueryable<T>, IDbAsyncEnumerable<T>>();

            ((IDbAsyncEnumerable<T>)entityDBSet).GetAsyncEnumerator().Returns(new TestDbAsyncEnumerator<T>(source.GetEnumerator()));
            ((IQueryable<T>)entityDBSet).Provider.Returns(new TestDbAsyncQueryProvider<T>(source.Provider));

            ((IQueryable<T>)entityDBSet).Expression.Returns(source.Expression);
            ((IQueryable<T>)entityDBSet).ElementType.Returns(source.ElementType);
            ((IQueryable<T>)entityDBSet).GetEnumerator().Returns(source.GetEnumerator());
            
            entityDBSet.AsNoTracking().Returns(entityDBSet);

            return entityDBSet;
        }
    }
}

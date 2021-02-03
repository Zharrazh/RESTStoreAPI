using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTStoreAPI.Setup.Sieve
{
    public class SieveCustomSortMethods : ISieveCustomSortMethods
    {
        //public IQueryable<Post> Popularity(IQueryable<Post> source, bool useThenBy, bool desc) // The method is given an indicator of weather to use ThenBy(), and if the query is descending 
        //{
        //    var result = useThenBy ?
        //        ((IOrderedQueryable<Post>)source).ThenBy(p => p.LikeCount) : // ThenBy only works on IOrderedQueryable<TEntity>
        //        source.OrderBy(p => p.LikeCount)
        //        .ThenBy(p => p.CommentCount)
        //        .ThenBy(p => p.DateCreated);

        //    return result; // Must return modified IQueryable<TEntity>
        //}

        //public IQueryable<T> Oldest<T>(IQueryable<T> source, bool useThenBy, bool desc) where T : BaseEntity // Generic functions are allowed too
        //{
        //    var result = useThenBy ?
        //        ((IOrderedQueryable<T>)source).ThenByDescending(p => p.DateCreated) :
        //        source.OrderByDescending(p => p.DateCreated);

        //    return result;
        //}
    }
}

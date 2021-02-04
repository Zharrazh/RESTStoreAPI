using Sieve.Models;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTStoreAPI.Setup.Sieve
{
    public static class SeiveExtentions
    {
        public static IQueryable<M> ApplySorting<M> (this ISieveProcessor sieveProcessor, SieveModel sieveModel, IQueryable<M> source)
        {
            return sieveProcessor.Apply(sieveModel, source, applyPagination: false, applySorting:false);
        }

        public static PaginationResult<M> ApplyOrderingAndPagination<M>(this ISieveProcessor sieveProcessor, SieveModel sieveModel, IQueryable<M> source)
        {
            int totalItems = source.Count();
            var items = sieveProcessor.Apply(sieveModel, source, applyFiltering: false);
            int itemsInPage = items.Count();
            
            var formatSieveModel = SieveModelPageFormating(sieveModel);

            int totalPages = (int) Math.Ceiling(totalItems / (float)formatSieveModel.PageSize);
            return new PaginationResult<M>
            {
                Items = items,
                PageInfo = new PageInfo
                {
                    CurrentPage = formatSieveModel.Page,
                    PageSize = formatSieveModel.PageSize>itemsInPage?itemsInPage:formatSieveModel.PageSize,
                    TotalItems = totalItems,
                    TotalPages = totalPages
                }
            };
        }

        private static FormatSieveModelPage SieveModelPageFormating(SieveModel sieveModel)
        {
            int defaultPageSize = 20;
            int maxPageSize = 50;
            int pageSize = defaultPageSize;
            if (sieveModel.PageSize != null && sieveModel.PageSize <= maxPageSize)
                pageSize = sieveModel.PageSize ?? defaultPageSize;

            return new FormatSieveModelPage
            {
                Page = sieveModel.Page ?? 1,
                PageSize = pageSize
            };
        }


    }

    class FormatSieveModelPage
    {
        public int Page { get; set; }
        public int PageSize { get; set; }

    }


    public class PaginationResult<M>
    {
        public IQueryable<M> Items { get; set; }

        public PageInfo PageInfo { get; set; }
    }
    public class PageInfo
    {
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }

    }
}

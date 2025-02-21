using CodeSphere.Domain.Premitives;
using Elastic.Clients.Elasticsearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Application.Helpers
{
    static public class ElasticHelper
    {
        static public string GetSortField(SortBy sortBy)
        {
            return sortBy switch
            {
                SortBy.Difficulty => "difficulty",
                SortBy.AcceptanceRate => "acceptanceRate",
                _ => "name.keyword"
            };
        }
    }
}

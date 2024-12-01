using AutoMapper;
using CodeSphere.Application.Features.Problem.Queries.GetAll;
using CodeSphere.Domain.Abstractions;
using CodeSphere.Domain.Responses.ElasticSearchResponses;
using Elastic.Clients.Elasticsearch.Security;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Application.Mapping.Resolvers
{
    //public class GetProblemsResponseResolvers : IValueResolver<ProblemDocument, GetAllQueryResponse,bool>
    //{
    //    private readonly IUnitOfWork _unitOfWork;
    //    //private readonly HttpContext _httpContext;


    //    public GetProblemsResponseResolvers(IUnitOfWork unitOfWork, HttpContext httpContext)
    //    {
    //        this._unitOfWork = unitOfWork;
    //        //this._httpContext = httpContext;
    //    }

    //    public bool Resolve(ProblemDocument source, GetAllQueryResponse destination, bool destMember, ResolutionContext context)
    //    { 
    //        //var request = _httpContext.Request;
    //        //var userId = request.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
    //        //var submission =  _unitOfWork.SubmissionRepository.GetSolvedSubmissions(source.Id, userId);
    //        //return submission != null;
       
    //        return false;
    //    }

    //}
}

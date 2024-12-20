﻿using CodeSphere.Domain.Models.Entities;

namespace CodeSphere.Domain.Responses.SubmissionResponses
{
    public class BaseSubmissionResponse
    {
        public decimal ExecutionTime { get; set; }
        public string Code { get; set; }
        public DateTime SubmissionDate { get; set; } = DateTime.UtcNow;
        public SubmissionResult SubmissionResult { get; set; } = SubmissionResult.Accepted;
    }
}
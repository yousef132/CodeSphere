﻿using CodeSphere.Domain.Models.Entities;
using CodeSphere.Domain.Premitives;

namespace CodeSphere.Domain.Responses.Contest
{
    public class ContestProblemResponse
    {
        // all these properties are cached for the problems while the contest 
        public int Id { get; set; }
        public string Name { get; set; }
        public Difficulty Difficulty { get; set; }

        public decimal RunTimeLimit { get; set; }
        public MemoryLimit MemoryLimit { get; set; }

        public string Description { get; set; }
        public ContestPoints ContestPoints { get; set; }
    }
}

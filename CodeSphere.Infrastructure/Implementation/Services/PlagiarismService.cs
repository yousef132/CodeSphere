using CodeSphere.Domain.Abstractions;
using CodeSphere.Domain.Abstractions.Services;
using CodeSphere.Domain.DTOs;
using CodeSphere.Domain.Models.Entities;
using Docker.DotNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CodeSphere.Infrastructure.Implementation.Services
{
    public class PlagiarismService : IPlagiarismService
    {
        const uint FNV_PRIME = 16777619;  // https://en.wikipedia.org/wiki/Fowler%E2%80%93Noll%E2%80%93Vo_hash_function#FNV_hash_parameters
        const uint OFFSET_BASIS = 2166136261;
        const uint N = 6;
        const uint WINDOW_SIZE = 5;


        private readonly IUnitOfWork unitOfWork;

        public PlagiarismService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<PlagiarismCaseDTO>> GetPlagiarismCases(int contestId, List<int> ProblemIds, int thresold)
        {
            var submissions = await unitOfWork.SubmissionRepository.GetContestACSubmissionsByProblemIdsAsync(contestId, ProblemIds);
            List<PlagiarismCaseDTO> plagiarismCases = new List<PlagiarismCaseDTO>();

            // divide submissions into groups of same problem 

            var groups = submissions.GroupBy(s => s.ProblemId);


            foreach (var group in groups)
            {
                var subList = group.ToList();
                for (int i = 0; i < subList.Count; i++)
                {
                    for (int j = i + 1; j < subList.Count; j++)
                    {
                        if (subList[i].UserId == subList[j].UserId)
                            continue;

                        var similarity = CalculateJaccardSimilarity(subList[i].Code, subList[j].Code);
                        if (similarity >= thresold)
                        {
                            plagiarismCases.Add(new PlagiarismCaseDTO
                            {
                                FirstSubmission = subList[i],
                                SecondSubmission = subList[j],
                                Similarity = similarity,
                                ProblemId = subList[i].ProblemId
                            });
                        }
                    }
                }
            }

            return plagiarismCases;
        }

        private decimal CalculateJaccardSimilarity(string code1, string code2)
        {
            code1 = PreProcess(code1);
            code2 = PreProcess(code2);

            var ngrams1 = GenerateN_Grams(code1);
            var ngrams2 = GenerateN_Grams(code2);

            var hashes1 = HashN_grams(ngrams1);
            var hashes2 = HashN_grams(ngrams2);

            var fingerprints1 = GetFingerPrints(hashes1);
            var fingerprints2 = GetFingerPrints(hashes2);

            var intersection = fingerprints1.Intersect(fingerprints2).Count();
            var union = fingerprints1.Union(fingerprints2).Count();

            return (decimal)intersection / union * 100;
        }

        private string PreProcess(string code)
        {
            code = code.ToLower();

            code = Regex.Replace(code, @"(//.*?$)|(/\*.*?\*/)", "", RegexOptions.Multiline);
            code = Regex.Replace(code, @"\s+", "");

            code = code.Replace(" ", "").Replace("\n", "").Replace("\t", "");


            return code;
        }


        private List<string> GenerateN_Grams(string code)
        {
            List<string> ngrams = new List<string>();
            for (int i = 0; i < code.Length - N + 1; i++)
            {
                ngrams.Add(code.Substring(i, (int)N));
            }
            return ngrams;
        }


        private uint Fnv_1(string word)
        {
            uint hash = OFFSET_BASIS;
            foreach (var c in word)
            {
                hash ^= c;
                hash *= FNV_PRIME;
            }
            return hash;
        }

        private List<uint> HashN_grams(List<string> ngrams)
        {
            List<uint> hashes = new List<uint>();
            foreach (var ngram in ngrams)
            {
                hashes.Add(Fnv_1(ngram));
            }
            return hashes;
        }

        private static List<uint> GetFingerPrints(List<uint> hashs)
        {
            List<uint> fingerPrints = new List<uint>();
            SortedSet<(uint value, uint index)> window = new SortedSet<(uint, uint)>();
            Dictionary<(uint, uint), uint> count = new Dictionary<(uint, uint), uint>();

            uint i = 0;
            for (; i < WINDOW_SIZE; i++)
            {
                window.Add((hashs[(int)i], i));
            }

            fingerPrints.Add(window.Min.value);
            count[window.Min] = 1;

            for (; i < hashs.Count; i++)
            {
                window.Add((hashs[(int)i], i));
                window.Remove((hashs[(int)(i - WINDOW_SIZE)], i - WINDOW_SIZE));

                if (!count.ContainsKey(window.Min))
                    count[window.Min] = 0;
                count[window.Min]++;

                if (count[window.Min] == 1)
                {
                    fingerPrints.Add(window.Min.value);
                }
            }

            return fingerPrints;
        }

        public decimal GetSimilarity(string code1, string code2)
        {
            return CalculateJaccardSimilarity(code1, code2);
        }
    }
}

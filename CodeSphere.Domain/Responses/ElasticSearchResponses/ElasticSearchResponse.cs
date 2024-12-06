namespace CodeSphere.Domain.Responses.ElasticSearchResponses
{

    public class ElasticSearchResponse<T>
    {
        public int Took { get; set; }
        public bool TimedOut { get; set; }
        public ShardInfo Shards { get; set; }
        public HitData Hits { get; set; }
    }

    public class ShardInfo
    {
        public int Total { get; set; }
        public int Successful { get; set; }
        public int Skipped { get; set; }
        public int Failed { get; set; }
    }

    public class HitData
    {
        public TotalHits Total { get; set; }
        public List<Hit<ProblemDocument>> Hits { get; set; }
    }

    public class TotalHits
    {
        public int Value { get; set; }
        public string Relation { get; set; }
    }

    public class Hit<T>
    {
        public string _index { get; set; }
        public string _id { get; set; }
        public double _score { get; set; }
        public T _source { get; set; }
    }
}

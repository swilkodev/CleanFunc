namespace CleanFunc.Application.Issuers.Queries.ExportIssuers
{
    public class ExportIssuersResponse
    {
        public string FileName { get; set; }

        public string ContentType { get; set; }

        public byte[] Content { get; set; }
    }
}
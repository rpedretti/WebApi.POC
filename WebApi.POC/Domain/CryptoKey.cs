namespace WebApi.POC.Domain
{
    public class CryptoKey
    {
        public int Id { get; set; }

        public int KindId { get; set; }
        public virtual KeyKind Kind { get; set; }
        public string Value { get; set; }
    }
}

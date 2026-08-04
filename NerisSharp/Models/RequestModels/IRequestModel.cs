namespace NerisSharp.Models.RequestModels
{
    public interface IRequestModel
    {
        public string CreateQueryURI(string baseUrl);
        public bool Validate();
    }
}

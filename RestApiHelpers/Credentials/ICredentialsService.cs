using Abp.Domain.Services;
using RestApiHelpers.Contracts.Output;

namespace RestApiHelpers.Credentials
{
    public interface ICredentialsService : IDomainService
    {
        RestApiCredentials GetRestApiCredentials(RestApiCredentialsRequest input);
    }
}

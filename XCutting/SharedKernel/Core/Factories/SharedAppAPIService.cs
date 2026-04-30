using Bfar.XCutting.Abstractions.Entities.Dtos;
using Bfar.XCutting.Abstractions.Entities.Models;
using Bfar.XCutting.Abstractions.SharedKernel;
using Microsoft.Extensions.Logging;

namespace Bfar.XCutting.SharedKernel.Core.Factories
{
    public sealed class SharedAppAPIService : ISharedKernel
    {
        private IRestClientService rest;
        private SharedKernelPortConfig sharedKernelPortConfig;
        private ILoggerFactory loggerFactory;

        public SharedAppAPIService(IRestClientService rest, SharedKernelPortConfig sharedKernelPortConfig, ILoggerFactory loggerFactory)
        {
            this.rest = rest;
            this.sharedKernelPortConfig = sharedKernelPortConfig;
            this.loggerFactory = loggerFactory;
        }

        public Task<IEnumerable<object>?> GetAllCitiesAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<object>?> GetAllProvincesAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<object>?> GetAllRegionAsync(int cityId = 0, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<string?> GetAPIGatewayTokenAsync(int AppId, string authorization, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<object>?> GetBrandsAsync(GeneralModel model, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<object>> GetCountScalesAsync(GeneralModel model, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<object>> GetInventoryItemStatusesAsync(GeneralModel model, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<object>?> GetUserAccessibleAttributesAsync(GeneralModel model, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MenuDto>?> GetUserMenuAsync(GeneralModel model, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<object?> GetUserRolesAndCityAsync(GeneralModel model, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasRoleAsync(object model, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
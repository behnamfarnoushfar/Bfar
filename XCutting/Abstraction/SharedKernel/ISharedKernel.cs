using Bfar.XCutting.Abstractions.Entities.Dtos;
using Bfar.XCutting.Abstractions.Entities.Models;

namespace Bfar.XCutting.Abstractions.SharedKernel
{
    public interface ISharedKernel
    {
        Task<IEnumerable<object>?> GetAllRegionAsync(int cityId = 0, CancellationToken cancellationToken = default);
        Task<IEnumerable<object>?> GetAllCitiesAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<object>?> GetAllProvincesAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<MenuDto>?> GetUserMenuAsync(GeneralModel model, CancellationToken cancellationToken = default);
        Task<object?> GetUserRolesAndCityAsync(GeneralModel model, CancellationToken cancellationToken = default);
        Task<IEnumerable<object>?> GetUserAccessibleAttributesAsync(GeneralModel model, CancellationToken cancellationToken = default);
        Task<bool> HasRoleAsync(object model, CancellationToken cancellationToken = default);
        Task<IEnumerable<object>> GetCountScalesAsync(GeneralModel model, CancellationToken cancellationToken = default);
        Task<IEnumerable<object>> GetInventoryItemStatusesAsync(GeneralModel model, CancellationToken cancellationToken = default);
        Task<IEnumerable<object>?> GetBrandsAsync(GeneralModel model, CancellationToken cancellationToken = default);
        Task<string?> GetAPIGatewayTokenAsync(int AppId, string authorization, CancellationToken cancellationToken = default);
    }
}

using Conduit.Application.Features.Account.Commands.Register;
using Conduit.Application.Features.Device.Commands.Create;
using Conduit.Application.Features.Device.Queries.GetDevices;
using Conduit.Domain.Common;

namespace Conduit.Application.Contracts.Providers;

public interface IIdentityUserProvider
{
    Task<Result<RegisterDto>> RegisterAsync(string username, string email, string password, CancellationToken ct = default);
    Task<Result> VerifyAsync(Guid userId, string verificationCode, CancellationToken ct = default);
    Task<Result> ResendVerificationCodeAsync(Guid userId, int codeType, Guid? nonceId, string? nonce, CancellationToken ct = default);

    Task<Result<CreateDeviceDto>> CreateDeviceAsync(
        Guid userId,
        Guid loginNonceId,
        string loginNonce,
        string deviceAppVersion,
        int deviceType,
        int devicePlatform,
        string base64DevicePublicKey,
        CancellationToken ct = default);

    Task<Result<GetDevicesDto>> GetDevicesAsync(CancellationToken ct = default);

    Task<Result> DeleteDeviceAsync(Guid targetDeviceId, CancellationToken ct = default);
}

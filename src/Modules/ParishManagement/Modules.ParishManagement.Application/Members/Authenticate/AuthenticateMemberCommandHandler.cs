using Ardalis.Result;
using BuildingBlocks.Application;
using Microsoft.Extensions.Logging;
using Modules.IdentityProvider.Endpoints.PublicAPI;
using Modules.IdentityProvider.Endpoints.PublicAPI.Requests;
using Modules.ParishManagement.Application.Members.Specifications;
using Modules.ParishManagement.Domain.Abstractions;

namespace Modules.ParishManagement.Application.Members.Authenticate;

internal class AuthenticateMemberCommandHandler(
    IIdentityProviderEndpoints userAccess,
    IMemberRepository MemberRepository,
    ILogger<AuthenticateMemberCommandHandler> logger)
    : ICommandHandler<AuthenticateMemberCommand, AuthenticateMemberResponse>
{
    private readonly IIdentityProviderEndpoints _userAccess = userAccess;
    private readonly IMemberRepository _MemberRepository = MemberRepository;
    private readonly ILogger<AuthenticateMemberCommandHandler> _logger = logger;

    public async Task<Result<AuthenticateMemberResponse>> Handle(AuthenticateMemberCommand request, CancellationToken cancellationToken)
    {
        var spec = new GetMemberByEmailReadOnlySpecification(request.Email);

        var Member = await _MemberRepository.FirstOrDefaultAsync(spec, cancellationToken);

        _logger.LogInformation("Usuário encontrado: {Member}", Member?.Email);

        if (Member == null)
            return Result.Error("Usuário não encontrado");

        var result = await _userAccess.Login(new LoginRequest(request.Email, request.Password), cancellationToken);

        return result.Map(result =>
            new AuthenticateMemberResponse(
                new AuthenticatedMember(
                    result.User.FullName,
                    result.User.Email),
                    result.Token));
    }
}

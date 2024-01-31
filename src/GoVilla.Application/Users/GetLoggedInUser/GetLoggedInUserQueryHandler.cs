using Dapper;
using GoVilla.Application.Abstractions.Authentication;
using GoVilla.Application.Abstractions.Data;
using GoVilla.Application.Abstractions.Messaging;
using GoVilla.Domain.Abstractions;

namespace GoVilla.Application.Users.GetLoggedInUser;

public sealed class GetLoggedInUserQueryHandler : IQueryHandler<GetLoggedInUserQuery, UserResponse>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    private readonly IUserContext _userContext;

    public GetLoggedInUserQueryHandler(ISqlConnectionFactory sqlConnectionFactory, IUserContext userContext)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
        _userContext = userContext;
    }

    public async Task<Result<UserResponse>> Handle(GetLoggedInUserQuery request, CancellationToken cancellationToken)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();
        const string sql = """
            SELECT
                id AS Id,
                name AS Name,
                email AS Email
            FROM users
            WHERE identity_id = @IdentityId
            """;
        var user = await connection.QuerySingleAsync<UserResponse>(sql, new { _userContext.IdentityId });

        return user;
    }
}
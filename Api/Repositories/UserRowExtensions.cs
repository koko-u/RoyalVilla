using System.Collections.Generic;
using System.Linq;
using RoyalVilla.Api.Dto;

namespace RoyalVilla.Api.Repositories;

/// <summary>
/// User with Role data extension class
/// </summary>
public static class UserRowExtensions
{
    /// <summary>
    /// Modify list of user and row data into user with roles data
    /// </summary>
    /// <param name="rows"></param>
    /// <returns></returns>
    public static IEnumerable<UserData> GroupedByRole(this IEnumerable<UserAndRoleRow> rows)
    {
        return rows.GroupBy(x => x.ExtractUserRow())
            .Select(g => new UserData()
            {
                Id = g.Key.Id,
                DisplayName = g.Key.DisplayName,
                Email = g.Key.Email,
                IsActive = g.Key.IsActive,
                Roles =
                    g.Where(x => x.RoleId.HasValue)
                        .Select(x => new RoleData
                        {
                            Id = x.RoleId.GetValueOrDefault(),
                            Name = x.RoleName ?? string.Empty
                        })
                        .ToArray(),
            });
    }
}
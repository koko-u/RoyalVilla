using System.Data.Common;
using Npgsql;

namespace RoyalVilla.Application.Shared.Tx;

public readonly record struct DbSession(NpgsqlConnection conn, DbTransaction tx);

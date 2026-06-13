using KozLibraries.DapperDateOnlySupport;

namespace RoyalVilla.Infrastructure;

public static class Infrastructure
{
    public static void Setup()
    {
        // Dapper snake_case to PascalCase property mapping configuration
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

        // DateOnly Type Handler
        Dapper.SqlMapper.AddTypeHandler(new DateOnlyHandler());
    }
}

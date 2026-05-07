namespace RoyalVilla.Api.Features.Villas.RequestData;

/// <summary>
/// PageQuery class's offset and limit extension methods
/// </summary>
public static class PageQueryExtension
{
    extension(PageQuery pageQuery)
    {
        /// <summary>
        /// offset for a database query
        /// </summary>
        public int Offset => (pageQuery.Page - 1) * pageQuery.PageSize;

        /// <summary>
        /// limit for a database query
        /// </summary>
        public int Limit => pageQuery.PageSize;
    }
}

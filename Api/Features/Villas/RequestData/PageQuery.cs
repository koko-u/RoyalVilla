using System.ComponentModel;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace RoyalVilla.Api.Features.Villas.RequestData;

/// <summary>
/// ページネーションのためのクエリオブジェクトです
/// </summary>
public sealed class PageQuery
{
    /// <summary>
    /// page number for query ( 1 start )
    /// </summary>
    [FromQuery(Name = "page")]
    [Description("page number for query ( 1 start )")]
    public int Page { get; set; } = 1;

    /// <summary>
    /// page size for query ( default 20 )
    /// </summary>
    [FromQuery(Name = "page_size")]
    [Description("page size for query ( default 20 )")]
    public int PageSize { get; set; } = 20;
}

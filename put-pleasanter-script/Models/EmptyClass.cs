using System;
using System.Text.Json.Serialization;

using System;
using System.Collections.Generic;

using System.Text.Json;
using System.Globalization;


namespace PutPleasanterScript.Models
{

    public partial class PleasanterApiResponse
    {
        [JsonPropertyName("StatusCode")]
        public long StatusCode { get; set; }

        [JsonPropertyName("Response")]
        public Response? Response { get; set; }
    }

    public partial class Response
    {
        [JsonPropertyName("Data")]
        public Data? Data { get; set; }
    }

    public partial class Data
    {
        [JsonPropertyName("TenantId")]
        public long? TenantId { get; set; }

        [JsonPropertyName("SiteId")]
        public long? SiteId { get; set; }

        [JsonPropertyName("UpdatedTime")]
        public DateTimeOffset? UpdatedTime { get; set; }

        [JsonPropertyName("Ver")]
        public long? Ver { get; set; }

        [JsonPropertyName("Title")]
        public string? Title { get; set; }

        [JsonPropertyName("Body")]
        public string? Body { get; set; }

        [JsonPropertyName("SiteName")]
        public string? SiteName { get; set; }

        [JsonPropertyName("SiteGroupName")]
        public string? SiteGroupName { get; set; }

        [JsonPropertyName("GridGuide")]
        public string? GridGuide { get; set; }

        [JsonPropertyName("EditorGuide")]
        public string? EditorGuide { get; set; }

        [JsonPropertyName("CalendarGuide")]
        public string? CalendarGuide { get; set; }

        [JsonPropertyName("CrosstabGuide")]
        public string? CrosstabGuide { get; set; }

        [JsonPropertyName("GanttGuide")]
        public string? GanttGuide { get; set; }

        [JsonPropertyName("BurnDownGuide")]
        public string? BurnDownGuide { get; set; }

        [JsonPropertyName("TimeSeriesGuide")]
        public string? TimeSeriesGuide { get; set; }

        [JsonPropertyName("KambanGuide")]
        public string? KambanGuide { get; set; }

        [JsonPropertyName("ImageLibGuide")]
        public string? ImageLibGuide { get; set; }

        [JsonPropertyName("ReferenceType")]
        public string? ReferenceType { get; set; }

        [JsonPropertyName("ParentId")]
        public long? ParentId { get; set; }

        [JsonPropertyName("InheritPermission")]
        public long? InheritPermission { get; set; }

        [JsonPropertyName("Permissions")]
        public object[]? Permissions { get; set; }

        [JsonPropertyName("SiteSettings")]
        public SiteSettings? SiteSettings { get; set; }

        [JsonPropertyName("Publish")]
        public bool? Publish { get; set; }

        [JsonPropertyName("DisableCrossSearch")]
        public bool? DisableCrossSearch { get; set; }

        [JsonPropertyName("LockedTime")]
        public DateTimeOffset? LockedTime { get; set; }

        [JsonPropertyName("LockedUser")]
        public long? LockedUser { get; set; }

        [JsonPropertyName("ApiCountDate")]
        public DateTimeOffset? ApiCountDate { get; set; }

        [JsonPropertyName("ApiCount")]
        public long? ApiCount { get; set; }

        [JsonPropertyName("Comments")]
        public string? Comments { get; set; }

        [JsonPropertyName("Creator")]
        public long? Creator { get; set; }

        [JsonPropertyName("Updator")]
        public long? Updator { get; set; }

        [JsonPropertyName("CreatedTime")]
        public DateTimeOffset? CreatedTime { get; set; }

        [JsonPropertyName("ApiVersion")]
        public double? ApiVersion { get; set; }

        [JsonPropertyName("ClassHash")]
        public Hash? ClassHash { get; set; }

        [JsonPropertyName("NumHash")]
        public Hash? NumHash { get; set; }

        [JsonPropertyName("DateHash")]
        public Hash? DateHash { get; set; }

        [JsonPropertyName("DescriptionHash")]
        public Hash? DescriptionHash { get; set; }

        [JsonPropertyName("CheckHash")]
        public Hash? CheckHash { get; set; }

        [JsonPropertyName("AttachmentsHash")]
        public Hash? AttachmentsHash { get; set; }
    }

    public partial class Hash
    {
    }

    public partial class SiteSettings
    {
        [JsonPropertyName("Version")]
        public double? Version { get; set; }

        [JsonPropertyName("ReferenceType")]
        public string? ReferenceType { get; set; }

        [JsonPropertyName("EditorColumnHash")]
        public EditorColumnHash? EditorColumnHash { get; set; }

        [JsonPropertyName("Columns")]
        public Column[]? Columns { get; set; }

        [JsonPropertyName("Scripts")]
        public Script[]? Scripts { get; set; }

        [JsonPropertyName("NoDisplayIfReadOnly")]
        public bool? NoDisplayIfReadOnly { get; set; }
    }

    public partial class Column
    {
        [JsonPropertyName("ColumnName")]
        public string? ColumnName { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("Description")]
        public string? Description { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("ChoicesText")]
        public string? ChoicesText { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("EditorFormat")]
        public string? EditorFormat { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("LabelText")]
        public string? LabelText { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("NoWrap")]
        public bool? NoWrap { get; set; }
    }

    public partial class EditorColumnHash
    {
        [JsonPropertyName("General")]
        public string[]? General { get; set; }
    }

    public partial class Script
    {
        [JsonPropertyName("Title")]
        public string? Title { get; set; }

        [JsonPropertyName("New")]
        public bool? New { get; set; }

        [JsonPropertyName("Edit")]
        public bool? Edit { get; set; }

        [JsonPropertyName("Index")]
        public bool? Index { get; set; }

        [JsonPropertyName("Calendar")]
        public bool? Calendar { get; set; }

        [JsonPropertyName("Crosstab")]
        public bool? Crosstab { get; set; }

        [JsonPropertyName("Gantt")]
        public bool? Gantt { get; set; }

        [JsonPropertyName("BurnDown")]
        public bool? BurnDown { get; set; }

        [JsonPropertyName("TimeSeries")]
        public bool? TimeSeries { get; set; }

        [JsonPropertyName("Kamban")]
        public bool? Kamban { get; set; }

        [JsonPropertyName("ImageLib")]
        public bool? ImageLib { get; set; }

        [JsonPropertyName("Body")]
        public string? Body { get; set; }

        [JsonPropertyName("Id")]
        public long? Id { get; set; }
    }
}


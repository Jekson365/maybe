using System.ComponentModel;

namespace TappApi.Enums;

public enum ItemTypeEnum
{
    [Description("artist")]
    Artist,
    [Description("album")]
    Album,
    [Description("track")]
    Track,
    [Description("playlist")]
    Playlist
}
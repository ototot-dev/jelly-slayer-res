using UnityEngine;

namespace Alfish.StickerStyle.BasicPack {
  public class CursorStart : MonoBehaviour
  {
    [Tooltip(
      "The texture to use for the cursor or \"None\" to set the default cursor."
      + " Note that a texture needs to be imported with \"Read/Write enabled\" in the texture importer"
      + " (or using the \"Cursor\" defaults), in order to be used as a cursor."
    )]
    public Texture2D cursor;
    [Tooltip(
      "Allow the cursors to render as hardware cursors on supported platforms, or force software cursors.\n"
      + "It is recommended to force software cursors if their size is greater than 32x32 px."
    )]
    public CursorMode cursorMode;
    [Tooltip(
      "The offset from the top left of the texture to use as the target point."
      + " Must be within the bounds of the cursor.\n"
      + "* This value is stored relative to the texture size."
    )]
    public Vector2 hotspot;
    void Start() {
      Cursor.SetCursor(cursor, new Vector2(hotspot.x * cursor.width, hotspot.y * cursor.height), cursorMode);
    }
  }
}

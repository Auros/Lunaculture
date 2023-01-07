using Lunaculture.Items;
using UnityEditor;
using UnityEngine;

namespace Lunaculture.Editor
{
    [CustomEditor(typeof(Item))]
    public class ItemEditor : UnityEditor.Editor
    {
        public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
        {
            var item = serializedObject.targetObject as Item;

            return item.Icon != null
                ? item.Icon.texture
                : null;
        }
    }
}

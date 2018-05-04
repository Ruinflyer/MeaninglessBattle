using UnityEngine;


[ExecuteInEditMode]
[AddComponentMenu("Scripts/Camera/Chunky")]
public class Chunky : MonoBehaviour
{
    public Shader SCShader;
    public Texture2D SprTex;
    public Color Color = Color.white;
    private Material SCMaterial;


    Material material
    {
        get
        {
            if (SCMaterial == null)
            {
                SCMaterial = new Material(SCShader);
                SCMaterial.hideFlags = HideFlags.HideAndDontSave;
            }
            return SCMaterial;
        }
    }
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        float w = Camera.main.pixelWidth;
        float h = Camera.main.pixelHeight;
        Vector2 count = new Vector2(w / SprTex.height, h / SprTex.height);
        Vector2 size = new Vector2(1.0f / count.x, 1.0f / count.y);
        //
        material.SetVector("BlockCount", count);
        material.SetVector("BlockSize", size);
        material.SetColor("_Color", Color);
        material.SetTexture("_SprTex", SprTex);
        Graphics.Blit(source, destination, material);
    }
}

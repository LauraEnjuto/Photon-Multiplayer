using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadingAnimation : MonoBehaviour
{
    #region Variables

    public float waveSpeed = 3f;
    public TMP_Text loadText;

    #endregion

    #region Unity Methods

    void Update()
    {
        //We need to ensure that the text mesh is updated
        loadText.ForceMeshUpdate();
        var textInfo = loadText.textInfo;

        //Loop through each char in the text
        for (int i = 0; i < textInfo.characterCount; i++)
        {
            var charInfo = textInfo.characterInfo[i];

            //If char is not visible, skip to the next one
            if (!charInfo.isVisible)
            {
                continue;
            }

            //Get the vertices of the current char
            var verts = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;

            //Apply the wave animation to each vertex of the current char
            for (int j = 0; j < 4; j++)
            {
                var orig = verts[charInfo.vertexIndex + j];
                verts[charInfo.vertexIndex + j] = orig + new Vector3(0, Mathf.Sin(Time.time * waveSpeed + orig.x * 0.01f) * 10f, 0);
            }
        }

        //Update the mesh for each char
        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            var meshInfo = textInfo.meshInfo[i];
            meshInfo.mesh.vertices = meshInfo.vertices;
            loadText.UpdateGeometry(meshInfo.mesh, i);
        }
    }

    #endregion
}

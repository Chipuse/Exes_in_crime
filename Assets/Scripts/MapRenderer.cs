using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRenderer
{
    Dictionary<InstancedMeshData, List<Matrix4x4>> renderData;

    public void DrawInstancedMeshes()
    {
        if (renderData == null)
            return;
        if (GameManager.instancedRendering)
        {
            foreach (var item in renderData)
            {
                if (item.Value.Count >= 1023)
                {
                    for (int i = 0; i * 1023 < item.Value.Count; i++)
                    {
                        if (item.Value.Count < 1023 * (i + 1))
                        {
                            if (item.Key.mesh.subMeshCount >= 1)
                            {
                                Graphics.DrawMeshInstanced(item.Key.mesh, 0, item.Key.mat1, item.Value.GetRange(i * 1023, item.Value.Count - i * 1023));
                            }
                            if (item.Key.mesh.subMeshCount >= 2)
                            {
                                Graphics.DrawMeshInstanced(item.Key.mesh, 1, item.Key.mat2, item.Value.GetRange(i * 1023, item.Value.Count - i * 1023));
                            }
                            if (item.Key.mesh.subMeshCount >= 3)
                            {
                                Graphics.DrawMeshInstanced(item.Key.mesh, 2, item.Key.mat3, item.Value.GetRange(i * 1023, item.Value.Count - i * 1023));
                            }
                            if (item.Key.mesh.subMeshCount >= 4)
                            {
                                Graphics.DrawMeshInstanced(item.Key.mesh, 3, item.Key.mat4, item.Value.GetRange(i * 1023, item.Value.Count - i * 1023));
                            }
                            if (item.Key.mesh.subMeshCount >= 5)
                            {
                                Graphics.DrawMeshInstanced(item.Key.mesh, 4, item.Key.mat5, item.Value.GetRange(i * 1023, item.Value.Count - i * 1023));
                            }
                        }
                        else
                        {
                            if (item.Key.mesh.subMeshCount >= 1)
                            {
                                Graphics.DrawMeshInstanced(item.Key.mesh, 0, item.Key.mat1, item.Value.GetRange(i * 1023, 1023));
                            }
                            if (item.Key.mesh.subMeshCount >= 2)
                            {
                                Graphics.DrawMeshInstanced(item.Key.mesh, 1, item.Key.mat2, item.Value.GetRange(i * 1023, 1023));
                            }
                            if (item.Key.mesh.subMeshCount >= 3)
                            {
                                Graphics.DrawMeshInstanced(item.Key.mesh, 2, item.Key.mat3, item.Value.GetRange(i * 1023, 1023));
                            }
                            if (item.Key.mesh.subMeshCount >= 4)
                            {
                                Graphics.DrawMeshInstanced(item.Key.mesh, 3, item.Key.mat4, item.Value.GetRange(i * 1023, 1023));
                            }
                            if (item.Key.mesh.subMeshCount >= 5)
                            {
                                Graphics.DrawMeshInstanced(item.Key.mesh, 4, item.Key.mat5, item.Value.GetRange(i * 1023, 1023));
                            }
                        }
                    }

                }
                else
                {
                    if (item.Key.mesh.subMeshCount >= 1)
                    {
                        foreach (var matrix in item.Value)
                        {
                            Graphics.DrawMesh(item.Key.mesh, matrix, item.Key.mat1, 0, Camera.main, 0);
                        }
                        Graphics.DrawMeshInstanced(item.Key.mesh, 0, item.Key.mat1, item.Value);
                    }
                    if (item.Key.mesh.subMeshCount >= 2)
                    {
                        foreach (var matrix in item.Value)
                        {
                            Graphics.DrawMesh(item.Key.mesh, matrix, item.Key.mat2, 0, Camera.main, 1);
                        }
                        Graphics.DrawMeshInstanced(item.Key.mesh, 1, item.Key.mat2, item.Value);
                    }
                    if (item.Key.mesh.subMeshCount >= 3)
                    {
                        foreach (var matrix in item.Value)
                        {
                            Graphics.DrawMesh(item.Key.mesh, matrix, item.Key.mat3, 0, Camera.main, 2);
                        }
                        Graphics.DrawMeshInstanced(item.Key.mesh, 2, item.Key.mat3, item.Value);
                    }
                    if (item.Key.mesh.subMeshCount >= 4)
                    {
                        foreach (var matrix in item.Value)
                        {
                            Graphics.DrawMesh(item.Key.mesh, matrix, item.Key.mat4, 0, Camera.main, 3);
                        }
                        Graphics.DrawMeshInstanced(item.Key.mesh, 3, item.Key.mat4, item.Value);
                    }
                    if (item.Key.mesh.subMeshCount >= 5)
                    {
                        foreach (var matrix in item.Value)
                        {
                            Graphics.DrawMesh(item.Key.mesh, matrix, item.Key.mat5, 0, Camera.main, 4);
                        }
                        Graphics.DrawMeshInstanced(item.Key.mesh, 4, item.Key.mat5, item.Value);
                    }
                }
            }
        }
        else
        {
            // not instanced meshes
            foreach (var item in renderData)
            {
                if (item.Key.mesh.subMeshCount >= 1)
                {
                    foreach (var matrix in item.Value)
                    {
                        Graphics.DrawMesh(item.Key.mesh, matrix, item.Key.mat1, 0, Camera.main, 0);
                    }
                }
                if (item.Key.mesh.subMeshCount >= 2)
                {
                    foreach (var matrix in item.Value)
                    {
                        Graphics.DrawMesh(item.Key.mesh, matrix, item.Key.mat2, 0, Camera.main, 1);
                    }
                }
                if (item.Key.mesh.subMeshCount >= 3)
                {
                    foreach (var matrix in item.Value)
                    {
                        Graphics.DrawMesh(item.Key.mesh, matrix, item.Key.mat3, 0, Camera.main, 2);
                    }
                }
                if (item.Key.mesh.subMeshCount >= 4)
                {
                    foreach (var matrix in item.Value)
                    {
                        Graphics.DrawMesh(item.Key.mesh, matrix, item.Key.mat4, 0, Camera.main, 3);
                    }
                }
                if (item.Key.mesh.subMeshCount >= 5)
                {
                    foreach (var matrix in item.Value)
                    {
                        Graphics.DrawMesh(item.Key.mesh, matrix, item.Key.mat5, 0, Camera.main, 4);
                    }
                }
            }
        }
    }

    public void ClearRenderData()
    {
        renderData = new Dictionary<InstancedMeshData, List<Matrix4x4>>();
    }

    public void AddMesh(InstancedMeshData _meshData, Matrix4x4 _transformMatrix)
    {
        if (renderData == null)
            renderData = new Dictionary<InstancedMeshData, List<Matrix4x4>>();
        //Add Matrix to existing to renderdata if InstancedMeshData already exists
        //Or create new entry with instancedMeshData key and then add
        if (!renderData.ContainsKey(_meshData))
        {
            renderData.Add(_meshData, new List<Matrix4x4>());
        }
        renderData[_meshData].Add(_transformMatrix);
    }

    public void RemoveMesh(InfoForDelete[] _meshesToDelete)
    {
        //Remove Matrix from InstancedMeshData
        //Make sure that array still works (resize, check for null, etc.)
        //If a list gets empty remove Key entry entirely
        foreach (var item in _meshesToDelete)
        {
            if (renderData.ContainsKey(item.instancedMeshData))
            {
                renderData[item.instancedMeshData].Remove(item.transformMatrix);
            }
        }    
    }

    public InfoForDelete[] BuildEntryFromPrefab(GameObject prefab, Transform mapOrigin)
    {
        //Unfold hierachy of the prefab and all its meshes
        //for each mesh + material combo create global Matrix4x4 (relation to parent objects and finally to the mapOrigin)
        //Perform the AddMesh for each combo
        List<InfoForDelete> AllMeshDataToAdd = new List<InfoForDelete>();
        //AllMeshDataToAdd.AddRange(RecursiveMeshFinder(prefab.transform, mapOrigin));

        Matrix4x4 mapMatrix = Matrix4x4.TRS(mapOrigin.localPosition, mapOrigin.localRotation, mapOrigin.localScale);
        MeshRenderer tempRend;
        MeshFilter tempFilter;
        Matrix4x4 tempTransformMatrix;
        Transform tempTRansform;
        tempTRansform = prefab.transform;
        tempTransformMatrix = mapMatrix * Matrix4x4.TRS(tempTRansform.localPosition, tempTRansform.localRotation, tempTRansform.localScale);
        if (tempRend = prefab.GetComponent<MeshRenderer>())
        {
            if (tempFilter = prefab.GetComponent<MeshFilter>())
            {
                List<Material> tempMats = new List<Material>();
                for (int index = 0; index < tempFilter.sharedMesh.subMeshCount; index++)
                {
                    tempMats.Add(tempRend.sharedMaterials[index]);
                }
                InstancedMeshData tempMeshData = new InstancedMeshData { mesh = tempFilter.sharedMesh, /*material = tempMats.ToArray()*/ };
                if (tempMats.Count >= 1)
                {
                    tempMeshData.mat1 = tempMats[0];
                }
                if (tempMats.Count >= 2)
                {
                    tempMeshData.mat2 = tempMats[1];
                }
                if (tempMats.Count >= 3)
                {
                    tempMeshData.mat3 = tempMats[2];
                }
                if (tempMats.Count >= 4)
                {
                    tempMeshData.mat4 = tempMats[3];
                }
                if (tempMats.Count >= 5)
                {
                    tempMeshData.mat5 = tempMats[4];
                }
                AllMeshDataToAdd.Add(new InfoForDelete { instancedMeshData = tempMeshData, transformMatrix =  tempTransformMatrix });
            }
        }

        for (int i = 0; i < prefab.transform.childCount; i++)
        {
            AllMeshDataToAdd.AddRange(RecursiveMeshFinder(prefab.transform.GetChild(i), tempTransformMatrix));
        }

        foreach (var item in AllMeshDataToAdd)
        {
            AddMesh(item.instancedMeshData, item.transformMatrix);
        }
        return AllMeshDataToAdd.ToArray();
    }

    List<InfoForDelete> RecursiveMeshFinder(Transform parent, Matrix4x4 matrixSoFar)
    {
        //Perform stuff
        List<InfoForDelete> AllMeshDataToAdd = new List<InfoForDelete>();
        MeshRenderer tempRend;
        MeshFilter tempFilter;
        Matrix4x4 tempTransformMatrix;
        Transform tempTRansform;
        tempTRansform = parent;
        tempTransformMatrix = matrixSoFar * Matrix4x4.TRS(tempTRansform.localPosition, tempTRansform.localRotation, tempTRansform.localScale);
        if (tempRend = parent.GetComponent<MeshRenderer>())
        {
            if (tempFilter = parent.GetComponent<MeshFilter>())
            {
                List<Material> tempMats = new List<Material>();
                for (int index = 0; index < tempFilter.sharedMesh.subMeshCount; index++)
                {
                    tempMats.Add(tempRend.sharedMaterials[index]);
                }
                InstancedMeshData tempMeshData = new InstancedMeshData { mesh = tempFilter.sharedMesh, /*material = tempMats.ToArray()*/ };
                if (tempMats.Count >= 1)
                {
                    tempMeshData.mat1 = tempMats[0];
                }
                if (tempMats.Count >= 2)
                {
                    tempMeshData.mat2 = tempMats[1];
                }
                if (tempMats.Count >= 3)
                {
                    tempMeshData.mat3 = tempMats[2];
                }
                if (tempMats.Count >= 4)
                {
                    tempMeshData.mat4 = tempMats[3];
                }
                if (tempMats.Count >= 5)
                {
                    tempMeshData.mat5 = tempMats[4];
                }
                AllMeshDataToAdd.Add(new InfoForDelete { instancedMeshData = tempMeshData, transformMatrix =  tempTransformMatrix });
            }
        }

        //call function with all children recursively
        for (int i = 0; i < parent.childCount; i++)
        {
            AllMeshDataToAdd.AddRange(RecursiveMeshFinder(parent.GetChild(i), tempTransformMatrix));
        }

        return AllMeshDataToAdd;
    } 
}

public struct InstancedMeshData
{
    public Mesh mesh;
    public Material mat1;
    public Material mat2;
    public Material mat3;
    public Material mat4;
    public Material mat5;
}

public struct InfoForDelete
{
    public InstancedMeshData instancedMeshData;
    public Matrix4x4 transformMatrix;
}

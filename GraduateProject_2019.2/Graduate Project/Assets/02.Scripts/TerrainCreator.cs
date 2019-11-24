using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainCreatorData
{
    public TerrainCreatorData(float height)
    {
        this.height = height;
        isSearched = false;
    }

    public TerrainCreatorData()
    {
        Clear();
    }

    public float height
    {
        get;
        set;
    }
    public bool isSearched
    {
        get;
        set;
    }

    public void Clear()
    {
        height = 0.0f;
        isSearched = false;
    }
}

public class TerrainQueueData
{
    public TerrainQueueData(int indexX, int indexY, float heigntToAdd)
    {
        this.indexX = indexX;
        this.indexY = indexY;

        this.heigntToAdd = heigntToAdd;
    }
    
    public int indexX;
    public int indexY;
    public float heigntToAdd;
    
}

public class TerrainCreator : MonoBehaviour
{
    [SerializeField]
    private Terrain terrain;

    [SerializeField]
    private float roughnessFactor =0.51f;
    // change 될경우 얼마나 올라갈지 지정

    [SerializeField]
    private int changeCount = 5;

    [SerializeField]
    private float defaultRandomHeightMax = 0.01f;

    List<TerrainCreatorData> terrainCreatorData;

    
    void Start()
    {
        terrainCreatorData = new List<TerrainCreatorData>();
        RandomGenerateTerrain();
    }
    
    void Update()
    {
        
    }


    public void ClearSearchInfo(List<TerrainCreatorData> terrainCreatorData)
    {
        int count = terrainCreatorData.Count;
        for(int i =0; i<count; ++i)
        {
            terrainCreatorData[i].isSearched = false;
        }
    }

    public float[,] getHeightDataFromTerrainCreatorData(List<TerrainCreatorData> terrainCreatorData)
    {
        if(null == terrainCreatorData)
        {
            return null;
        }

        int widthOrHeight = terrainCreatorData.Count;

        widthOrHeight = (int)Mathf.Sqrt(widthOrHeight);
        float[,] result = new float[widthOrHeight, widthOrHeight];

        for (int i = 0; i < widthOrHeight; i++)
        {
            for (int j = 0; j < widthOrHeight; j++)
            {
                result[i, j] = terrainCreatorData[i + j* widthOrHeight].height;
            }
        }

        return result;
    }

    public void saveHeightDataToTerrainCreatorData(float[,] data, int heightmapWidth, int heightmapHeight)
    {
        if(null == data)
        {
            return;
        }

        for (int i = 0; i < heightmapWidth; i++)
        {
            for (int j = 0; j < heightmapHeight; j++)
            {

                terrainCreatorData.Add(new TerrainCreatorData(data[i,j]));
            }
        }
    }

    void SaveTerrain(string filename)
    {

        float[,] temp = Terrain.activeTerrain.terrainData.GetHeights(0, 0, terrain.terrainData.heightmapWidth, terrain.terrainData.heightmapHeight);

        print("terrain width : " + terrain.terrainData.heightmapWidth);
        print("terrain height : " + terrain.terrainData.heightmapHeight);

        saveHeightDataToTerrainCreatorData(temp,terrain.terrainData.heightmapWidth, terrain.terrainData.heightmapHeight);
        //for (int i = 0; i < terrain.terrainData.heightmapWidth/30; i++)
        //{
        //    for (int j = 0; j < terrain.terrainData.heightmapHeight/30; j++)
        //    {
        //       print("heightmap height : " + i + " , " + j + " : " + terrainHeightMapdata[i, j]);
        //       // bw.Write(dat[i, j]);
        //    }
        //}


        //FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite);
        //BinaryWriter bw = new BinaryWriter(fs);
        //for (int i = 0; i < terrain.heightmapWidth; i++)
        //{
        //    for (int j = 0; j < terrain.heightmapHeight; j++)
        //    {
        //        bw.Write(dat[i, j]);
        //    }
        //}
        //bw.Close();
    }

    void LoadTerrain(string filename)
    {
        //float[,] dat = terrain.GetHeights(0, 0, terrain.heightmapWidth, terrain.heightmapHeight);
        //FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite);
        //BinaryReader br = new BinaryReader(fs);
        //br.BaseStream.Seek(0, SeekOrigin.Begin);
        //for (int i = 0; i < terrain.heightmapWidth; i++)
        //{
        //    for (int j = 0; j < terrain.heightmapHeight; j++)
        //    {
        //        dat[i, j] = (float)br.ReadSingle();
        //    }
        //}
        //br.Close();
        //terrain.SetHeights(0, 0, dat);
        //heights = terrain.GetHeights(50, 50, 100, 100);
    }

    void RandomGenerateTerrain()
    {
        terrainCreatorData.Clear();
        int terrainSize = terrain.terrainData.heightmapWidth * terrain.terrainData.heightmapHeight;
        for (int i =0; i< terrainSize; ++i)
        {
            // 랜덤하게 height를 1/4확률로 넣는다.
            float randomHeight = Random.Range(0.0f, defaultRandomHeightMax);
            int randomGen =(int) (randomHeight * 1000.0f);
            if (randomGen % 4 == 0)
            {
                terrainCreatorData.Add(new TerrainCreatorData(randomHeight));
            }
            else
            {
                terrainCreatorData.Add(new TerrainCreatorData());
            }

        }

        
        
        for(int i =0;i< changeCount; ++i)
        {
            int randomIndexX = (int)(terrain.terrainData.heightmapWidth * Random.Range(0.0f, 1.0f));
            int randomIndexY = (int)(terrain.terrainData.heightmapHeight * Random.Range(0.0f, 1.0f));

            // 자기자신부터 바뀌주고
            ChangeTerrainHeightMapData(terrainCreatorData, (int)Mathf.Sqrt(terrainCreatorData.Count), randomIndexX, randomIndexY, roughnessFactor);

            // BFS로 높이 변경
            ChangeNeighbors(terrainCreatorData, (int)Mathf.Sqrt(terrainCreatorData.Count), randomIndexX,  randomIndexY, roughnessFactor);

            // 다시 되돌려준다.
            for (int index =0; index < terrainSize; ++index)
            {
                terrainCreatorData[index].isSearched = false;
            }
        }
        

        float[,] convertedData = getHeightDataFromTerrainCreatorData(terrainCreatorData);
        if(null != convertedData)
        {
            terrain.terrainData.SetHeights(0, 0, convertedData);
        }

    }

    void ChangeNeighbors(List<TerrainCreatorData> terrainCreatorData, int widthOrHeight, int indexX, int indexY, float roughnessFactor)
    {
        if (null == terrainCreatorData)
        {
            return;
        }
        
        if (widthOrHeight < indexX || widthOrHeight < indexY)
        {
            return;
        }

        Queue<TerrainQueueData> queue = new Queue<TerrainQueueData>();
        
        float heigntToAdd = roughnessFactor - 0.1f;
        print("halfRoughnessFactor" + heigntToAdd.ToString());
        if (heigntToAdd <= 0.05f)
        {
            return;
        }

        queue.Enqueue(new TerrainQueueData(indexX, indexY + 1, heigntToAdd));
        queue.Enqueue(new TerrainQueueData(indexX, indexY - 1, heigntToAdd));
        queue.Enqueue(new TerrainQueueData(indexX + 1, indexY, heigntToAdd));
        queue.Enqueue(new TerrainQueueData(indexX + 1, indexY + 1, heigntToAdd));
        queue.Enqueue(new TerrainQueueData(indexX - 1, indexY, heigntToAdd));
        queue.Enqueue(new TerrainQueueData(indexX - 1, indexY - 1, heigntToAdd));

        TerrainQueueData qData;
        while (0 != queue.Count)
        {
            qData = queue.Dequeue();
            if(true == ChangeTerrainHeightMapData(terrainCreatorData, widthOrHeight, qData.indexX, qData.indexY, qData.heigntToAdd))
            {
                heigntToAdd = qData.heigntToAdd - 0.1f;

                if (heigntToAdd <= 0.05f)
                {
                    continue;
                }
                
                queue.Enqueue(new TerrainQueueData(qData.indexX, qData.indexY + 1, heigntToAdd));
                queue.Enqueue(new TerrainQueueData(qData.indexX, qData.indexY - 1, heigntToAdd));
                queue.Enqueue(new TerrainQueueData(qData.indexX + 1, qData.indexY, heigntToAdd));
                queue.Enqueue(new TerrainQueueData(qData.indexX + 1, qData.indexY + 1, heigntToAdd));
                queue.Enqueue(new TerrainQueueData(qData.indexX - 1, qData.indexY, heigntToAdd));
                queue.Enqueue(new TerrainQueueData(qData.indexX - 1, qData.indexY - 1, heigntToAdd));
            }
        }
    }

    bool ChangeTerrainHeightMapData(List<TerrainCreatorData> terrainCreatorData, int widthOrHeight, int indexX, int indexY, float roughnessFactor)
    {
        if(indexX < 0 || indexY < 0)
        {
            return false;
        }

        //print("시도전 : " + indexX + " , " + indexY);
        if (widthOrHeight <= indexX || widthOrHeight <= indexY)
        {
            return false;
        }

        if(terrainCreatorData[indexY * widthOrHeight + indexX].isSearched == true)
        {
            return false;
        }
        //print("성공 : " + indexX + " , " + indexY);
        terrainCreatorData[indexY * widthOrHeight + indexX].height += roughnessFactor;
        terrainCreatorData[indexY * widthOrHeight + indexX].isSearched = true;

        return true;

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 터레인 생성을 위한 데이터
// height : 실제 터레인 높이
// isSearched : 순차적으로 주변 높이도 올려주기 위한 변수 // 높이 적용을 한번만 하기 위한 변수
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

    // 만들어진 터레인을 고정
    [SerializeField]
    private bool fixedTerrain;

    // 생성할 산의 최대 높이
    [SerializeField]
    private float roughnessFactor;
    // change 될경우 얼마나 올라갈지 지정

    // 생성할 산의 개수
    [SerializeField]
    private int changeCount;

    // 처음 생성할때의 렌덤 최대 높이
    [SerializeField]
    private float defaultRandomHeightMax;

    // 처음 터레인을 생성할때의 높이
    [SerializeField]
    private float defaultHeight;

    // 0.0f ~ 1.0f사이의 값 // 경사 정도
    [SerializeField]
    private float slope;

    List<TerrainCreatorData> terrainCreatorData;

    // 배치에 사용할 프리팹
    // 나무 / 돌 / 캘 수 있는 자원들
    [SerializeField]
    private int treeCount;

    [SerializeField]
    private int rockCount;

    [SerializeField]
    private List<GameObject> treePrefabs = new List<GameObject>();
    [SerializeField]
    private List<GameObject> rockPrefabs = new List<GameObject>();


    void Start()
    {
        if(false == fixedTerrain)
        {
            terrainCreatorData = new List<TerrainCreatorData>();
            RandomGenerateTerrain();
        }


        // 랜덤 오브젝트 생성
        RandomImplantObjects();
    }
    
    void Update()
    {
        
    }

    public void RandomImplantObjects()
    {
        Vector3 terrainStart = terrain.GetPosition();
        Vector3 terrainEnd = terrainStart + terrain.terrainData.size;

        int randomPrefab = 0;
        if (0 != treePrefabs.Count)
        {
            for (int index = 0; index < treeCount; ++index)
            {
                randomPrefab = Random.Range(0, 1000);
                int prefabIndex = randomPrefab % (treePrefabs.Count);

                Vector3 implantPosition = new Vector3();
                implantPosition.x = Random.Range(terrainStart.x, terrainEnd.x);
                implantPosition.z = Random.Range(terrainStart.z, terrainEnd.z);
                implantPosition.y = terrain.SampleHeight(implantPosition) + terrain.GetPosition().y;

                int randomY = Random.Range(0, 360);
                Quaternion rot = new Quaternion();
                rot.eulerAngles = new Vector3(0, randomY, 0);

                Instantiate(treePrefabs[prefabIndex], implantPosition, rot);
            }
        }

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


    // 터레인 생성
    void RandomGenerateTerrain()
    {
        terrainCreatorData.Clear();

        // 랜덤한 지형 만들기 위한 처리
        {
            // 외부 터레인 사이즈를 받아와서 
            int terrainSize = terrain.terrainData.heightmapWidth * terrain.terrainData.heightmapHeight;
            for (int i = 0; i < terrainSize; ++i)
            {
                // 랜덤하게 height를 1/4확률로 넣는다.
                float randomHeight = Random.Range(0.0f, defaultRandomHeightMax);
                int randomGen = (int)(randomHeight * 1000.0f);
                if (randomGen % 4 == 0)
                {
                    terrainCreatorData.Add(new TerrainCreatorData(randomHeight + defaultHeight));
                }
                else
                {
                    terrainCreatorData.Add(new TerrainCreatorData(defaultHeight));
                }

            }
        }


        // 산들을 만들어 준다
        {
            for (int i = 0; i < changeCount; ++i)
            {
                // 터레인의 랜덤한 인덱스를 받아서 처리
                int randomIndexX = (int)(terrain.terrainData.heightmapWidth * Random.Range(0.0f, 1.0f));
                int randomIndexY = (int)(terrain.terrainData.heightmapHeight * Random.Range(0.0f, 1.0f));

                // 최대 산의 높이의 0.0배 ~ 1.0배까지 랜덤한 산의 높이를 만든다.
                float finalRoughnessFactor = Random.Range(0.0f, 1.0f) * roughnessFactor;

                MakeHigher(terrainCreatorData, (int)Mathf.Sqrt(terrainCreatorData.Count), randomIndexX, randomIndexY, finalRoughnessFactor, 1);
            }
        }


        //// 특정위치에 정해진 지형 추가
        //MakeHigher(terrainCreatorData, (int)Mathf.Sqrt(terrainCreatorData.Count), terrain.terrainData.heightmapWidth / 2, terrain.terrainData.heightmapHeight / 2, 0.5f, 1);

        // 만들어 두었던 데이터를 가지고 실제 터레인에 적용해준다.
        float[,] convertedData = getHeightDataFromTerrainCreatorData(terrainCreatorData);
        if (null != convertedData)
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
        
        float heigntToAdd = roughnessFactor - slope;
//        print("halfRoughnessFactor" + heigntToAdd.ToString());
        if (heigntToAdd <= 0.0f)
        {
            return;
        }

        // 주변의 노드들을 큐에 넣고
        queue.Enqueue(new TerrainQueueData(indexX, indexY + 1, heigntToAdd));
        queue.Enqueue(new TerrainQueueData(indexX, indexY - 1, heigntToAdd));
        queue.Enqueue(new TerrainQueueData(indexX + 1, indexY, heigntToAdd));
        queue.Enqueue(new TerrainQueueData(indexX + 1, indexY + 1, heigntToAdd));
        queue.Enqueue(new TerrainQueueData(indexX - 1, indexY, heigntToAdd));
        queue.Enqueue(new TerrainQueueData(indexX - 1, indexY - 1, heigntToAdd));

        TerrainQueueData qData;
        // 큐에 데이터가 없을떄까지 디큐를 하면서 점점 낮아지는 높이를 적용해준다.
        // 만약에 처음 탐색되었다면, 주변노드를 큐에 넣어준다.
        while (0 != queue.Count)
        {
            qData = queue.Dequeue();
            if(true == ChangeTerrainHeightMapData(terrainCreatorData, widthOrHeight, qData.indexX, qData.indexY, qData.heigntToAdd))
            {
                heigntToAdd = qData.heigntToAdd - slope;

                if (heigntToAdd <= 0.0f)
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

    bool MakeHigher(List<TerrainCreatorData> terrainCreatorData, int widthOrHeight, int indexX, int indexY, float factor, int count)
    {
        if (indexX < 0 || indexY < 0)
        {
            return false;
        }

        if (widthOrHeight <= indexX || widthOrHeight <= indexY)
        {
            return false;
        }

        int terrainSize = terrain.terrainData.heightmapWidth * terrain.terrainData.heightmapHeight;

        for(int i =0; i<count; ++i)
        {
            // 먼저 자기자신의 높이를 높여준다.
            ChangeTerrainHeightMapData(terrainCreatorData, widthOrHeight, indexX, indexY, factor);
            // 주변의 높이를 높여준다.
            ChangeNeighbors(terrainCreatorData, widthOrHeight, indexX, indexY, factor);

            // 한번 산이 만들어졌으면 탐색한 정보를 지워준다.
            for (int index = 0; index < terrainSize; ++index)
            {
                terrainCreatorData[index].isSearched = false;
            }
        }

        return true;
    }
    bool MakeLower(List<TerrainCreatorData> terrainCreatorData, int widthOrHeight, int indexX, int indexY, float factor, int count)
    {
        factor *= -1.0f;
        return MakeHigher(terrainCreatorData, widthOrHeight, indexX, indexY, factor, count);
    }

}

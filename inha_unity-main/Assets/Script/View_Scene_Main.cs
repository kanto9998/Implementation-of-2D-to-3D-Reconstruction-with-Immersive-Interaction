using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using Dummiesman;

public class View_Scene_Main : MonoBehaviour
{
    public GameObject       objMeshBase;
    public RawImage         backgroundImg;

    public GameObject       obj_BasketBall_Goal;
    public GameObject       obj_BasketBall;
    public GameObject       obj_BasketBall_FBX;

    public GameObject       obj_CatBase;
    public GameObject       obj_Cats;

    public GameObject       obj_Gear_Clone_Base;
    public GameObject       obj_Gear_Button_Base;

    public GameObject       obj_Gear_Chair;
    public GameObject       obj_Gear_Sofa;
    public GameObject       obj_Gear_Table;

    public GameObject       obj_Guide_Game1;
    public GameObject       obj_Guide_Game2;
    public GameObject       obj_Guide_Game3;

    private Vector3         vec3_cats_pos;

    public struct ObjectFile
    {
        public string o;
        public string mtllib;
        public List<string> usemtl;
        public List<Vector3> v;
        public List<Vector3> vn;
        public List<Vector2> vt;
        public List<List<int[]>> f;
    }

    public struct MaterialFile
    {
        public List<string> newmtl;
        public List<string> mapKd;
    }

    void Start()
    {
        //string path = Path.Combine(Application.streamingAssetsPath, "image/outsidetest.jpg");
        //string path2 = Path.Combine(Application.streamingAssetsPath, "image/outsidetest_mesh.obj");
        string path = Path.Combine(Application.streamingAssetsPath, "image/" + Game_Mgr.Instance.imageName );

        //mesh 파일
        string mesh_filename = Game_Mgr.Instance.imageName.Split( '.' ) [ 0 ] + "_mesh.obj";
        string path2 = Path.Combine( Application.streamingAssetsPath, "image/" + mesh_filename );
        string path3 = Path.Combine(Application.streamingAssetsPath, "image/");

        string image_path = "python_data/" + Game_Mgr.Instance.imageName;
        string mesh_path = "python_data/" + Game_Mgr.Instance.imageName.Split('.')[0] + "_mesh";

        /*
        byte[] byteTexture = File.ReadAllBytes(path);
        if (byteTexture.Length > 0)
        {
            Texture2D texture = new Texture2D(0, 0);
            texture.LoadImage(byteTexture);

            backgroundImg.texture = texture;
        }
        */

        //메쉬 파일 삽입
        //GameObject mesh = new GameObject();
        //byte[] byteMesh = File.ReadAllBytes( path2 );
        //if (byteMesh.Length > 0)
        //{
        //object obj = byteMesh.Clone();

        //mesh = (GameObject)ByteToObject( byteMesh );

        //Texture2D texture = new Texture2D(0, 0);
        //texture.LoadImage(byteTexture);

        //backgroundImg.texture = texture;
        //}

        //이미지 선택된걸로 교체
        backgroundImg.texture = Resources.Load<Texture2D>( image_path );


        GameObject ori_mesh = Resources.Load<GameObject>(mesh_path);
        Instantiate( ori_mesh, objMeshBase.transform );

        //GameObject ori_mesh = new OBJLoader().Load( path2 );
        //Instantiate( ori_mesh, objMeshBase.transform );
        //GameObject ori_mesh = new OBJLoader().Load(path2);
        //Instantiate(ori_mesh, objMeshBase.transform);

        //StartCoroutine(Run_Mesh_Init());

        vec3_cats_pos = obj_CatBase.transform.position;

        GameObject meshChild = objMeshBase.transform.GetChild(0).GetChild(0).gameObject;
        meshChild.AddComponent<MeshCollider>();
        meshChild.GetComponent<MeshRenderer>().enabled = false;

        //게임 초기화
        Game_Hide();
    }

    IEnumerator Run_Mesh_Init()
    {
        string mesh_filename = Game_Mgr.Instance.imageName.Split('.')[0] + "_mesh.obj";
        string path3 = Path.Combine(Application.streamingAssetsPath, "image/" + mesh_filename );

        // AssetBundle 파일을 비동기로 다운로드
        using (WWW www = new(path3))
        {
            yield return www;

            if (www.error != null)
            {
                Debug.Log("실패!");
            }
            else
            {
                // obj 파일의 내용을 문자열로 가져옵니다.
                string objContent = www.text;

                // obj 파일을 파싱하여 Mesh 생성
                //Mesh mesh = CreateMeshFromObjContent(objContent);

                ObjectFile obj = ReadObjectFile(objContent);

                // Mesh를 사용하여 GameObject 생성 또는 기존 GameObject에 Mesh 적용
                CreateGameObjectWithMesh( obj );

                /*
                byte[] objBytes = www.bytes;

                // byte 배열을 정점 정보와 삼각형 정보로 변환
                List<Vector3> vertices;
                List<int> triangles;
                ExtractVerticesAndTrianglesFromObjBytes(objBytes, out vertices, out triangles);

                // Unity의 Mesh 생성
                Mesh mesh = new Mesh();
                mesh.vertices = vertices.ToArray();
                mesh.triangles = triangles.ToArray();

                // Mesh를 사용하여 GameObject 생성 또는 기존 GameObject에 적용
                CreateGameObjectWithMesh(mesh);
                */
            }

            /*
            if (string.IsNullOrEmpty(www.error))
            {
                // AssetBundle을 불러온 후 .obj 모델을 로드
                AssetBundle assetBundle = www.assetBundle;
                GameObject loadedObject = Instantiate(assetBundle.LoadAsset(mesh_filename)) as GameObject;

                // Mesh Renderer에 할당
                MeshRenderer meshRenderer = loadedObject.GetComponent<MeshRenderer>();
                MeshFilter meshFilter = loadedObject.GetComponent<MeshFilter>();

                if (meshRenderer != null && meshFilter != null)
                {
                    // .obj 모델을 렌더링
                    meshRenderer.material = new Material(Shader.Find("Standard"));
                    // 여기에 추가적인 설정을 할 수 있습니다.

                    // 씬에 추가
                    Instantiate(loadedObject);
                }

                // AssetBundle 언로드
                assetBundle.Unload(false);
            }
            else
            {
                Debug.LogError("AssetBundle 로드 중 에러 발생: " + www.error);
            }
            */
        }

        /*
        GameObject meshChild = objMeshBase.transform.GetChild(0).GetChild(0).gameObject;
        meshChild.AddComponent<MeshCollider>();
        meshChild.GetComponent<MeshRenderer>().enabled = false;
        */
    }

    Mesh CreateMeshFromObjContent(string objContent)
    {
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        using (StringReader reader = new StringReader(objContent))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] parts = line.Split(' ');

                if (parts[0] == "v")
                {
                    // 정점 정보 추출
                    float x = float.Parse(parts[1]);
                    float y = float.Parse(parts[2]);
                    float z = float.Parse(parts[3]);
                    vertices.Add(new Vector3(x, y, z));
                }
                else if (parts[0] == "f")
                {
                    // 삼각형 정보 추출
                    for (int i = 1; i < 4; i++)
                    {
                        Debug.Log("triangles 1 : " + parts[i]);
                        string str = parts[i].Split("//")[0];

                        int vertexIndex;
                        if (int.TryParse(str, out vertexIndex))
                        {
                            triangles.Add(vertexIndex - 1);
                        }
                        //int vertexIndex = int.Parse(parts[i]) - 1; // 정점 인덱스는 1부터 시작하므로 1을 빼줌
                        //triangles.Add(vertexIndex);
                    }
                }
            }
        }

        // Unity의 Mesh 생성
        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();

        return mesh;
    }


    void ExtractVerticesAndTrianglesFromObjBytes(byte[] objBytes, out List<Vector3> vertices, out List<int> triangles)
    {
        vertices = new List<Vector3>();
        triangles = new List<int>();

        using (MemoryStream stream = new MemoryStream(objBytes))
        using (StreamReader reader = new StreamReader(stream))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] parts = line.Split(' ');

                if (parts[0] == "v")
                {

                    Debug.Log("vertices start");

                    // 정점 정보 추출
                    float x = float.Parse(parts[1]);
                    float y = float.Parse(parts[2]);
                    float z = float.Parse(parts[3]);
                    vertices.Add(new Vector3(x, y, z));

                    Debug.Log("vertices end");
                }
                else if (parts[0] == "f")
                {
                    Debug.Log("triangles start");

                    // 삼각형 정보 추출
                    for (int i = 1; i < 4; i++)
                    {
                        Debug.Log("triangles 1 : " + parts[ i ]);
                        string str = parts[i].Split("//")[ 0 ];

                        int vertexIndex;
                        if (int.TryParse(str, out vertexIndex))
                        {
                            triangles.Add(vertexIndex - 1);
                        }

                        /*
                        string[] str = parts[ i ].Split("//");
                        for(int j = 0; j < str.Length; j++)
                        {
                            int vertexIndex;
                            if (int.TryParse(str[ j ], out vertexIndex))
                            {
                                Debug.Log("triangles 2");
                                triangles.Add(vertexIndex - 1);
                            }
                        }
                        */
                        //int vertexIndex = int.Parse(parts[i]) - 1; // 정점 인덱스는 1부터 시작하므로 1을 빼줌
                    }
                }
            }
        }
    }

    void CreateGameObjectWithMesh(ObjectFile objfile)
    {
        // Mesh를 사용하여 GameObject 생성 또는 기존 GameObject에 Mesh 적용하는 작업을 수행하세요.

        //GameObject obj = new GameObject("LoadedObj");
        GameObject obj = Instantiate(new GameObject("LoadedObj"), objMeshBase.transform);


        //MeshFilter filter = gameObject.AddComponent<MeshFilter>();
        //filter.mesh = PopulateMesh(objfile);

        MeshFilter meshFilter = obj.AddComponent<MeshFilter>();
        meshFilter.mesh = PopulateMesh( objfile );

        // MeshRenderer를 추가하여 텍스처, 머티리얼 등을 설정할 수 있습니다.
        // 예를 들어:
        MeshRenderer meshRenderer = obj.AddComponent<MeshRenderer>();
        meshRenderer.material = new Material(Shader.Find("Standard"));

        // 추가적으로 필요한 설정을 수행하세요.

        /*
        GameObject meshChild = objMeshBase.transform.GetChild(0).GetChild(0).gameObject;
        meshChild.AddComponent<MeshCollider>();
        meshChild.GetComponent<MeshRenderer>().enabled = false;
        */
    }

    public static ObjectFile ReadObjectFile(string objContent)
    {
        ObjectFile obj = new ObjectFile();
        //string[] lines = File.ReadAllLines(path);

        obj.usemtl = new List<string>();
        obj.v = new List<Vector3>();
        obj.vn = new List<Vector3>();
        obj.vt = new List<Vector2>();
        obj.f = new List<List<int[]>>();


        using (StringReader reader = new StringReader(objContent))
        {
            string temp_line;
            while ((temp_line = reader.ReadLine()) != null)
            {
                //string[] parts = temp_line.Split(' ');

                if (temp_line == "" || temp_line.StartsWith("#"))
                    continue;

                string[] token = temp_line.Split(' ');
                switch (token[0])
                {

                    case ("o"):
                        obj.o = token[1];
                        break;
                    case ("mtllib"):
                        obj.mtllib = token[1];
                        break;
                    case ("usemtl"):
                        obj.usemtl.Add(token[1]);
                        obj.f.Add(new List<int[]>());
                        break;
                    case ("v"):
                        obj.v.Add(new Vector3(
                            float.Parse(token[1]),
                            float.Parse(token[2]),
                            float.Parse(token[3])));
                        break;
                    case ("vn"):
                        obj.vn.Add(new Vector3(
                            float.Parse(token[1]),
                            float.Parse(token[2]),
                            float.Parse(token[3])));
                        break;
                    case ("vt"):
                        obj.vt.Add(new Vector3(
                            float.Parse(token[1]),
                            float.Parse(token[2])));
                        break;
                    case ("f"):
                        for (int i = 1; i < 4; i += 1)
                        {
                            int[] triplet = Array.ConvertAll(token[i].Split('/'), x => {
                                if (String.IsNullOrEmpty(x))
                                    return 0;
                                return int.Parse(x);
                            });
                            //Debug.Log( "cnt : " + ( obj.f.Count - 1 ) );
                            if( obj.f.Count > 0 )
                            {
                                obj.f[obj.f.Count - 1].Add(triplet);
                            }
                        }
                        break;
                }

                //foreach (string line in parts)
                //{
                //}
            }
        }

        return obj;
    }

    public static MaterialFile ReadMaterialFile(string objContent)
    {
        MaterialFile mtl = new MaterialFile();
        //string[] lines = File.ReadAllLines(path);

        mtl.newmtl = new List<string>();
        mtl.mapKd = new List<string>();

        using (StringReader reader = new StringReader(objContent))
        {
            string temp_line;
            while ((temp_line = reader.ReadLine()) != null)
            {
                if (temp_line == "" || temp_line.StartsWith("#"))
                    continue;

                string[] token = temp_line.Split(' ');
                switch (token[0])
                {

                    case ("newmtl"):
                        mtl.newmtl.Add(token[1]);
                        break;
                    case ("map_Kd"):
                        mtl.mapKd.Add(token[1]);
                        break;
                }
            }
        }

        return mtl;
    }

    Mesh PopulateMesh(ObjectFile obj)
    {
        Mesh mesh = new Mesh();

        List<int[]> triplets = new List<int[]>();
        List<int> submeshes = new List<int>();

        for (int i = 0; i < obj.f.Count; i += 1)
        {
            for (int j = 0; j < obj.f[i].Count; j += 1)
            {
                triplets.Add(obj.f[i][j]);
            }
            submeshes.Add(obj.f[i].Count);
        }

        Vector3[] vertices = new Vector3[triplets.Count];
        Vector3[] normals = new Vector3[triplets.Count];
        Vector2[] uvs = new Vector2[triplets.Count];

        for (int i = 0; i < triplets.Count; i += 1)
        {
            vertices[i] = obj.v[triplets[i][0] - 1];
            normals[i] = obj.vn[triplets[i][2] - 1];
            if (triplets[i][1] > 0)
                uvs[i] = obj.vt[triplets[i][1] - 1];
        }

        mesh.name = obj.o;
        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uvs;
        mesh.subMeshCount = submeshes.Count;

        int vertex = 0;
        for (int i = 0; i < submeshes.Count; i += 1)
        {
            int[] triangles = new int[submeshes[i]];
            for (int j = 0; j < submeshes[i]; j += 1)
            {
                triangles[j] = vertex;
                vertex += 1;
            }
            mesh.SetTriangles(triangles, i);
        }

        mesh.RecalculateBounds();
        mesh.Optimize();

        return mesh;
    }

    Material[] DefineMaterial(ObjectFile obj, MaterialFile mtl)
    {

        Material[] materials = new Material[obj.usemtl.Count];

        for (int i = 0; i < obj.usemtl.Count; i += 1)
        {
            int index = mtl.newmtl.IndexOf(obj.usemtl[i]);

            //Texture2D texture = new Texture2D(1, 1);
            //texture.LoadImage(File.ReadAllBytes(directoryPath + mtl.mapKd[index]));

            materials[i] = new Material(Shader.Find("Diffuse"));
            materials[i].name = mtl.newmtl[index];
            //materials[i].mainTexture = texture;
        }

        return materials;
    }

    void Update()
    {
        if( Game_Mgr.Instance.gameSort == 0 )
        {
            if (Input.GetKey(KeyCode.Q))
            {
                obj_BasketBall_FBX.transform.localPosition = new Vector3(0, 0, 0);

                obj_BasketBall_FBX.GetComponent<Rigidbody>().useGravity = false;
                obj_BasketBall_FBX.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

                Game_Mgr.Instance.gamePlay = false;
            }
        }
    }

    public void Game_Button_Event(int nGameIndex)
    {
        Game_Mgr.Instance.gameSort = nGameIndex;
        Game_Mgr.Instance.gamePlay = false;

        Game_Hide();

        switch ( nGameIndex )
        {
            case 0:     Game_1_Init();      break;
            case 1:     Game_2_Init();      break;
            case 2:     Game_3_Init();      break;
        }
    }

    public void Game_3_Button_Event(int nIndex)
    {
        Game_Mgr.Instance.game3Sort = nIndex;

        Game_3_Init();
    }


    private void Game_Hide()
    {
        //Guide
        obj_Guide_Game1.SetActive( false );
        obj_Guide_Game2.SetActive( false );
        obj_Guide_Game3.SetActive( false );

        //Game 1
        obj_BasketBall_Goal.SetActive(false);
        obj_BasketBall.SetActive(false);

        //Game 2
        obj_CatBase.SetActive(false);

        //Game 3
        foreach (Transform child in obj_Gear_Clone_Base.transform)
        {
            Destroy( child.gameObject );
        }
        obj_Gear_Clone_Base.SetActive( false );

        obj_Gear_Button_Base.SetActive( false );

        obj_Gear_Chair.SetActive( false );
        obj_Gear_Sofa.SetActive( false );
        obj_Gear_Table.SetActive( false );
    }

    private void Game_1_Init()
    {
        obj_Guide_Game1.SetActive( true );

        Game_Mgr.Instance.vecMousePos.x = Input.GetAxis("Mouse X");
        Game_Mgr.Instance.vecMousePos.y = Input.GetAxis("Mouse Y");

        obj_BasketBall_Goal.SetActive( true );
        obj_BasketBall.SetActive( true );

        obj_BasketBall_Goal.transform.localPosition = new Vector3(0, -15, 200);
        obj_BasketBall_FBX.transform.localPosition = new Vector3( 0, 0, 0 );

        obj_BasketBall_FBX.GetComponent<Rigidbody>().useGravity = false;
        obj_BasketBall_FBX.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

    }

    private void Game_2_Init()
    {
        obj_Guide_Game2.SetActive( true );

        obj_Cats.transform.localPosition = new Vector3( 0, 0, 0 );
        obj_CatBase.transform.position = vec3_cats_pos;
        obj_CatBase.SetActive( true );
    }

    private void Game_3_Init()
    {
        obj_Guide_Game3.SetActive( true );

        obj_Gear_Chair.transform.localPosition = new Vector3(0, 0, 100);
        obj_Gear_Sofa.transform.localPosition = new Vector3(0, 0, 100);
        obj_Gear_Table.transform.localPosition = new Vector3(0, 0, 100);

        obj_Gear_Chair.transform.localRotation = new Quaternion(0, 0, 0, 0);
        obj_Gear_Sofa.transform.localRotation = new Quaternion(0, 0, 0, 0);
        obj_Gear_Table.transform.localRotation = new Quaternion(0, 0, 0, 0);

        obj_Gear_Clone_Base.SetActive( true );
        obj_Gear_Button_Base.SetActive( true );

        Game_3_ButtonInit();
    }

    private void Game_3_ButtonInit()
    {
        obj_Gear_Chair.SetActive( false );
        obj_Gear_Sofa.SetActive( false );
        obj_Gear_Table.SetActive( false );
        
        switch( Game_Mgr.Instance.game3Sort )
        {
            case 0:     obj_Gear_Chair.SetActive( true );       break;
            case 1:     obj_Gear_Sofa.SetActive( true );        break;
            case 2:     obj_Gear_Table.SetActive( true );       break;
        }
    }
}

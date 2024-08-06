using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Image_File_Find : MonoBehaviour
{
    public Dropdown drop;

    // Start is called before the first frame update
    void Start()
    {
        drop.ClearOptions();
        File_Find_Event();

        //Game_Mgr.Instance.imageName = "outsidetest.jpg";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void File_Find_Event()
    {
        //string strPath = Application.dataPath + "/Resources/python_data/";
        //string strPath = "Assets/Resources/python_data/";

        //StreamingAssets
        Debug.Log( Application.streamingAssetsPath );
        Debug.Log( Application.streamingAssetsPath + "/image" );
        Debug.Log( Application.dataPath );

        string str = Application.streamingAssetsPath + "/image";

        StartCoroutine(GetRemoteFolderContents(str));

        /*
        DirectoryInfo dir = new DirectoryInfo( Application.dataPath + "/StreamingAssets/image");
        Debug.Log( dir.FullName );

        FileInfo[] info = dir.GetFiles( "*.jpg" );
        //string imageFiles = Directory.GetFiles( str_path + "/image", "*.jpg" )[ 0 ].Split( '\\' )[ 1 ];
        //string[] imageFiles = Directory.GetFiles( Application.streamingAssetsPath + "/image", "*.jpg" );

        foreach (var t in info)
        {
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = t.Name;
            drop.options.Add(option);
        }
        */


        /*
        string path = Path.Combine(Application.streamingAssetsPath, "image/outsidetest.jpg");
        Debug.Log( path );

        //Debug.Log( Application.dataPath );

        byte[] byteTexture = File.ReadAllBytes( path );
        if( byteTexture.Length > 0 )
        {
            Texture2D texture = new Texture2D(0, 0);
            texture.LoadImage( byteTexture );


            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = texture.name;
            drop.options.Add(option);
        }
        */

        Texture2D[] arr = Resources.LoadAll<Texture2D>("python_data");
        foreach (var t in arr)
        {
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = t.name;
            drop.options.Add( option );
        }

        /*
        Debug.Log( strPath );

        DirectoryInfo di = new DirectoryInfo( strPath );

        //jpg 리스트
        foreach (FileInfo file in di.GetFiles("*.jpg"))
        {
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = file.Name.Replace(".jpg", "");
            drop.options.Add( option );
        }

        //png 리스트
        foreach (FileInfo file in di.GetFiles("*.png"))
        {
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = file.Name.Replace(".png", "");
            drop.options.Add(option);
        }
        */

        drop.value = 1;
        drop.value = 0;
    }

    IEnumerator GetRemoteFolderContents(string folderURL)
    {
        Debug.Log("url : " + folderURL);

        UnityWebRequest www = UnityWebRequest.Get(folderURL);
        yield return www.SendWebRequest();

        Debug.Log("neok 1");
        string fileNames = www.downloadHandler.text;

        Debug.Log("neok 2 : " + fileNames);
        string[] fileList = fileNames.Split('\n');

        Debug.Log("neok 3");
        foreach (string fileName in fileList)
        {
            if (fileName.EndsWith(".jpg"))
            {
                // 파일 이름을 출력하거나 다른 작업을 수행
                Debug.Log("File Name: " + fileName);
            }
        }
    }

    public void Drop_Down_Change()
    {
        Game_Mgr.Instance.imageName = drop.options[ drop.value ].text;
        //Game_Mgr.Instance.imageName = "outsidetest.jpg";
    }
}

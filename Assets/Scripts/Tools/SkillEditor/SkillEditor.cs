#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;

public class SkillEditor : MonoBehaviour
{
    public TMP_Dropdown Dropdown;

    public Transform ScrollItemRoot; 
    public GameObject ScrollItemPrefab;

    public TextMeshProUGUI TextMeshPro_Text;

    private string ScritableObjPath => Path.Combine(Application.dataPath, SubPath);
    private string SubPath = "Resources/ScriptableObj";
    private string AssetDatabasePath => Path.Combine("Assets", SubPath);

    public string CurrentAbsolutePath { get; private set; }

    void Start()
    {
        Prepare();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Prepare()
    {
        Dropdown.options =new List<TMP_Dropdown.OptionData> 
        { 
            new TMP_Dropdown.OptionData() { text = "Skill" }, //index 0
            new TMP_Dropdown.OptionData() { text = "State" } 
        };

        Dropdown.onValueChanged.AddListener(OnDropDownSelect);
        Dropdown.onValueChanged.Invoke(0);
    }

    public void OnDropDownSelect(int index)
    {

        foreach(Transform child in ScrollItemRoot)
        {
            GameObject.Destroy(child.gameObject);
        }


        var option = Dropdown.options[index];

        Debug.Log($"{option.text}  is selected");


        CurrentAbsolutePath = Path.Combine(ScritableObjPath, option.text);
        TextMeshPro_Text.SetText(Path.Combine("Assets", Path.GetRelativePath(Application.dataPath, CurrentAbsolutePath)));

        var fileList = Directory.GetFiles(CurrentAbsolutePath).ToList();
        List<string> assetList = new List<string>();
        foreach(var file in fileList)
        {
            if(file.EndsWith(".asset"))
                assetList.Add(file.ToString());
        }


        foreach(var asset in assetList)
        {
            var relativePath = Path.GetRelativePath(Application.dataPath, asset);
            var obj = AssetDatabase.LoadAssetAtPath<ScriptableObject>(Path.Combine("Assets",relativePath));

            var scrollItemGO = MonoBehaviour.Instantiate(ScrollItemPrefab);
            var scrollItem = scrollItemGO.GetComponent<SkillEditorScrollItem>();
            scrollItem.SetScriptableObj(obj);

            scrollItemGO.SetActive(true);
            scrollItemGO.transform.SetParent(ScrollItemRoot);
            scrollItemGO.transform.localPosition= Vector3.zero;
            scrollItemGO.transform.localScale= Vector3.one;
        }

    }
}


#endif

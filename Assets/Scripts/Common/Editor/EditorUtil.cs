using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class EditorUtil
{
    //# : shift
    //& : alt
    //% :  Winows의 Ctrl, macOS의 cmd키


    /// <summary>
    /// 선택한 콤포넌트의 주소를 복사한다.
    /// </summary>
    [MenuItem("Util/Copy Component Path %T")]
    private static void CopyComponentPath()
    {
        StringBuilder sb2 = new StringBuilder();

        HashSet<System.Type> findTypes = new HashSet<System.Type>(
            new System.Type[]{
                typeof(TextMesh),
                typeof(TMPro.TextMeshPro),
                typeof(Animation),
                typeof(Image),
                typeof(Text),
                typeof(Button)
            });

        List<Transform> selectItems = new List<Transform>();// ;
        for (int i = 0; i < Selection.gameObjects.Length; i++)
        {
            selectItems.Add(Selection.gameObjects[i].transform);
        }

        selectItems.Sort(
            delegate (Transform p1, Transform p2)
            {
                return (p1.name.CompareTo(p2.name)) * -1;
            });

        //부모도 선택되어 있으면 부모는 선택 목록에서 제외 한다.
        Transform parent = null;
        foreach (Transform t in selectItems)
        {
            Component[] m = t.GetComponents<Component>();
            bool containComponent = false;
            for (int i = 0; i < m.Length; i++)
            {
                Component c = m[i];
                if (c == null)
                    continue;

                System.Type itType = c.GetType();
                if (findTypes.Contains(itType))
                {
                    containComponent = true;
                    break;
                }
            }

            if (containComponent == false)
            {
                bool isParent = false;
                foreach (var item in selectItems)
                {
                    if (item == t)
                        continue;

                    var trs = item.GetComponentsInParent<Transform>(true);
                    foreach (var tr in trs)
                    {
                        if (tr == t)
                        {
                            parent = t;
                            isParent = true;
                            break;
                        }
                    }

                    if (isParent)
                        break;
                }
            }
        }

        selectItems.Remove(parent);

        StringBuilder sb1 = new StringBuilder();
        foreach (Transform t in selectItems)
        {
            string originalName = t.name;
            string componentPath = t.name;
            Transform tParent = t.parent;
            while (true)
            {
                if (tParent.parent == null || tParent == parent || tParent.parent == parent)
                    break;
                componentPath = string.Format("{0}/{1}", tParent.name, componentPath);
                tParent = tParent.parent;
            }

            Component[] m = t.GetComponents<Component>();

            // 콤포넌트 중에 버튼이 있으면 이미지는 복사하지 말자.
            bool isIncludeButton = m.Where(x => x.GetType() == typeof(Button)).Count() > 0;
            for (int i = 0; i < m.Length; i++)
            {
                System.Type itType = m[i].GetType();
                if (findTypes.Contains(itType))
                {
                    // 버튼있을때 다른 컴포넌트 복사는 하지 않게함.
                    if (isIncludeButton)
                        if (itType != typeof(Button))
                            continue;

                    string smallCharacterName = string.Format("{0}{1}", originalName[0].ToString().ToLower(), originalName.Substring(1));
                    var typeStr = itType.ToString().Split('.');
                    string typeString = typeStr[typeStr.Length - 1];
                    sb1.AppendFormat("{0} {1};\n", typeString, smallCharacterName);
                    sb2.AppendFormat("{2} = transform.Find(\"{0}\").GetComponent<{1}>();\n", componentPath, typeString, smallCharacterName);
                }
            }
        }

        sb1.AppendLine();

        clipboard = sb1.ToString() + sb2.ToString().Trim();
    }

    /// <summary>
    /// 선택한 모든 트랜스폼의 주소를 복사한다.
    /// </summary>
    [MenuItem("Util/Copy Path")]
    private static void CopyTransformsPath()
    {
        List<Transform> selectItems = new List<Transform>();
        selectItems.AddRange(Selection.transforms);
        selectItems.Sort(
            delegate (Transform p1, Transform p2)
            {
                return (p1.name.CompareTo(p2.name)) * -1;
            });

        StringBuilder sb = new StringBuilder();
        foreach (Transform t in selectItems)
        {
            string originalName = t.name;
            string componentPath = t.name;
            Transform tParent = t.parent;
            while (tParent != null)
            {
                componentPath = string.Format("{0}/{1}", tParent.name, componentPath);
                tParent = tParent.parent;
            }

            sb.AppendLine("\"" + componentPath + "\"");
        }

        clipboard = sb.ToString().Trim();
    }

    public static string clipboard
    {
        get
        {
            TextEditor te = new TextEditor();
            te.Paste();
            return te.text;
        }
        set
        {
            TextEditor te = new TextEditor();
            te.text = value;
            te.OnFocus();
            te.Copy();
        }
    }
}

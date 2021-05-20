using System.Collections.Generic;
using UnityEngine;

public enum OptionType
{
    StartIndex = -1,
    SuperPlayer,
    DisableEnemy,
    LastIndex
}

/// <summary>
/// 에디터에서 테스트할 때 사용.
/// if (EditorOption.Options[OptionType.SuperPlayer])
///     return;
/// </summary>
public class EditorOption
{
    static public Dictionary<OptionType, bool> Options
    {
        get
        {
            if (m_DevOption == null)
                InitDevOptionValue();
            return m_DevOption;
        }
    }

    static Dictionary<OptionType, bool> m_DevOption;
    static public void InitDevOptionValue()
    {
        if (m_DevOption == null)
            m_DevOption = new Dictionary<OptionType, bool>();

        for (OptionType i = OptionType.StartIndex + 1; i < OptionType.LastIndex; i++)
        {
#if UNITY_EDITOR
            string key = "DevOption_" + i;
            m_DevOption[i] = UnityEditor.EditorPrefs.GetInt(key, 0) == 0 ? false : true;
#else
        m_DevOption[i] = false;
#endif
        }
    }
}

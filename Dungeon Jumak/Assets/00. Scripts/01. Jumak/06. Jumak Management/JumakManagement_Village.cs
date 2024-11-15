//System
using System.Collections.Generic;

//Unity
using UnityEngine.UI;

//TMPro
using TMPro;
using UnityEngine;

public class JumakManagement_Village : UI_Base_JumakManagement
{
    private CSVReader csvReader;
    private List<YardData> yardData;

    private enum Buttons
    {
        Dim,

        //Category Buttons
        Village_Button,
        Hall_Button,
        Kitchen_Button,
        Employee_Button,
        Menu_Button,

        //Table 1
        T100, T101, T102, T103, T104, T105, T106, T107, T108, T109,

        //Table 2
        //T200, T201, T202, T203, T204, T205, T206, T207, T208, T209,

        //Table 3
        //T300, T301, T302, T303, T304, T305, T306, T307, T308, T309,

        //Table 4
        //T400, T401, T402, T403, T404, T405, T406, T407, T408, T409,

        //Table 5
        //T500, T501, T502, T503, T504, T505, T506, T507, T508, T509,

        //Table 6
        //T600, T601, T602, T603, T604, T605, T606, T607, T608, T609,

        //Table 7
        //T700, T701, T702, T703, T704, T705, T706, T707, T708, T709,

        //Table 8
        //T800, T801, T802, T803, T804, T805, T806, T807, T808, T809,

    }

    private enum TMPs
    {
        T100_Tmp, T101_Tmp, T102_Tmp, T103_Tmp, T104_Tmp, T105_Tmp, T106_Tmp, T107_Tmp, T108_Tmp, T109_Tmp,
    }

    private void Start()
    {
        //데이터 로트
        csvReader = new CSVReader();
        yardData = csvReader.ReadCSVFile<List<YardData>>("YardData");

        //버튼 Enum 바인딩
        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(TMPs));

        //딤 버튼 이벤트 셋팅
        GetButton((int)Buttons.Dim).gameObject.BindEvent(ClosePopUp);

        //카테고리 버튼 이벤트 바인딩
        GetButton((int)Buttons.Hall_Button).gameObject.BindEvent(ChangeCategoryToHall);
        GetButton((int)Buttons.Kitchen_Button).gameObject.BindEvent(ChangeCategoryToKitchen);
        GetButton((int)Buttons.Employee_Button).gameObject.BindEvent(ChangeCategoryToEmployee);
        GetButton((int)Buttons.Menu_Button).gameObject.BindEvent(ChangeCategoryToMenu);
    }

    private YardData FindYardData(string _id)
    {
        for (int i = 0; i < yardData.Count; i++)
        {
            if (yardData[i].ID == _id) return yardData[i];
        }

        return null;
    }

    private void ShowDetail()
    {
        UI_PopUp popup = GameManager.UI.ShowPopupUI<UI_PopUp>("");
        
    }
}

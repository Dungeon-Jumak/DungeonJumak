using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTest : MonoBehaviour
{
    public void OnClick()
    {
        GameManager.QuestManager.UpdateQuestProgress(1, 1);
        GameManager.QuestManager.UpdateQuestProgress(2, 10);
        GameManager.QuestManager.UpdateQuestProgress(3, 1);
        GameManager.QuestManager.UpdateQuestProgress(4, 1);
        GameManager.QuestManager.UpdateQuestProgress(5, 500);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class QuestIcon : MonoBehaviour
{
    public GameObject CanStart_Icon;
    public GameObject CanFinish_Icon;

    public void ChangeState(QuestProgress progress, bool startPoint, bool finishPoint)
    {
        CanStart_Icon.SetActive(false);
        CanFinish_Icon.SetActive(false);

        switch (progress)
        {
            case QuestProgress.CAN_START:
                CanStart_Icon.SetActive(true);
                break;
            case QuestProgress.CAN_FINISH:
                CanFinish_Icon.SetActive(true);
                break;
        }
    }
}

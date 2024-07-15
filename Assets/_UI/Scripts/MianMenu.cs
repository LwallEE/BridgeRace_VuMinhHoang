using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ManagerExample
{
    public class MianMenu : UICanvas
    {
        public void PlayButton()
        {
            UIManager.Ins.OpenUI<GamePlay>();
            Close(0);
        }
    }
}

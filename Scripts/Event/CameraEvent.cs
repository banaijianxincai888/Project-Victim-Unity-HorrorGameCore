using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns
{
    ///summary
    ///摄像机事件
    ///summary 

    public class CameraEvent
    {
        public Func<IInteractable[]> OnRayCheck;  // 用 event 包裹，更安全
    }
}


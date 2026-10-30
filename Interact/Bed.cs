using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns
{
    ///summary
    ///床脚本
    ///summary 
    public class Bed : MonoBehaviour, ISearchHideout
    {
        public HideoutInfo hideoutInfo;

        public HideoutInfo SearchHideoutPoint()
        {
            return hideoutInfo;
        }
    }
}


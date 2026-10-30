using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
///脚本单例类，提取多个管理类的相同概念，作为一个父类管理子类
/// <summary>
/// 

namespace ns
{
    //使用泛型类来管理,泛型的话，就要使用泛型约束,告诉程序这里的T约束在子类中
    public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        //T表示子类类型
        //public static T Instance {  get; private set; }
        //按需加载，在哪里调用就在哪里加载，不在Awake里

        private static T instance;

        public static T Instance
        {
            get
            {
                //如果子类是空的在找，不是空的不找
                if (instance == null)
                {
                    //这里加载一次就好
                    instance = FindAnyObjectByType<T>();
                    //如果在场景中忘记挂脚本了，那么就创建一个物体来挂载子类
                    if (instance == null)
                    {
                        instance = new GameObject("Singleton of " + typeof(T).Name).AddComponent<T>();
                    }
                    //保证一次初始化就好，与Awake区别
                    else
                    {
                        instance.Init();
                    }
                }
                return instance;

            }
        }


        private void Awake()
        {
            //与加载Instance区分，如果Awake里面访问，那么就会造成二次赋值！需要加判断
            if (instance == null)
            {
                instance = this as T;
                Init();
            }
        }


        //初始化方法，给子类初始化
        public virtual void Init() { }
    }
}
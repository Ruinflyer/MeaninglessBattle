using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Meaningless
{
    public delegate void TimerEnd();
    public delegate void TimerEverySec();

    public class Timer : MonoBehaviour
    {
        private static ArrayList TimerList;
        private static Timer _instance;


        public static Timer Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("Timer");
                    _instance = go.AddComponent<Timer>();
                    TimerList = new ArrayList();
                }
                return _instance;
            }

        }

        /// <summary>
        /// 倒计时
        /// </summary>
        /// <param name="seconds">倒计时秒数</param>
        /// <param name="EndFunction">倒计时为0时 执行的函数</param>
        public void StartCountdown(int seconds, TimerEnd EndFunction)
        {
            int TimerID = TimerList.Add(false);
            StartCoroutine(Countdown(TimerID, seconds, () => { }, EndFunction));
        }

        /// <summary>
        /// 每秒执行函数的倒计时
        /// </summary>
        /// <param name="seconds">倒计时秒数</param>
        /// <param name="EverySecFunction">每秒调用的函数</param>
        /// <param name="EndFunction">倒计时为0时 执行的函数</param>
        public void StartCountdown_EverysecRun(int seconds, TimerEverySec EverySecFunction, TimerEnd EndFunction)
        {
            int TimerID = TimerList.Add(false);
            StartCoroutine(Countdown(TimerID, seconds, EverySecFunction, EndFunction));
        }

        /// <summary>
        /// 关闭计时器
        /// </summary>
        public void StopTimer(int timerID)
        {
            TimerList[timerID] = true;
        }

        IEnumerator Countdown(int ID, int sec, TimerEverySec EverySecFunction, TimerEnd EndFunction)
        {
            if ((bool)TimerList[ID] == true)
            {
                yield return null;
            }

            TimerList[ID] = sec;

            while (sec > 0)
            {
                yield return new WaitForSeconds(1);
                sec--;
                TimerList[ID] = sec;
                EverySecFunction();
            }
            EndFunction();
            TimerList[ID] = true;
        }

    }
}
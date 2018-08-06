using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.code.controllers
{
    class wwwController : MonoBehaviour
    {
        public const float downloadTimeLimit = 15f;
        public WWW www;

        public IEnumerator wwwRequest(string url, float timeLimit = downloadTimeLimit)
        {
            Handheld.StartActivityIndicator();

            www = new WWW(url);
            if (GPSController.Instance.userLocationStable)
            {
                float downloadTime = 0;

                while (!www.isDone)
                {
                    downloadTime += Time.deltaTime;
                    if (downloadTime >= timeLimit)
                        break;
                    
                    yield return null;
                }
                Debug.Log("WWW transfer from "+url.Substring(8, 40)+".. @: " + (www.progress * 100) + "% in " + downloadTime + "s / " + timeLimit + "s");

                if (!www.isDone || !string.IsNullOrEmpty(www.error))
                {
                    Debug.LogError("Network error. Load not complete! TERMINATED AT:" + (int)www.progress*100 + "%");
                    yield break;
                }

                Handheld.StopActivityIndicator();
                yield return www;
            }

        } 
    }
}

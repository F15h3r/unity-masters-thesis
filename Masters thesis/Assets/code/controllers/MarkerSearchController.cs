using Assets.code.controllers;
using Assets.code.models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarkerSearchController : MonoBehaviour {
    public static MarkerSearchController Instance { get; set; }
    public InputField searchInput;
    public Button search, close;
    public GameObject popUp;
    public bool isDisplayed = false;
    public int searchRadius = 10000;
    public List<MarkerData> lastResults;
    private MarkerData md;


    public void Awake()
    {
        Instance = this;
        popUp.SetActive(isDisplayed);
        lastResults = new List<MarkerData>();
    }

    public void toggleSearch()
    {
        isDisplayed = !isDisplayed;
        popUp.SetActive(isDisplayed);
        MarkersListController.Instance.closeMarkersMenu();
    }

    public void searchStringGoogleMaps(string next_page_token = "")
    {
        
        string url = "https://maps.googleapis.com/maps/api/place/textsearch/json?query="
            + WWW.EscapeURL(searchInput.text)
            + "&location=" + GPSController.Instance.userWorldLocation.z + ","
            + GPSController.Instance.userWorldLocation.x
            + "&radius=" + searchRadius;

        url += (next_page_token == "") ? "" : next_page_token;
            
        url += "&key=" + Application.googleApiKey;

        //Debug.Log("URL: " + url);

        StartCoroutine(getSearchResults(url));
    }

    private IEnumerator getSearchResults(string url)
    {

        wwwController wCtrl = gameObject.AddComponent<wwwController>();
        yield return StartCoroutine(wCtrl.wwwRequest(url));
        yield return StartCoroutine(JSONDecodeGoogleSearchResponse(wCtrl.www.text));

        Destroy(wCtrl);
    }

    private IEnumerator JSONDecodeGoogleSearchResponse(string wwwResponse)
    {
        if(wwwResponse != null)
        {
            JSONObject j = new JSONObject(wwwResponse);

            if (j["status"].ToString().Contains("OK"))
            {
                lastResults = new List<MarkerData>();

                for(int i = 0; i < j["results"].Count; i++)
                {
                    md = new MarkerData();
                    
                    md.acquiredOnline = true;
                    md.dateAdded = Application.Utils.dateToday();
                    
                    md.worldCoords = new Vector3(float.Parse(j["results"][i]["geometry"]["location"]["lng"].ToString()),
                        0,
                        float.Parse(j["results"][i]["geometry"]["location"]["lat"].ToString()));

                    md.name = j["results"][i].GetField("name").str.ToString();
                
                    if (j["results"][i].HasField("formatted_address"))
                        md.description += "Address:\n" + j["results"][i].GetField("formatted_address").str + "\n\n";
                    if (j["results"][i].HasField("rating"))
                        md.description += "Google users rating: " + j["results"][i]["rating"].ToString() + " stars \n";

                    md.visible = false;
                    
                    lastResults.Add(md);
                }

            }
            else if(j["status"].ToString().Contains("ZERO_RESULTS"))
            {
                Debug.Log("NO RESULTS FOUND!");
                lastResults.Clear();
            }
            else Debug.LogError("CANT DECODE MESSAGE WITH CONTENT:\n" + wwwResponse);


            if(MarkersListController.Instance.isDisplayed)
                MarkersListController.Instance.toggleMarkersMenu(); // hide menu
            MarkersListController.Instance.toggleMarkersMenu(true, lastResults); // show menu with new data

            yield return lastResults;
        }

    }
}

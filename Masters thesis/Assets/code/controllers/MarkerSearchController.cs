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
    public int searchRadius = 8000;
    public List<MarkerData> lastResults;


    public void Awake()
    {
        Instance = this;
        lastResults = new List<MarkerData>();
        popUp.SetActive(isDisplayed);
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
        Handheld.StartActivityIndicator();

        WWW www = new WWW(url);
        while (!www.isDone)
            yield return null;

        if (string.IsNullOrEmpty(www.error))
        {
            //Debug.Log(www.text);

            if (!string.IsNullOrEmpty(www.text))
                JSONDecodeGoogleSearchResponse(www.text);
            else
                Debug.LogError("SearchController:www.text response empty!");
        }
        else
            Debug.LogError(www.error);

        Handheld.StopActivityIndicator();
    }

    private void JSONDecodeGoogleSearchResponse(string wwwResponse)
    {
        lastResults.Clear();

        JSONObject j = new JSONObject(wwwResponse);
        if (j["status"].ToString().Contains("OK"))
        {
            //Debug.Log("St rezultatov: " + j["results"].Count);
            for(int i = 0; i < j["results"].Count; i++)
            {
                MarkerData md = new MarkerData();
                md.acquiredOnline = true;
                md.dateAdded = Application.Utils.dateToday();
                md.worldCoords = new Vector3(float.Parse(j["results"][i]["geometry"]["location"]["lng"].ToString()),
                    0,
                    float.Parse(j["results"][i]["geometry"]["location"]["lat"].ToString()));

                md.name = j["results"][i].GetField("name").str;
                
                if(j["results"][i].HasField("formatted_address"))
                    md.description += "Address:\n" + j["results"][i].GetField("formatted_address").str + "\n\n";
                if (j["results"][i].HasField("rating"))
                    md.description += "Rating: " + j["results"][i]["rating"].ToString() + " stars \n";
                if (j["results"][i].HasField("opening_hours"))
                    md.description += "Open now: " + j["results"][i]["opening_hours"]["open_now"].ToString() + "\n\n";
                md.visible = false;

                lastResults.Add(md);
            }
        }
        else if(j["status"].ToString().Contains("ZERO_RESULTS"))
        {
            Debug.Log("No results found!");
        }
        else Debug.LogError("CANT DECODE MESSAGE WITH CONTENT:\n" + wwwResponse);

        //MarkersListController.Instance.refreshAllMarkersList(true, results);

        if(MarkersListController.Instance.isDisplayed)
            MarkersListController.Instance.toggleMarkersMenu(); // hide menu
        MarkersListController.Instance.toggleMarkersMenu(true, lastResults); // show menu with new data
    }
}

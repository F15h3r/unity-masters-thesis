using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarkerAddPopupController : MonoBehaviour {
    public bool isDisplayed = false;
    public GameObject popUp;
    public InputField nameText, lonText, latText, descriptionText;
    public Toggle googleElevationToggle;
    public static MarkerAddPopupController Instance { set; get; }

    void Start()
    {
        Instance = this;

        
        popUp.transform.FindChild("closeButton").GetComponent<Button>().onClick.AddListener(Instance.closeMarkerAddPopup);
        popUp.transform.FindChild("addButton").GetComponent<Button>().onClick.AddListener(Instance.addMarker);

        popUp.SetActive(isDisplayed);
    }

    public void toggleMarkerAddPopUp()
    {
        MarkerInfoPopUpController.Instance.closeMarkerInfoPopup();
        MarkersListController.Instance.closeMarkersMenu();

        isDisplayed = !isDisplayed;

        if(isDisplayed)
        {
            lonText.text = GPSController.Instance.userWorldLocation.x.ToString();
            latText.text = GPSController.Instance.userWorldLocation.z.ToString();

            nameText.Select();
            nameText.ActivateInputField();
        }

        popUp.SetActive(isDisplayed);

    }

    public void closeMarkerAddPopup()
    {
        if (isDisplayed)
            toggleMarkerAddPopUp();
    }

    public void addMarker()
    {
        //Debug.Log(nameText.text + " " + latText.text + " " + lonText.text + "\n" + descriptionText.text + " " + googleElevationToggle.isOn);
        Vector3 worldLocation = new Vector3(float.Parse(lonText.text), float.MinValue, float.Parse(latText.text));
        if (!googleElevationToggle.isOn)
            worldLocation.y = 0; // TODO: SPREMENI NA UPORABNIKOVO POLJE

        MarkerController.Instance.add3DMarkerInstance(worldLocation, nameText.text, descriptionText.text);
        Instance.closeMarkerAddPopup();
        GoogleMapsController.Instance.reloadMapImage();
    }

}

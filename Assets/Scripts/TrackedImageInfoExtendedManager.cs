using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEditor;
using UnityObject = UnityEngine.Object;

[RequireComponent(typeof(ARTrackedImageManager))]

 
// [CustomPropertyDrawer(typeof(MyPrefabsDictionary))]
// public class MyDictionaryDrawer1 : DictionaryDrawer<string, GameObject> { }

[Serializable] public class MyPrefabsDictionary : SerializableDictionary<string, GameObject> { }

public class TrackedImageInfoExtendedManager : MonoBehaviour
{

    [SerializeField]
    private Text imageTrackedText;
    
    private ARTrackedImageManager m_TrackedImageManager;

    public MyPrefabsDictionary myPrefabsDictionary;


    public GameObject JockerPrefab;
    public GameObject defaultPrefab;
    public GameObject EarthPrefab;

    private Dictionary<string, GameObject> objectPresent;

    void Awake()
    {
        // dismissButton.onClick.AddListener(Dismiss);
        m_TrackedImageManager = GetComponent<ARTrackedImageManager>();
        objectPresent = new Dictionary<string, GameObject>();
    }

    void OnEnable()
    {
        m_TrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        m_TrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    // private void Dismiss() => welcomePanel.SetActive(false);

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        // imageTrackedText.text = "Tracked Changed";
        Debug.Log("Tracked Changed");
        // Debug.Log(eventArgs.removed.Count);

        foreach (ARTrackedImage image in eventArgs.added)
        {
            XRReferenceImage refImage = image.referenceImage;
            image.destroyOnRemoval = true;
            string imgName = refImage.name; // this is the name in the library
            imageTrackedText.text = imgName;
            // GameObject prefab = myPrefabsDictionary[imgName];

            GameObject prefab = getPrefabByName(imgName);
            // prefab.GetComponent("Recognitiontion");
            prefab.GetComponentInChildren<TextMesh>().text = imgName;
        
            GameObject prefabRef = Instantiate(prefab, image.transform);

            objectPresent.Add(imgName, prefabRef);
        }

        foreach (ARTrackedImage image in eventArgs.updated)
        {


            if(image.trackingState.Equals(TrackingState.Limited))
            {
                Debug.Log("u- ---------------------------");
                Debug.Log("u- State event");
                Debug.Log("u- " + image.trackingState);
                Debug.Log("u- " + image.trackingState.Equals(TrackingState.Limited));
                Debug.Log("u- ---------------------------");
                Debug.Log("u- Delete limited event");
                Debug.Log("u- " + objectPresent.Count);
                imageTrackedText.text = "Delete something ";

                XRReferenceImage refImage = image.referenceImage;
                string imgName = refImage.name; // this is the name in the library
                imageTrackedText.text = "Delete : " + imgName;
                Debug.Log("u- Delete : " + imgName);

                GameObject destroyPrefab = objectPresent[imgName];
                // Debug.Log(destroyPrefab);
                destroyPrefab.SetActive(false);
                // Destroy(destroyPrefab);
                // Debug.Log("Well Destroyed");
            }


            if(image.trackingState.Equals(TrackingState.Tracking))
            {
                Debug.Log("Tracking detected");

                XRReferenceImage refImage = image.referenceImage;
                string imgName = refImage.name; // this is the name in the library

                GameObject destroyPrefab = objectPresent[imgName];

                if (!destroyPrefab.activeSelf)
                {
                    Debug.Log("u- Restore Prefab visibility");
                    imageTrackedText.text = "Restore Prefab visibility";
                    imageTrackedText.text = "Restore : " + imgName;
                    Debug.Log("u- Restore : " + imgName);
                    // Debug.Log(destroyPrefab);
                    destroyPrefab.SetActive(true);
                    // Destroy(destroyPrefab);
                    // Debug.Log("Well Destroyed");
                }
            }

        }
        foreach (ARTrackedImage image in eventArgs.removed)
        {
            Debug.Log("u- Delete event");
            imageTrackedText.text = "Delete something ";

            // XRReferenceImage refImage = image.referenceImage;
            // string imgName = refImage.name; // this is the name in the library
            // imageTrackedText.text = "Delete : " + imgName;
            // Debug.Log("Delete : " + imgName);


            // GameObject destroyPrefab = objectPresent[imgName];

            // Destroy(destroyPrefab);
        }
        // Debug.Log("Hello from OnTrackedImagesChanged Func");
        // foreach (ARTrackedImage trackedImage in eventArgs.added)
        // {
        //     Debug.Log("Hello from foreach (ARTrackedImage trackedImage Func");
        //     Debug.Log(trackedImage.referenceImage.name);
        //     // Debug.Log(trackedImage.gameObject)


        //     // Display the name of the tracked image in the canvas
        //     imageTrackedText.text = trackedImage.referenceImage.name;
        //     // Give the initial image a reasonable default scale
        //     // trackedImage.transform.localScale = 
        //     //     new Vector3(-trackedImage.referenceImage.size.x, 0.005f, -trackedImage.referenceImage.size.y);
        // }

        // foreach (ARTrackedImage trackedImage in eventArgs.updated)
        // {
        //     // Display the name of the tracked image in the canvas
        //     imageTrackedText.text = trackedImage.referenceImage.name;
        //     // Give the initial image a reasonable default scale
        //     trackedImage.transform.localScale = 
        //         new Vector3(-trackedImage.referenceImage.size.x, 0.005f, -trackedImage.referenceImage.size.y);
        // }
    }

    private GameObject getPrefabByName(string imgName)
    {
        Debug.Log("u- Hello from getPrefabByName Func");
        Debug.Log("u- Detected name : " + imgName);
        if (imgName == "Jocker"){
            return JockerPrefab;
        }
        else if (imgName == "earth"){
            return EarthPrefab;
        }
        else{
            return defaultPrefab;
        }
    }
}
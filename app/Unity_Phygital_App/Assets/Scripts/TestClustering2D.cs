using System.Linq;
using UnityEngine;

public class TestClustering2D : MonoBehaviour
{
    public int countMarkers = 50000;
    public Texture2D markerTexture;

    private void GenerateMarkers()
    {
        for (int i = 0; i < countMarkers; i++)
        {
            double longitude = Random.Range(-180f, 180f);
            double latitude = Random.Range(-90f, 90f);

            /*OnlineMapsMarker marker = new OnlineMapsMarker();
            marker.SetPosition(longitude, latitude);
            marker.label = "Marker " + i;
            marker.texture = markerTexture;*/
            OnlineMapsMarker marker = OnlineMapsMarkerManager.CreateItem(longitude, latitude);
            marker.OnClick += delegate (OnlineMapsMarkerBase m)
            {
                Debug.Log("Marker Click. " + m.label);
                if (Input.GetKeyDown(KeyCode.LeftControl)) Clustering2DMarkers.Remove(m as OnlineMapsMarker);
                else
                {
                    double mx, my;
                    marker.GetPosition(out mx, out my);

                    Clustering2DMarkers.MarkerWrapper wrapper = Clustering2DMarkers.rootCluster.FindMarkerWrapper(marker);
                    Clustering2DMarkers.Cluster p = wrapper.parent;

                    Debug.Log(p.zoom);
                }
            };
            marker.Init();

            marker.OnPositionChanged += delegate(OnlineMapsMarkerBase m)
            {
                Clustering2DMarkers.UpdateMarkerPosition(m as OnlineMapsMarker);
            };

            Clustering2DMarkers.Add(marker);
        }

        Clustering2DMarkers.UpdatePositions();
    }

    private void Start()
    {
        GenerateMarkers();
    }
}
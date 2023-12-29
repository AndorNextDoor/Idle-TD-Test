using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TowerPlacementController : MonoBehaviour
{
    public GameObject[] towerPrefab;
    public GameObject[] towerPreviewPrefabs;
    [SerializeField] private Material validPlacementMaterial;
    [SerializeField] private Material invalidPlacementMaterial;

    public LayerMask placementLayer;
    public int towerIndex = 0;
    private GameObject towerPreview;

    private bool isTowerSelected;



    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CameraController.Instance.SetCameraPos(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            CameraController.Instance.SetCameraPos(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            CameraController.Instance.SetCameraPos(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            CameraController.Instance.SetCameraPos(0);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            SetTowerIndex(0);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isTowerSelected = false;
            Destroy(towerPreview);
            towerPreview = null;
        }


        if (isTowerSelected)
        {
            TowerPreviewPosition();
        }
    }

    private void PlaceTower()
    {
        GameObject newTower = Instantiate(towerPrefab[towerIndex], towerPreview.transform.position, Quaternion.identity);
        DestroyTowerPreview();
        CameraSystem.instance.isDisabled = false;
        isTowerSelected = false;
    }

    public void SetTowerIndex(int _index)
    {
        if(towerPreview != null)
        {
            Destroy(towerPreview.gameObject);
        }

        towerIndex = _index;

        CameraSystem.instance.isDisabled = true;

        towerPreview = Instantiate(towerPreviewPrefabs[towerIndex]);
        Invoke("towerIsSelected", 1);
    }

    private void towerIsSelected()
    {
        isTowerSelected = true;
    }

    private void TowerPreviewPosition()
    {
        if (towerPreview == null) return;
        if (Input.touchCount < 1) return;

        Touch touch = Input.GetTouch(0);

        switch (touch.phase)
        {
            case TouchPhase.Began:
            case TouchPhase.Moved:
                UpdateTowerPreviewPosition(touch.position);
                break;

            case TouchPhase.Ended:
                PlaceTower();
                break;
        }
    }

    private void UpdateTowerPreviewPosition(Vector2 touchPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);

        if (Physics.Raycast(ray, out RaycastHit hitData, 100, placementLayer))
        {
            Vector3 towerPosition = hitData.point;

            // Adjust the Y coordinate to match the ground elevation
            float groundElevation = hitData.point.y;

            // Assuming towerPreview has a base at the Y=0 position
            towerPosition.y = groundElevation + towerPreviewPrefabs[towerIndex].transform.position.y;

            towerPreview.transform.position = towerPosition;
            towerPreview.GetComponent<TowerPlacementColor>().SetMaterial(validPlacementMaterial);
        }
        else
        {
            towerPreview.GetComponent<TowerPlacementColor>().SetMaterial(invalidPlacementMaterial);
        }
    }

    private void DestroyTowerPreview()
    {
        if (towerPreview != null)
        {
            Destroy(towerPreview);
        }
    }
}

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
    private Tile currentTile;

    private bool canPlaceTower = false;


    private void Update()
    {
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
#if UNITY_EDITOR
            TowerPreviewPositionEditor();
#else
            TowerPreviewPosition();
#endif
        }
    }

    private void PlaceTower()
    {
        if (canPlaceTower)
        {
            GameObject newTower = Instantiate(towerPrefab[towerIndex], towerPreview.transform.position, towerPrefab[towerIndex].transform.rotation);
            currentTile.OccupyTile();
            newTower.GetComponent<Tower>().SetLaneIndex(currentTile.laneIndex);
            DestroyTowerPreview();
            CameraSystem.instance.isDisabled = false;
            isTowerSelected = false;
        }
        else
        {
            DestroyTowerPreview();
            CameraSystem.instance.isDisabled = false;
            isTowerSelected = false;
        }

    }

    private void SetTowerIndex(int _index)
    {
        if (towerPreview != null)
        {
            Destroy(towerPreview.gameObject);
        }

        towerIndex = _index;

        CameraSystem.instance.isDisabled = true;

        // Create a new tower preview
        towerPreview = Instantiate(towerPreviewPrefabs[towerIndex]);

        // Invoke the towerIsSelected method after a delay
        Invoke("TowerIsSelected", 1);
    }

    private void TowerIsSelected()
    {
        isTowerSelected = true;
    }


#if UNITY_EDITOR

    private void TowerPreviewPositionEditor()
    {
        // Check for mouse input instead of touch
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
        {
            UpdateTowerPreviewPosition(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            PlaceTower();
        }
    }

#else

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

#endif

    private void UpdateTowerPreviewPosition(Vector2 touchPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);

        if (Physics.Raycast(ray, out RaycastHit hitData, 100, placementLayer))
        {
            Vector3 towerPosition = hitData.point;
            currentTile = hitData.collider.gameObject.GetComponent<Tile>();


            // Snap the tower to the center of the tile
            towerPosition = GetCenterOfTile(hitData.collider.gameObject);

            towerPosition.y = towerPreviewPrefabs[towerIndex].transform.position.y;

            towerPreview.transform.position = towerPosition;
            
            if (currentTile.isOccupied)
            {
                canPlaceTower = false;
                towerPreview.GetComponent<TowerPlacementColor>().SetMaterial(invalidPlacementMaterial);
            }
            else
            {
                canPlaceTower = true;
                towerPreview.GetComponent<TowerPlacementColor>().SetMaterial(validPlacementMaterial);
            }
        }
        else
        {
            canPlaceTower = false;
            towerPreview.GetComponent<TowerPlacementColor>().SetMaterial(invalidPlacementMaterial);
        }
    }

    private Vector3 GetCenterOfTile(GameObject tile)
    {
        // Calculate and return the center of the tile
        return tile.transform.position;
    }

    private void DestroyTowerPreview()
    {
        if (towerPreview != null)
        {
            Destroy(towerPreview);
        }
    }
}

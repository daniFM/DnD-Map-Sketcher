﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType { none, groundLow, groundMid, groundHigh, wall, eraser }
public enum TilePlacing { center, side, corner }

//[ExecuteInEditMode]
public class TileController : MonoBehaviour
{
    [Header("Configuration")]
    public float gridTextureTiling = 0.476f;
    public int gridToUnity = 1;
    public bool snapToCenter;
    public float sensitivityX = 1;
    public float sensitivityY = 1;
    public LayerMask layerMask;
    public GameObject brush;
    public GameObject tilePrefab;

    [Header("Painting")]
    public TileType brushType = TileType.groundHigh;
    [Range(1, 5)]
    public int brushSize;
    public TileGroup[] tileMeshes;

    private bool active = true;
    private new Renderer renderer;
    private LayerMask tileLayer;
    private Vector3 accumulator;
    private Vector3 halfTile = new Vector3(0.5f, 0, 0.5f);
    private Vector3 floorCorrection = new Vector3(0, 0.001f, 0);

    public static TileController instance = null;

    void Awake()
    {
        if(instance == null)
            instance = this;
        else if(instance != this)
            Destroy(gameObject);

        //DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        renderer = GetComponent<Renderer>();
        tileLayer = LayerMask.GetMask("Tile");
    }

    void OnEnable()
    {
        GameController.OnToolChanged += ToolChanged;
        ToolChanged();
    }

    void OnDisable()
    {
        GameController.OnToolChanged += ToolChanged;
    }

    private void OnValidate()
    {
        if(renderer != null)
            renderer.sharedMaterial.mainTextureScale = Vector2.one * gridTextureTiling / gridToUnity;

        brush.transform.localScale = new Vector3(brushSize + 0.4f, brushSize + 0.4f, 1);
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        float mouseWheel = Input.GetAxis("Mouse ScrollWheel");

        // Grid movement - MOVE TO OTHER SCRIPT
        if(Input.GetMouseButton(1))
        {
            accumulator += new Vector3(mouseX * sensitivityX, 0, mouseY * sensitivityY);

            if(accumulator.magnitude > 1)
            {
                transform.position -= (Quaternion.AngleAxis(45, Vector3.up) * accumulator).Round();
                accumulator = Vector3.zero;
            }
        }

        if(mouseWheel > 0)
        {
            transform.Translate(0, 1, 0);
        }
        else if(mouseWheel < 0)
        {
            transform.Translate(0, -1, 0);
        }

        if(active)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                //Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 60);
                //Debug.Log("Did Hit");

                // Determine final position
                Vector3 position = (hit.point + floorCorrection).Floor();
                if(!snapToCenter)
                {
                    if(brushSize % 2 == 0)
                        brush.transform.position = position + floorCorrection;
                    else
                        brush.transform.position = position + floorCorrection + halfTile;
                    position += halfTile;
                }
                else
                {
                    if(brushSize % 2 == 0)
                        brush.transform.position = position + floorCorrection + halfTile;
                    else
                        brush.transform.position = position + floorCorrection;
                    position += halfTile;
                }

                // Tile placing and erasing
                if(Input.GetMouseButtonDown(0) || (Input.GetMouseButton(0) && mouseX != 0))
                {
                    for(int i = 0; i < brushSize; ++i)
                    {
                        for(int j = 0; j < brushSize; ++j)
                        {
                            int offset = (int)Mathf.Ceil((float)brushSize / 2) - 1;
                            Vector3 tposition = position - new Vector3(i - offset, 0, j - offset);

                            Collider[] hitColliders = Physics.OverlapSphere(tposition, 0.1f, tileLayer);
                            //if(!Physics.CheckSphere(tposition, 0.1f, tileLayer))

                            // Paint/replace/erase logic
                            TileType otherType = TileType.none;

                            if(hitColliders.Length > 0)
                            {
                                otherType = hitColliders[0].GetComponent<Tile>().type;
                                if(brushType == TileType.eraser || brushType != otherType)
                                    Destroy(hitColliders[0].gameObject);
                            }

                            if((otherType == TileType.none || brushType != otherType) && brushType != TileType.eraser)
                            {
                                Tile tile = Instantiate(tilePrefab, tposition, Quaternion.identity/*, this.transform*/).GetComponent<Tile>();
                                tile.SetTile(brushType);
                            }
                        }
                    }
                }
            }
        }
    }

    public Mesh GetTileMesh(TileType type, TilePlacing placing)
    {
        foreach(TileGroup group in tileMeshes)
        {
            if(group.type == type)
            {
                switch(placing)
                {
                    case TilePlacing.side:
                        return group.sides[Random.Range(0, group.sides.Length)];
                    case TilePlacing.center:
                        return group.centers[Random.Range(0, group.centers.Length)];
                    case TilePlacing.corner:
                        return group.corners[Random.Range(0, group.corners.Length)];
                }
            }
        }
        return null;
    }

    public void SetBrushType(int brushType)
    {
        this.brushType = (TileType)brushType;
    }

    public void SetBrushSize(float brushSize)
    {
        this.brushSize = (int)brushSize;
        brush.transform.localScale = new Vector3(brushSize + 0.4f, brushSize + 0.4f, 1);
    }

    private void ToolChanged()
    {
        if(GameController.instance.tool == ToolType.brush)
        {
            active = true;
            brush.SetActive(true);
        }
        else
        {
            active = false;
            brush.SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;

public enum TileType
{
    none        = 0,
    groundLow   = 1,
    groundML    = 2,
    groundMH    = 3,
    groundHigh  = 4,
    stair1      = 5,
    column      = 9,
    eraser      = 10,
    door1       = 11,
    door2       = 12,
    statue1     = 13,
    statue2     = 14,
    statue3     = 15,
    statue4     = 16,
    chest1      = 17,
    chest2      = 18,
    orb         = 19,
    trap1on     = 20,
    trap1off    = 21
}
public enum TilePlacing { center, side, corner }

//[ExecuteInEditMode]
public class TileController : MonoBehaviour
{
    [Header("Configuration")]
    public float gridTextureTiling = 0.476f;
    public int gridToUnity = 1;
    public bool snapToCenter;
    //public float sensitivityX = 1;
    //public float sensitivityY = 1;
    public LayerMask layerMask;
    public GameObject brush;
    public GameObject tilePrefab;

    [Header("Painting")]
    public TileType brushType = TileType.groundHigh;
    [SerializeField][ReadOnly]
    private int brushSize = 1;
    [SerializeField][ReadOnly]
    private int brushHeight = 1;
    public TileGroup[] tileMeshes;

    private bool active = true;
    private new Renderer renderer;
    private int tileLayer;
    private int blockRaycastLayer;
    //private Vector3 accumulator;
    private Vector3 halfTile = new Vector3(0.5f, 0, 0.5f);
    private Vector3 floorCorrection = new Vector3(0, 0.1f, 0);
    private object[][] tileInitData;

    [Header("Floor")]
    public bool createFloor = true;
    [SerializeField] private int floorX = 10;
    [SerializeField] private int floorZ = 10;


    [Header("Data")]
    [SerializeField] private int ctrlzAmount;
    [SerializeField] [ReadOnly] private int snapshotIndex;
    [SerializeField] private TileData[] tileSnapshots;

    [SerializeField]
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
        blockRaycastLayer = LayerMask.NameToLayer("BlockRaycast");

        tileInitData = new object[System.Enum.GetValues(typeof(TileType)).Cast<int>().Max() + 1][]; // Max value of the enum
        for(int i = 0; i < tileInitData.Length; ++i)
        {
            tileInitData[i] = new object[1];
            tileInitData[i][0] = i;
        }

        tileSnapshots = new TileData[ctrlzAmount];
        for(int i = 0; i < ctrlzAmount; ++i)
        {
            tileSnapshots[i] = new TileData();
        }

        if(createFloor && PhotonNetwork.IsMasterClient)
            Invoke("CreateFloor", 0.1f);

        Invoke("TakeSnapshot", 0.5f);
    }

    void OnEnable()
    {
        GameController.OnToolChanged += ToolChanged;
    }

    void OnDisable()
    {
        GameController.OnToolChanged -= ToolChanged;
    }

    private void OnValidate()
    {
        if(renderer != null)
            renderer.sharedMaterial.mainTextureScale = Vector2.one * gridTextureTiling / gridToUnity;

        brush.transform.localScale = new Vector3(brushSize, brushHeight, brushSize) * 1.01f;
    }

    void Update()
    {
        if(active && GameManager.instance.isDM)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask) && hit.collider.gameObject.layer != blockRaycastLayer)
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
                if(GameController.instance.controls.GetKey(ControlAction.Paint) || 
                    (
                        GameController.instance.controls.GetKey(ControlAction.Paint) && 
                        (
                            Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0
                        )
                    )
                )
                {
                    for(int i = 0; i < brushSize; ++i)
                    {
                        for(int j = 0; j < brushSize; ++j)
                        {
                            for(int k = 0; k < brushHeight; k++)
                            {
                                int offset = (int)Mathf.Ceil((float)brushSize / 2) - 1;
                                Vector3 tposition = position - new Vector3(i - offset, -k, j - offset);

                                Collider[] hitColliders = Physics.OverlapSphere(tposition + new Vector3(0, 0.2f, 0), 0.16f, tileLayer);
                                //if(!Physics.CheckSphere(tposition, 0.1f, tileLayer))

                                //Vector3 p = tposition + new Vector3(0, 0.2f, 0);
                                //float a = 0.16f;
                                //Debug.DrawLine(p, p + Vector3.up * a, Color.red, 10, false);
                                //Debug.DrawLine(p, p + Vector3.down * a, Color.red, 10, false);
                                //Debug.DrawLine(p, p + Vector3.right * a, Color.red, 10, false);
                                //Debug.DrawLine(p, p + Vector3.left * a, Color.red, 10, false);

                                // Paint/replace/erase logic
                                TileType otherType = TileType.none;

                                if(hitColliders.Length > 0)
                                {
                                    otherType = hitColliders[0].GetComponent<Tile>().type;
                                    if(brushType == otherType && GameController.instance.controls.GetKey(ControlAction.Paint))
                                    {
                                        hitColliders[0].GetComponent<Tile>().RotateTile();
                                    }
                                    if(brushType == TileType.eraser || brushType != otherType)
                                    {
                                        //PhotonNetwork.Destroy(hitColliders[0].gameObject);
                                        hitColliders[0].GetComponent<Tile>().DestroyByAnybody();
                                    }
                                }

                                if((otherType == TileType.none || brushType != otherType) && brushType != TileType.eraser)
                                {
                                    //Tile tile = Instantiate(tilePrefab, tposition, Quaternion.identity/*, this.transform*/).GetComponent<Tile>();
                                    //tile.SetTile(brushType);
                                    PhotonNetwork.Instantiate(tilePrefab.name, tposition, Quaternion.identity/*, this.transform*/, 0, tileInitData[(int)brushType]);
                                    //tileBuffer.Add(brushType, transform);
                                }
                            }
                        }
                    }
                }
                // Save snapshot
                else if(GameController.instance.controls.GetKeyUp(ControlAction.Paint))
                {
                    TakeSnapshot();
                }
            }

            // CTRL+Z
            if(GameController.instance.controls.GetKey(ControlAction.Undo))
            {
                if(snapshotIndex > 1)
                {
                    snapshotIndex--;
                    LoadSnapshot(tileSnapshots[snapshotIndex - 1]);
                    tileSnapshots[snapshotIndex].Clear();
                }
            }
        }
    }

    // Called from Invoke
    public void CreateFloor()
    {
        for(int i = 0; i < floorX; ++i)
        {
            for(int j = 0; j < floorZ; ++j)
            {
                Vector3 pos = new Vector3(i - (int)(floorX / 2), -1, j - (int)(floorZ / 2));
                if(!snapToCenter)
                    pos += /*floorCorrection +*/ halfTile;
                //else
                //    pos += floorCorrection;
                PhotonNetwork.Instantiate(tilePrefab.name, pos, Quaternion.identity, 0, tileInitData[(int)TileType.groundHigh]);
            }
        }
    }

    public TileData GetLastSnapshot(bool takeSnapshot = false)
    {
        if(takeSnapshot)
            TakeSnapshot();

        return tileSnapshots[snapshotIndex-1];
    }

    private void TakeSnapshot()
    {
        if(snapshotIndex == tileSnapshots.Length)
        {
            for(int i = 0; i < snapshotIndex - 1; ++i)
            {
                tileSnapshots[i] = new TileData(tileSnapshots[i + 1]);
            }
            snapshotIndex--;
            tileSnapshots[snapshotIndex].Clear();
        }

        foreach(Tile t in FindObjectsOfType<Tile>())
        {
            tileSnapshots[snapshotIndex].Add(t.type, t.transform.position, t.transform.rotation);
        }

        snapshotIndex++;
    }

    public void LoadSnapshot(TileData snapshot, bool takeSnapshot = false)
    {
        foreach(Tile t in FindObjectsOfType<Tile>())
        {
            t.DestroyByAnybody();
        }

        for(int i = 0; i < snapshot.Count; ++i)
        {
            // TO DO: Implement conditions for optimization
            //if(tileSnapshots[snapshotIndex].GetPositionAt(i) != tileSnapshots[snapshotIndex - 1].GetPositionAt(i))
            PhotonNetwork.Instantiate(
                tilePrefab.name,
                snapshot.GetPositionAt(i),
                snapshot.GetRotationAt(i),
                0,
                tileInitData[(int)snapshot.GetTypeAt(i)]);
        }

        if(takeSnapshot)
            TakeSnapshot();
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
        brush.transform.localScale = new Vector3(brushSize, brushHeight, brushSize) * 1.01f;
    }

    public void SetBrushHeight(float brushHeight)
    {
        this.brushHeight = (int)brushHeight;
        brush.transform.localScale = new Vector3(brushSize, brushHeight, brushSize) * 1.01f;
    }

    private void ToolChanged()
    {
        if(GameController.instance.Tool == ToolType.brush)
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

    // TO DO
    private object[] GetTileInitData(TileType type, bool autorotate)
    {
        throw new System.NotImplementedException();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public static Pathfinding Instance {get; private set;}
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14; //hipotenusa
    [SerializeField] private Transform gridDebugObjectPrefab;

    [SerializeField] private LayerMask obstaclesLayerMask;
    private int width;
    private int height;
    private float cellSize;

    private GridSystem<PathNode> gridSystem;
    void Awake()
    {
        if(Instance !=null){
            Debug.LogError("There is another instance of Pathfinding");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        

    }

    public void Setup(int width, int height, float cellSize){
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridSystem = new GridSystem<PathNode>(width, height, cellSize,(GridSystem<PathNode>g, GridPosition gridPosition)=> new PathNode(gridPosition));
        
        //gridSystem.CreateDebugObjects(gridDebugObjectPrefab);

        //GetNode(1,0).SetIsWalkable(false);
        //GetNode(1,1).SetIsWalkable(false);

        for(int x=0; x < width; x++){
            for(int z = 0; z < height; z++){
                GridPosition gridPosition = new GridPosition(x,z);
                Vector3 worldPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
                float raycastOffsetDistance = 5f;
                if(Physics.Raycast(worldPosition + Vector3.down * raycastOffsetDistance, Vector3.up, raycastOffsetDistance * 2, obstaclesLayerMask)){
                    GetNode(x, z).SetIsWalkable(false);
                }
            }
        }

    }

    public List<GridPosition> FindPath(GridPosition startGridPosition, GridPosition endGridPosition, out int pathLenght){
        List<PathNode> openList = new List<PathNode>();
        List<PathNode> closeList = new List<PathNode>();

        PathNode startNode = gridSystem.GetGridObject(startGridPosition);
        PathNode endNode = gridSystem.GetGridObject(endGridPosition);
        openList.Add(startNode);

        for(int x=0; x < gridSystem.GetWitdh(); x++){
            for(int z = 0; z < gridSystem.GetHeight(); z++){
                GridPosition gridPosition = new GridPosition(x, z);
                PathNode pathNode = gridSystem.GetGridObject(gridPosition);
                pathNode.SetGCost(int.MaxValue);
                pathNode.SetHCost(0);
                pathNode.CalculateFCost();
                pathNode.ResetCameFromPathNode();
            }
        }

        startNode.SetGCost(0);
        startNode.SetHCost(CalculateDistance(startGridPosition, endGridPosition));
        startNode.CalculateFCost();

        while(openList.Count>0){
            PathNode currentNode = GetLowestFCostPathNode(openList);

            if(currentNode == endNode){
                //Reached final node
                pathLenght = endNode.GetFCost();
                return CalculatePath(endNode);
            }
            
            openList.Remove(currentNode);
            closeList.Add(currentNode);

            foreach(PathNode neigbourNode in GetNeighbourList(currentNode)){
                if(closeList.Contains(neigbourNode)){
                    continue;
                }
                if(!neigbourNode.IsWalkable()){
                    closeList.Add(neigbourNode);
                    continue;
                }
                int tentativeGCost = currentNode.GetGCost() + CalculateDistance(currentNode.GetGridPosition(), neigbourNode.GetGridPosition());
                if(tentativeGCost < neigbourNode.GetGCost()){
                    neigbourNode.SetCameFromPathNode(currentNode);
                    neigbourNode.SetGCost(tentativeGCost);
                    neigbourNode.SetHCost(CalculateDistance(neigbourNode.GetGridPosition(), endGridPosition));
                    neigbourNode.CalculateFCost();

                    if(!openList.Contains(neigbourNode)){
                        openList.Add(neigbourNode);
                    }
                }
            }

        }
        //No path found
        pathLenght = 0;
        return null;
    }


    public int CalculateDistance(GridPosition gridPositionA, GridPosition gridPositionB){
        GridPosition gridPositionDistance = gridPositionA - gridPositionB;
        //int totalDistance = Mathf.Abs(gridPositionDistance.x) + Mathf.Abs(gridPositionDistance.z);
        int xDistance = Mathf.Abs(gridPositionDistance.x);
        int zDistance = Mathf.Abs(gridPositionDistance.z);
        int remaining = Mathf.Abs(xDistance - zDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private PathNode GetLowestFCostPathNode(List<PathNode> pathNodeList){
        PathNode lowestFCostPathNode = pathNodeList[0];
        for(int i = 0; i < pathNodeList.Count; i++){
            if(pathNodeList[i].GetFCost()<lowestFCostPathNode.GetFCost()){
                lowestFCostPathNode = pathNodeList[i];
            }
        }
        return lowestFCostPathNode;
    }

    private PathNode GetNode(int x, int z){
        return gridSystem.GetGridObject(new GridPosition(x, z));
        
    }

    private List<PathNode> GetNeighbourList(PathNode currentNode){
        List<PathNode> neighbourList = new List<PathNode>();

        GridPosition gridPosition = currentNode.GetGridPosition();


        if(gridPosition.x - 1 >0){
             //left
            neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z + 0));
            if(gridPosition.z - 1 >= 0){
                //LeftDown
                neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z - 1));

            }
            if(gridPosition.z + 1< gridSystem.GetHeight()){
                //DIAGONAL
                //LeftUp
                neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z + 1));

            }
        }
        if(gridPosition.x + 1 < gridSystem.GetWitdh()){
            //right 
            neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z + 0));
            if(gridPosition.z - 1 >= 0){
                //RigthDown
                neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z - 1));
            }
            if(gridPosition.z + 1 < gridSystem.GetHeight()){

                //RightUp
                neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z + 1));
            }

        }
        if(gridPosition.z - 1 >= 0){
            //down
            neighbourList.Add(GetNode(gridPosition.x + 0, gridPosition.z - 1));
        }
        if(gridPosition.z + 1 < gridSystem.GetHeight()){
            //top
            neighbourList.Add(GetNode(gridPosition.x + 0, gridPosition.z + 1));

        }



        return neighbourList;
    }
    private List<GridPosition> CalculatePath(PathNode endNode){
        List<PathNode> pathNodeList = new List<PathNode>();
        pathNodeList.Add(endNode);
        PathNode currentNode = endNode;
        while(currentNode.GetCameFromPathNode() !=null){
            pathNodeList.Add(currentNode.GetCameFromPathNode());
            currentNode = currentNode.GetCameFromPathNode();
        }
        pathNodeList.Reverse();
        List<GridPosition> gridPositionList = new List<GridPosition>();
        foreach(PathNode pathNode in pathNodeList){
            gridPositionList.Add(pathNode.GetGridPosition());
        }
        return gridPositionList;
    }

    public bool IsWalkable(GridPosition gridPosition){
        return gridSystem.GetGridObject(gridPosition).IsWalkable();
    }
    
    public void SetIsWalkable(GridPosition gridPosition, bool isWalkable){
        gridSystem.GetGridObject(gridPosition).SetIsWalkable(isWalkable);
    }

    public bool HasPath(GridPosition startGridPosition, GridPosition endGridPosition){
        return FindPath(startGridPosition, endGridPosition, out int pathLenght) != null;
    }

    public int GetPathLength(GridPosition startGridPosition, GridPosition endGridPosition){
        FindPath(startGridPosition, endGridPosition, out int pathLenght);
        return pathLenght;
    }

}

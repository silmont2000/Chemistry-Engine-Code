using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateWoods : MonoBehaviour
{
    int sum = 0;
    public int row;
    public int col;
    public int dis = 15;
    public GameObject Prefab;

    // Start is called before the first frame update
    void OnEnable()
    {
        // GameObject instance = (GameObject)Instantiate(Resources.Load("Prefabs/wood"), transform.position, transform.rotation);
        for (int i = 0; i < row; i++)
        {
            CreateLine(col, transform.position + new Vector3(0, 0, -i * dis));
        }
    }

    void CreateLine(int a, Vector3 position)
    {
        for (int i = 0; i < a; i++)
        {
            GameObject instance = (GameObject)Instantiate(Prefab, position + new Vector3(-i * dis, 0, 0), transform.rotation);
            // instance.name = "wood" + sum.ToString();
            sum++;
        }

    }
    // Update is called once per frame
    void Update()
    {

    }
}

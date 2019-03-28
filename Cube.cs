using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Cube : MonoBehaviour {
    public Rigidbody rb;
    public bool isSleep = false;
    public int num;
    public int idCube;
    public Text scoreBoard;
    public GameManager gameManager;
    public LayerMask layerMask;
    bool isStart;
	// Use this for initialization
	public void Start()
    {
        transform.localEulerAngles = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
        rb.AddTorque(Vector3.left * Random.Range(2, 500) + Vector3.forward * Random.Range(2, 500));
        rb.AddForce(Vector3.left * Random.Range(2, 500) + Vector3.forward * Random.Range(2, 500));
        isSleep = false;
        isStart = false;
        Time.timeScale = 0;
    }
	
	// Update is called once per frame
	void Update () {
        Time.timeScale = 1;
        if (rb.velocity.magnitude == 0 & rb.angularVelocity.magnitude == 0 & !isSleep & isStart)
        {
            
            isSleep = true;
            RaycastHit hit;
            CheckWall(transform.position, transform.up,out hit,3);
            CheckWall(transform.position, transform.up *-1, out hit, 4);
            CheckWall(transform.position, transform.right, out hit, 5);
            CheckWall(transform.position, transform.right * -1, out hit, 2);
            CheckWall(transform.position, transform.forward, out hit, 6);
            CheckWall(transform.position, transform.forward * -1, out hit, 1);
            scoreBoard.text = num + "";
            gameManager.players[gameManager.idThisPlayer].AddScore(num);
            if(num == 6)
            {
                gameManager.players[gameManager.idThisPlayer].indexSixScore++;
            }
            if (gameManager.players[gameManager.idThisPlayer].indexSixScore == 2)
            {
                gameManager.players[gameManager.idThisPlayer].numIsSteps++;
            }
            gameManager.AllCubesSleep++;
            if(gameManager.AllCubesSleep >= 2)
            {
                gameManager.DeleteCubes();
            }
        }
        else if(rb.velocity.magnitude != 0 & rb.angularVelocity.magnitude != 0 & !isStart)
        {
            isSleep = false;
            isStart = true;
        }
        
    }

    void CheckWall(Vector3 goPosition, Vector3 direction, out RaycastHit hit, int indexCube)
    {
        if (Physics.Raycast(goPosition, direction, out hit, 20f, layerMask))
        {
            Debug.Log(hit.collider.tag);
            if (hit.collider.tag == "floor")
            {
                num = indexCube;
            }
        }
    }
}
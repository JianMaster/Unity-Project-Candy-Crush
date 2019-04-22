using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candy : MonoBehaviour {

    public GameObject[] candys;
    public GameController game;
    public int rowIndex = 0;
    public int columnIndex = 0;
    public float offsetX = -4.5f;
    public float offsetY = -3.2f;
    public int type = 0;

    public SpriteRenderer selected;

    // Use this for initialization
    void Awake () {
        selected = GetComponentInChildren<SpriteRenderer>();

        int random = Random.Range(0, candys.Length);
        type=random;
        GameObject o=Instantiate(candys[random],transform );
        o.transform.localPosition = Vector3.zero;
    }

    void Update()
    {

    }

    public void UpdatePosition()
    {
        transform.position = new Vector3(offsetX + columnIndex, offsetY + rowIndex, 0f);
    }

    private void OnMouseDown()
    {
        game.Select(this);
    }

    public void TweenToPosition()
    {
        iTween.MoveTo(gameObject, iTween.Hash("x", offsetX + columnIndex, "y", offsetY + rowIndex, "time", 0.3f));
    }
}

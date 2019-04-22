using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public int columnNum = 10;
    public int rowNum = 7;

    public Candy candy;

    private List<List<Candy>> candyArr=new List<List<Candy>> ();

    public float waitTime = 0.3f;

    void Start () {
		for(int i=0;i<rowNum;++i)
        {
            List<Candy> tmp=new List<Candy>();

            for (int j=0;j<columnNum;++j)
            {
                Candy o = AddCandy(i, j);
                tmp.Add(o);
            }

            candyArr.Add(tmp);
        }
        if (CheckMatchs())
            RemoveMatches();
	}


    private Candy AddCandy(int rowIndex,int columnIndex)
    {
        Candy o = Instantiate(candy, transform);
        o.rowIndex = rowIndex;
        o.columnIndex = columnIndex;
        o.UpdatePosition();
        o.game = this;
        return o;
    }



    private Candy crt;

    public void Select(Candy c)
    {
        if (crt == null)
        {
            crt = c;
            crt.selected.enabled = true;
        }
        else
        {
            if (Mathf.Abs(crt.rowIndex - c.rowIndex) + Mathf.Abs(crt.columnIndex - c.columnIndex) == 1)
            {
                crt.selected.enabled = false;
                Exchange(crt, c);
                StartCoroutine(RemoveDetect(crt, c));
                crt = null;
            }
        }
    }

    private void Exchange(Candy m ,Candy n)
    {
        candyArr[m.rowIndex][m.columnIndex] = n;
        candyArr[n.rowIndex][n.columnIndex] = m;
        int row = m.rowIndex;
        int column = m.columnIndex;
        m.rowIndex = n.rowIndex;
        m.columnIndex = n.columnIndex;
        n.rowIndex = row;
        n.columnIndex = column;

        m.TweenToPosition();
        n.TweenToPosition();
    }



    private void Remove(Candy c )
    {
        Destroy(c.gameObject);

        int column = c.columnIndex;
        for (int row=c.rowIndex+1;row <rowNum;++row )
        {
            Candy c2 = candyArr[row][column];
            --c2.rowIndex;
            c2.TweenToPosition();
            candyArr[row - 1][column] = c2;
        }

        //Add Candy
        Candy newCandy = AddCandy(rowNum - 1, column);
        candyArr[rowNum - 1][column] = newCandy;
        ++newCandy.rowIndex;
        newCandy.UpdatePosition();
        --newCandy.rowIndex;
        newCandy.TweenToPosition();
    }

    IEnumerator RemoveDetect(Candy crt ,Candy c)
    {
        yield return new WaitForSeconds(waitTime);
        if (CheckMatchs())
            RemoveMatches();
        else
            Exchange(crt, c);
    }



    private List <Candy > matches = new List<Candy>();
    private void AddMatches(Candy c)
    {
        if (matches.IndexOf(c) == -1)
            matches.Add(c);
    }
    private void RemoveMatches()
    {
        foreach (Candy i in matches)
            Remove(i);
        matches.Clear();
        if (CheckMatchs())
            RemoveMatches();
    }
    

    private bool CheckMatchs()
    {
        return CheckHorizontalMatchs()|| CheckVerticalMatchs();
    }
    private bool CheckHorizontalMatchs()
    {
        bool result = false;

        for (int row = 0; row < rowNum; ++row)
        {
            for (int column = 0; column < columnNum - 2; ++column)
            {
                if ((candyArr[row][column].type == candyArr[row][column + 1].type) && (candyArr[row][column + 1].type == candyArr[row][column + 2].type))
                {
                    result = true;
                    AddMatches(candyArr[row][column]);
                    AddMatches(candyArr[row][column + 1]);
                    AddMatches(candyArr[row][column + 2]);
                }
            }
        }

        return result;
    }
    private bool CheckVerticalMatchs()
    {
        bool result = false;

        for (int column = 0; column < columnNum; ++column)
        {
            for (int row = 0; row < rowNum - 2; ++row)
            {
                if ((candyArr[row][column].type == candyArr[row + 1][column].type) && (candyArr[row + 1][column].type == candyArr[row + 2][column].type))
                {
                    result = true;
                    AddMatches(candyArr[row][column]);
                    AddMatches(candyArr[row + 1][column]);
                    AddMatches(candyArr[row + 2][column]);
                }
            }
        }

        return result;
    }

}

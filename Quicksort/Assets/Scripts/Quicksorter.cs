﻿
using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class Quicksorter : MonoBehaviour
{
    public float[] NumbersToSort;
    public float MinNumber = 0.1f;
    public float MaxNumber = 0.8f;
    private int posInList = 1;
    private float xPosToSpawn = 0.0f;
    public List<SortItemInfo> ItemsToSort = new List<SortItemInfo>();

    // Start is called before the first frame update
    void Start()
    {
        xPosToSpawn = ((((2f * Camera.main.orthographicSize) * Camera.main.aspect) / 2) * -1) + 1f;
        var numberToGenerate = (int)((((2f * Camera.main.orthographicSize) * Camera.main.aspect) - 2f) / 0.4f);
        NumbersToSort = Enumerable
                        .Repeat(0, numberToGenerate)
                        .Select(i => UnityEngine.Random.Range(MinNumber, MaxNumber))
                        .ToArray();

        Debug.Log(NumbersToSort.Count());
        foreach (var number in NumbersToSort)
            GenerateSquare(number);
    }

    void GenerateSquare(float height)
    {
        var widthOfObject = 0.3f;

        GameObject newSquare = new GameObject();
        newSquare.name = height.ToString();
        newSquare.transform.Translate(xPosToSpawn, -4.0f, newSquare.transform.position.z);
        SpriteRenderer sr = newSquare.AddComponent<SpriteRenderer>();
        Texture2D texture = new Texture2D(1025, 1025);
        texture.name = height.ToString();
        SortItemInfo info = newSquare.AddComponent<SortItemInfo>();
        info.Height = height;
        info.PositionInList = posInList;
        var colorToUse = new Color(UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f), height * 100);

        List<Color> cols = new List<Color>();
        for (int i = 0; i < (texture.width * texture.height); i++)
            cols.Add(colorToUse);
        texture.SetPixels(cols.ToArray());
        texture.Apply();

        sr.color = colorToUse;
        sr.sprite = Sprite.Create(texture, new Rect(0, 0, widthOfObject, height * 10), Vector2.zero, 1);

        xPosToSpawn += widthOfObject + 0.1f;
        ItemsToSort.Add(info);
        posInList++;

    }


    //public float Partition(List<SortItemInfo> listToPartition, float left, float right)
    //{
    //    float pivot = listToPartition.Where(xx => xx.Height == left).First().Height;
    //    while(true)
    //    {
    //        while(listToPartition.Where(xx => xx.Height == left).First().Height < pivot)
    //        {
    //            left++;
    //        }

    //        while(listToPartition.Where(xx => xx.Height == right).First().Height > pivot)
    //        {
    //            right++;
    //        }

    //        if(left < right)
    //        {
    //            if (listToPartition.Where(xx => xx.Height == left).First() == listToPartition.Where(xx => xx.Height == right).First()) return right;

    //            Swap(GameObject.Find(listToPartition.Where(xx => xx.Height == left).First().Height.ToString()), GameObject.Find(listToPartition.Where(xx => xx.Height == right).First().Height.ToString()));
    //        }
    //        else
    //        {
    //            return listToPartition.Where(xx => xx.Height == right).First().Height;
    //        }
    //    }
    //}

    public IEnumerator QuickSort(float[] listToSort)
    {
        if (listToSort.Length > 1)
        {
            float pivot = listToSort[UnityEngine.Random.Range(0,listToSort.Length-1)];
            Debug.Log(pivot);
            int left = 0;
            int right = listToSort.Length - 1;
            while (left <= right)
            {
                while (listToSort[left] < pivot)
                        left++;
                while (listToSort[right] > pivot)
                    right--;
                if (left <= right)
                {
                    Swap(GameObject.Find(listToSort[left].ToString()), GameObject.Find(listToSort[right].ToString()));
                    var temp = listToSort[left];
                    listToSort[left] = listToSort[right];
                    listToSort[right] = temp;
                    left++;
                    right--;
                    yield return new WaitForSecondsRealtime(.2f);
                }
            }
            List<float> startToPivot = new List<float>();
            List<float> pivotToEnd = new List<float>();

            for (int i = 0; i <= right; i++)
                startToPivot.Add(listToSort[i]);
            StartCoroutine(QuickSort(startToPivot.ToArray()));

            for (int i = left; i <= listToSort.Length -1; i++)
                pivotToEnd.Add(listToSort[i]);
            StartCoroutine(QuickSort(pivotToEnd.ToArray()));
                    
        }
    }

    public void Swap(GameObject currentPosition, GameObject NewPosition)
    {

        var currentPos = new Vector3(currentPosition.transform.position.x, currentPosition.transform.position.y);
        var newPos = new Vector3(NewPosition.transform.position.x, NewPosition.transform.position.y);

        currentPosition.transform.position = newPos;
        NewPosition.transform.position = currentPos;
        
        Debug.Log(currentPosition.name + "swapped with" + NewPosition.name);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            StartCoroutine(QuickSort(ItemsToSort.Select(xx => xx.Height).ToArray()));
    }
}

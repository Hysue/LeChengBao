using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinaryTree : MonoBehaviour
{
    private List<int> recordLayers = new List<int>();
    private Point p13;
    private Point p12;
    private Point p14;
    private Point p10;
    private Point p15;
    private Point p7;
    private Point p11;
    private Point p23;
    private Point p2;
    void Start()
    {
        p13 = new Point(13);
        p12 = new Point(12, p13);
        p14 = new Point(14);
        p10 = new Point(10);
        p15 = new Point(15);
        p7 = new Point(7, null, p12);
        p11 = new Point(11, p10, p15);
        p23 = new Point(23, p7, p14);
        p2 = new Point(2, p11, p23);
        PreorderTraversal(p2);
    }

    void PreorderTraversal(Point point, int layer = 1)
    {
        if (recordLayers.Contains(layer) == false)
        {
            recordLayers.Add(layer);
            Debug.Log(point.num);
        }
        layer++;
        if (point.leftChild != null) PreorderTraversal(point.leftChild, layer);
        if (point.rightChild != null) PreorderTraversal(point.rightChild, layer);
    }
}

class Point
{
    public int num;
    public Point leftChild;
    public Point rightChild;
    public Point(int num, Point leftChild = null, Point rightChild = null)
    {
        this.num = num;
        this.leftChild = leftChild;
        this.rightChild = rightChild;
    }
}
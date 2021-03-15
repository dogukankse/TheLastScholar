using UnityEngine;

public class Book : MonoBehaviour
{
    public int Weight => _weight;

    private int _weight;

    public void Start()
    {
        _weight = Random.Range(1, 16);
    }
}
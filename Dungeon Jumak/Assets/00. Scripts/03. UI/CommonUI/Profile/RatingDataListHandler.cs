using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatingDataListHandler : MonoBehaviour
{
    public static RatingDataListHandler Instance { get; private set; }

    [SerializeField] private RatingDataList ratingDataList;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public RatingDataList GetRatingDataList()
    {
        return ratingDataList;
    }
}


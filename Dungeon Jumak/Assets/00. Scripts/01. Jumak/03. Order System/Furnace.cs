using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class Furnace : MonoBehaviour
{
    [Header("터치시 감소되는 값")]
    [SerializeField] private float decreaseValue = 0.5f;

    [Header("주막 Horizontal Scroll Snap")]
    public HorizontalScrollSnap snap;

    private bool isCooking;
    private bool canCooking;
    private float cookingTime;

    private float startCookingTime;

    private Slider timerSlider;
    private CompletedFood completedFood;

    //탭 관련 변수
    private const float maxTimeBetweenTpas = 0.3f;
    private int requiredTapCount = 3;

    private int tapCount = 0;
    private float lastTapTime = 0f;

    //일일 터치 가능 관련 변수
    private int currentDecreaseCount = 0;
    private const int maxDecreaseCount = 80;

    //현재 조리하고 있는 음식을 주문한 자리 번호
    private int seatNumber;

    private MenuData menuData;

    private void Start()
    {
        canCooking = true;

        timerSlider = GetComponentInChildren<Slider>();
        timerSlider.gameObject.SetActive(false);

        completedFood = GetComponentInChildren<CompletedFood>();
        completedFood.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isCooking)
        {
            if (startCookingTime + cookingTime <= Time.time) CompleteCook(seatNumber);

            timerSlider.value += Time.deltaTime / cookingTime;

            //주방에 있을 때만 탭 가능
            if (snap.CurrentPage == 0) MultiTapDetector();
        }
    }

    public bool CanCooking()
    {
        return canCooking;
    }

    public void SetCanFood(bool _bool)
    {
        canCooking = _bool;
    }

    public void StartCook(MenuData _menuData, int _seatNumber)
    {
        isCooking = true;
        canCooking = false;

        menuData = _menuData;

        seatNumber = _seatNumber;

        //Setting Timer Slider
        timerSlider.gameObject.SetActive(true);
        timerSlider.value = 0;

        //Setting Timer
        startCookingTime = Time.time;
        cookingTime = menuData.cookingTimes[menuData.level - 1];
    }

    private void CompleteCook(int _seatNumber)
    {
        isCooking = false;

        timerSlider.gameObject.SetActive(false);

        completedFood.gameObject.SetActive(true);
        //completedFood.image.sprite = menuData.sprite;
        completedFood.Init(menuData, seatNumber);

        Debug.Log("요리 완료");
    }

    private void MultiTapDetector()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            //터치 종료시 체크
            if (touch.phase == TouchPhase.Ended)
            {
                if (Time.time - lastTapTime <= maxTimeBetweenTpas) tapCount++;
                else tapCount = 1;

                lastTapTime = Time.time;

                if (tapCount >= requiredTapCount)
                {
                    DecreaseCookingTime();
                    tapCount = 0;
                }
            }
        }
    }   

    private void DecreaseCookingTime()
    {
        if (maxDecreaseCount < currentDecreaseCount) return;

        currentDecreaseCount++;

        Debug.Log("시간 감소!");
        startCookingTime -= decreaseValue;
        timerSlider.value += decreaseValue / cookingTime;
    }
}

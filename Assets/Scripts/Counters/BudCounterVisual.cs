using System.Collections.Generic;
using UnityEngine;

public class BudCounterVisual : MonoBehaviour
{
    [SerializeField] private BudCounter budCounter;
    [SerializeField] private Transform berryPoint1;
    [SerializeField] private Transform berryPoint2;
    [SerializeField] private Transform berryVisualPrefab;

    private List<GameObject> berryVisualGameObjectList;
    private List<Transform> berryPoints;

    private void Awake()
    {
        berryVisualGameObjectList = new List<GameObject>();
        berryPoints = new List<Transform> { berryPoint1, berryPoint2 };
    }

    private void Start()
    {
        budCounter.OnBerrySpawned += BudCounter_OnBerrySpawned;
        budCounter.OnBerryRemoved += BudCounter_OnBerryRemoved;
    }

    private void BudCounter_OnBerryRemoved(object sender, System.EventArgs e)
    {
        // Проверяем, есть ли ягоды для удаления
        if (berryVisualGameObjectList.Count > 0)
        {
            GameObject berryGameObject = berryVisualGameObjectList[berryVisualGameObjectList.Count - 1];
            berryVisualGameObjectList.Remove(berryGameObject);
            Destroy(berryGameObject);
        }
    }

    private void BudCounter_OnBerrySpawned(object sender, System.EventArgs e)
    {
        Transform freePoint = GetFirstFreeBerryPoint();

        if (freePoint != null)
        {
            Transform berryVisualTransform = Instantiate(berryVisualPrefab, freePoint);
            berryVisualGameObjectList.Add(berryVisualTransform.gameObject);
        }
        /*else
        {
            Debug.LogWarning("No free berry points available!");
        }*/
    }

    private Transform GetFirstFreeBerryPoint()
    {
        // Проходим по всем точкам и находим первую пустую
        foreach (Transform point in berryPoints)
        {
            bool pointIsOccupied = false;

            // Проверяем, есть ли ягода в этой точке
            foreach (GameObject berry in berryVisualGameObjectList)
            {
                if (berry.transform.parent == point)
                {
                    pointIsOccupied = true;
                    break;
                }
            }

            if (!pointIsOccupied)
            {
                return point;
            }
        }

        return null; // Все точки заняты
    }
}
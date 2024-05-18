using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//public class TowerGenerator : MonoBehaviour
//{
//    public static TowerGenerator Instance;  // Singleton instance

//    public GameObject safePlatformPrefab;
//    public GameObject dangerPlatformPrefab;
//    public int levels = 50;
//    public float radius = 2.0f;
//    public float verticalSpacing = 2.0f;
//    private Dictionary<int, List<GameObject>> platformsByLevel = new Dictionary<int, List<GameObject>>();

//    void Awake()
//    {
//        if (Instance != null && Instance != this)
//        {
//            Destroy(this.gameObject);
//        }
//        else
//        {
//            Instance = this;
//        }
//    }

//    void Start()
//    {
//        for (int i = 0; i < levels; i++)
//        {
//            float height = i * verticalSpacing;
//            CreateLevel(height, i);
//        }
//    }

//    /*void CreateLevel(float height, int levelIndex)
//    {
//        List<GameObject> levelPlatforms = new List<GameObject>();
//        int platformsPerLevel = 6;
//        float angleStep = 360.0f / platformsPerLevel;

//        for (int j = 0; j < platformsPerLevel; j++)
//        {
//            float angle = j * angleStep;
//            GameObject platform = SpawnPlatform(angle, height, Random.Range(0, 2) == 0);
//            levelPlatforms.Add(platform);
//        }
//        platformsByLevel.Add(levelIndex, levelPlatforms);
//    }*/
//    void CreateLevel(float height, int levelIndex)
//    {
//        List<GameObject> levelPlatforms = new List<GameObject>();
//        int platformsPerLevel = 6;
//        float angleStep = 360.0f / platformsPerLevel;
//        bool hasSafePlatform = false;  // Flag to ensure there is at least one safe platform

//        for (int j = 0; j < platformsPerLevel; j++)
//        {
//            bool isDanger = Random.Range(0, 2) == 0;  // Randomly decide if the platform is dangerous

//            // Ensure the last platform is safe if none have been safe so far
//            if (j == platformsPerLevel - 1 && !hasSafePlatform)
//            {
//                isDanger = false;  // Force the last platform to be safe
//            }

//            GameObject platform = SpawnPlatform(angleStep * j, height, isDanger);
//            levelPlatforms.Add(platform);

//            if (!isDanger)
//            {
//                hasSafePlatform = true;  // Confirm at least one safe platform
//            }
//        }

//        platformsByLevel.Add(levelIndex, levelPlatforms);
//    }



//    /*GameObject SpawnPlatform(float angle, float height, bool isDanger)
//    {
//        float angleRad = angle * Mathf.Deg2Rad;
//        GameObject prefab = isDanger ? dangerPlatformPrefab : safePlatformPrefab;
//        Vector3 position = new Vector3(Mathf.Cos(angleRad) * radius, height, Mathf.Sin(angleRad) * radius);
//        Quaternion rotation = Quaternion.Euler(0, -angle, 0);
//        return Instantiate(prefab, position, rotation);
//    }*/

//    GameObject SpawnPlatform(float angle, float height, bool isDanger)
//    {
//        float angleRad = angle * Mathf.Deg2Rad;
//        GameObject prefab = isDanger ? dangerPlatformPrefab : safePlatformPrefab;

//        // Increase the radius to expand the distance between platform pieces
//        float expandedRadius = radius + 1.0f; // Adjust this value to increase the spacing

//        Vector3 position = new Vector3(Mathf.Cos(angleRad) * expandedRadius, height, Mathf.Sin(angleRad) * expandedRadius);
//        Quaternion rotation = Quaternion.Euler(0, -angle, 0);
//        return Instantiate(prefab, position, rotation);
//    }


//    public void DestroyLevel(int levelIndex)
//    {
//        if (platformsByLevel.ContainsKey(levelIndex))
//        {
//            foreach (GameObject platform in platformsByLevel[levelIndex])
//            {

//                Destroy(platform);
//            }
//            platformsByLevel.Remove(levelIndex);
//        }
//    }

//}
public class TowerGenerator : MonoBehaviour
{
    public static TowerGenerator Instance;  // Singleton instance

    public GameObject safePlatformPrefab;
    public GameObject dangerPlatformPrefab;
    public GameObject playerPrefab;
    public int levels = 50;
    public float radius = 2.0f;
    public float verticalSpacing = 2.0f;
    private Dictionary<int, List<GameObject>> platformsByLevel = new Dictionary<int, List<GameObject>>();
    private List<GameObject> platformPool = new List<GameObject>();
    [SerializeField] private CamerFollow camerafollow;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        for (int i = 0; i < levels; i++)
        {
            float height = i * verticalSpacing;
            CreateLevel(height, i);
        }
        SpawnPlayer();

    }

    void CreateLevel(float height, int levelIndex)
    {
        List<GameObject> levelPlatforms = new List<GameObject>();
        int platformsPerLevel = 6;
        float angleStep = 360.0f / platformsPerLevel;
        bool hasSafePlatform = false;

        for (int j = 0; j < platformsPerLevel; j++)
        {
            bool isDanger = Random.Range(0, 2) == 0;
            if (j == platformsPerLevel - 1 && !hasSafePlatform)
            {
                isDanger = false;
            }

            GameObject platform = SpawnPlatform(angleStep * j, height, isDanger);
            levelPlatforms.Add(platform);

            if (!isDanger)
            {
                hasSafePlatform = true;
            }
        }

        platformsByLevel.Add(levelIndex, levelPlatforms);
    }

    GameObject SpawnPlatform(float angle, float height, bool isDanger)
    {
        GameObject platform = GetPlatformFromPool(isDanger);
        float angleRad = angle * Mathf.Deg2Rad;
        float expandedRadius = radius + 1.0f;

        platform.transform.position = new Vector3(Mathf.Cos(angleRad) * expandedRadius, height, Mathf.Sin(angleRad) * expandedRadius);
        platform.transform.rotation = Quaternion.Euler(0, -angle, 0);
        platform.SetActive(true);
        return platform;
    }

    GameObject GetPlatformFromPool(bool isDanger)
    {
        foreach (GameObject obj in platformPool)
        {
            if (!obj.activeInHierarchy && obj.tag == (isDanger ? "Danger" : "Safe"))
            {
                return obj;
            }
        }

        GameObject prefab = isDanger ? dangerPlatformPrefab : safePlatformPrefab;
        GameObject newPlatform = Instantiate(prefab);
        newPlatform.tag = isDanger ? "Danger" : "Safe";
        platformPool.Add(newPlatform);
        return newPlatform;
    }

    public void DestroyLevel(int levelIndex)
    {
        if (platformsByLevel.ContainsKey(levelIndex))
        {
            foreach (GameObject platform in platformsByLevel[levelIndex])
            {
                platform.SetActive(false);  // Deactivate platforms instead of destroying
            }
            platformsByLevel.Remove(levelIndex);
        }
    }
    void SpawnPlayer()
    {
        if (platformsByLevel.Count > 0)
        {
            // Assuming the highest level has the highest index
            int highestLevelIndex = platformsByLevel.Keys.Max();
            List<GameObject> highestLevelPlatforms = platformsByLevel[highestLevelIndex];
            GameObject firstPlatform = highestLevelPlatforms[0];
            Vector3 spawnPosition = firstPlatform.transform.position + new Vector3(0, 1, 0); // Offset player slightly above the platform
            camerafollow.player = Instantiate(playerPrefab, spawnPosition, Quaternion.identity).transform;
        }
    }
}
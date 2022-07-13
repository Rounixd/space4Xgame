using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalaxyGeneration : MonoBehaviour
{
    [SerializeField] int maxX;
    [SerializeField] int maxY;
    [SerializeField] int amountOfStars;
    [SerializeField] float blackHolePercentage;
    [SerializeField] int overlap;
    [SerializeField] GameObject starPrefab;
    [SerializeField] GameObject galaxyContainer;
    [SerializeField] GameObject starsystemContainer;

    [SerializeField] List<Vector2> starsystemPositions = new List<Vector2>();

    int[] starTypeChances = new int[] { 35, 50 };

    bool hasTestPassed = true;

    enum starTypes {
        YELLOW_DWARF, RED_DWARF
    }

    public delegate void StarGenerated(GameObject go, StarSystem ss);
    public static StarGenerated starGenerated; 

    void Start()
    {
        int numOfBlackHoles = Mathf.RoundToInt(amountOfStars * blackHolePercentage);

        //without -1 it generates an additional star
        for(int i = 0; i <= amountOfStars -1; i++)
        {
            StarSystem star = null;

            //Generates star coordinates, which are stored in starsystemPositions. 
            if (hasTestPassed == true)
            GenerateStarCoordinates();
        
            // Calculate the middle point of the galaxy map and place both containers there. Place the camera there as well.
            Vector3 middlePoint = new Vector3(maxX / 2, maxY / 2, 0);
            galaxyContainer.transform.position = new Vector3(middlePoint.x, middlePoint.y, 15);
            Camera.main.transform.position = new Vector3(maxX / 2, maxY / 2, 15);

            //Generate physical instances. Set the Galaxy container as the parent of newly generated stars.
            GameObject physicalInstance = Instantiate(starPrefab, new Vector3(starsystemPositions[i].x, starsystemPositions[i].y, 0), Quaternion.identity);
            physicalInstance.transform.SetParent(galaxyContainer.transform);
            physicalInstance.transform.localPosition = new Vector3(physicalInstance.transform.localPosition.x, physicalInstance.transform.localPosition.y, 15);
            physicalInstance.name = "Star " + (i + 1);

            //Generate the startype. Black holes are generated differently, becasue there has to be a certain number of them on the map.
            if (i >= amountOfStars - numOfBlackHoles)
            {
                star = new BlackHole();        
            }
            else
            {
                switch (WeightedProbability.CalculateWeightedProbability(starTypeChances))
                {
                    case (int)starTypes.YELLOW_DWARF:
                        star = new YellowDwarf();
                        break;

                    case (int)starTypes.RED_DWARF:
                        star = new RedDwarf();
                        break;
                }
            }

            starGenerated?.Invoke(physicalInstance, star);
        }
        
    }

    void GenerateStarCoordinates()
    {
        //The script will try to genereate new coordinates if the newly generated ones happened to be inside of the overlap of another star.
        for (int failedAttempts = 0; failedAttempts < 1999; failedAttempts++)
        {

            int xPos = Random.Range(0, maxX);
            int yPos = Random.Range(0, maxY);
            

            for(int i = 0; i < starsystemPositions.Count; i++)
            {
                //Checks if the newly generated star is in the circle shaped area of 'overlap' radius. If it is, destroy it.
                float distanceOverlap = Vector3.Distance(new Vector2(xPos, yPos), starsystemPositions[i]);

                if(distanceOverlap < overlap)
                {
                    //No point in checking the overlap for other stars if one had failed. Break the loop and generate new coordinates.
                    hasTestPassed = false;
                    break;
                }
                else
                {
                    //If the coordinates are fine, continue the loop to check if they work with the coords of the other stars as well.
                    hasTestPassed = true;
                }
            }
            
            //Needed to populate first value in the starsystemPositions list. It won't work without it
            if (starsystemPositions.Count == 0)
                hasTestPassed = true;

            if (hasTestPassed == true)
            {
                starsystemPositions.Add(new Vector2(xPos, yPos));
                break;
            }
            
        }
       
    }
}

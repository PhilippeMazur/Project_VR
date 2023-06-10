using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArtifactCollector : MonoBehaviour
{
    // Start is called before the first frame update
    public Text uiText;
    int artifactsBroughtToAltar;

    public void Start()
    {
        artifactsBroughtToAltar = 0;
    }

    private float timer = 0f;
    private float interval = 1f;
    void Update()
    {
        //Verwijder alle code als we onze level hebben. Dit is placeholder code om de werking te tonen
        timer += Time.deltaTime;

        if (timer >= interval)
        {
            // Call your method here
            CollectArtifact();

            // Reset the timer
            timer = 0f;
        }
    }

    public void CollectArtifact()
    {
        if(artifactsBroughtToAltar < 5)
        {
            artifactsBroughtToAltar += 1;
            if (uiText != null)
            {
                uiText.text = $"{artifactsBroughtToAltar} / 5";
            }
        }
        
    }
}

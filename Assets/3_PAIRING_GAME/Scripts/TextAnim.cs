using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextAnim : MonoBehaviour
{
    //Declarations
    [SerializeField] TextMeshProUGUI welcomeText;
    public string[] stringArray;

    [SerializeField] float timeBtwnChars;
    [SerializeField] float timeBtwnWords;

    int i = 0;

    // Start is called before the first frame update
    void Start()
    {
        EndCheck();
    }

    public void EndCheck()
    {
        if (i < stringArray.Length - 1)
        {
            welcomeText.text = stringArray[i];
            StartCoroutine(TextVisible());
        }
    }

    private IEnumerator TextVisible()
    {
        welcomeText.ForceMeshUpdate();

        int TotalVisibleCharacters = welcomeText.textInfo.characterCount;
        int counter = 0;

        while(true)
        {
            int visibleCount = counter % (TotalVisibleCharacters + 1);
            welcomeText.maxVisibleCharacters = visibleCount;

            if (visibleCount >= TotalVisibleCharacters)
            {
                i += 1;
                //Invoke("EndCheck", timeBtwnWords);
                break;
            }

            counter += 1;
            yield return new WaitForSeconds(timeBtwnChars);
        }
    }
}

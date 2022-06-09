using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] private MemoryCard originalCard;
    [SerializeField] private Sprite[] images;
    [SerializeField] private TextMesh scoreLabel;
    
    private MemoryCard first;
    private MemoryCard second;
    private int score = 0;
    public bool CanReveal
    {
        get { return second == null; }
    }
    public void CardRevealed(MemoryCard card)
    {
        if (first == null)
        {
            first = card;
        }
        else
        {
            second = card;
            StartCoroutine(CheckMatch());
        }

    }
    private IEnumerator CheckMatch()
    {
        if (first.Id == second.Id)
        {
            score++;
            scoreLabel.text = "Score: " + score;
        }
        else
        {
            yield return new WaitForSeconds(1.0f);

            first.Unreveal();
            second.Unreveal();
        }
        first = null;
        second = null;
    }

    public int columns = 4;
    public int rows = 2;
    public float offsetX = 2f;
    public float offsetY = 2.5f;


    private void Start()
    {
        Vector3 startPos = originalCard.transform.position;

        int[] numbers = { 0, 0, 1, 1, 2, 2, 3, 3 };
        numbers = ShuffleArray(numbers);

        for(int c = 0; c < columns; c++)
        {
            for(int r = 0; r < rows; r++)
            {
                MemoryCard card;
                if( c == 0 && r == 0)
                {
                    card = originalCard;
                }
                else
                {
                    card = Instantiate(originalCard) as MemoryCard;
                }

                int index = r * columns + c;
                int id = numbers[index];
                card.SetCard(id, images[id]);

                float posX = (offsetX * c) + startPos.x;
                float posY = -(offsetY * r) + startPos.y;
                card.transform.position = new Vector3(posX, posY, startPos.z);
            }
        }    
    }
    private int[] ShuffleArray(int[] array)
    {
        int[] newArray = array.Clone() as int[];

        for(int i = 0; i < newArray.Length; i++)
        {
            int temp = newArray[i];
            int r = Random.Range(i, newArray.Length);
            newArray[i] = newArray[r];
            newArray[r] = temp;
        }
        return newArray;
    }
    public void Restart()
    {
        SceneManager.LoadScene("SampleScene");
    }

}

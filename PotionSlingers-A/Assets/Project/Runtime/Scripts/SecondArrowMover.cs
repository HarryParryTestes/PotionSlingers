using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class SecondArrowMover : MonoBehaviour
{

    public GameManager manager;
    // Start is called before the first frame update
    void Start()
    {
        transform.DOMove(new Vector2(transform.position.x + 50, transform.position.y), 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine).SetId(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Awake()
    {

    }

    public void checkArrow()
    {
        DOTween.Pause(1);
        if (manager.dialog.textBoxCounter == 5)
        {
            Debug.Log("Changed arrow position");
            transform.position = new Vector3(780f, 450f, 0);
            // transform.position = new Vector3(transform.position.x + 327, transform.position.y + 25, 0);
            transform.DOMove(new Vector2(transform.position.x + 50, transform.position.y), 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine).SetId(1);
        }
        if (manager.dialog.textBoxCounter == 10)
        {
            transform.position = new Vector3(1400f, 750f, 0);
            gameObject.SetActive(true);
            transform.DOMove(new Vector2(transform.position.x + 50, transform.position.y), 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine).SetId(1);
        }
    }
}

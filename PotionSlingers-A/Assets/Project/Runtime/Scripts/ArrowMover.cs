using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class ArrowMover : MonoBehaviour
{

    public GameManager manager;
    // Start is called before the first frame update
    void Start()
    {
        transform.DOMove(new Vector2(transform.position.x, transform.position.y - 50), 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine).SetId(0);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if(manager.marketSelected && manager.dialog.textBoxCounter == 10)
        {
            gameObject.SetActive(false);
        }
        */
    }

    void Awake()
    {
        
    }

    public void checkArrow()
    {
        DOTween.Pause(0);
        if (manager.dialog.textBoxCounter == 5)
        {
            Debug.Log("Changed arrow position");
            transform.position = new Vector3(780f, 450f, 0);
            // transform.position = new Vector3(transform.position.x + 327, transform.position.y + 25, 0);
            transform.DOMove(new Vector2(transform.position.x, transform.position.y - 50), 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine).SetId(0);
        }

        if(manager.marketSelected && manager.dialog.textBoxCounter == 10)
        {
            transform.position = new Vector3(1600f, 500f, 0);
            transform.DOMove(new Vector2(transform.position.x, transform.position.y - 50), 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine).SetId(0);
            return;
        }

        else if (manager.dialog.textBoxCounter == 10)
        {
            Debug.Log("Changed arrow position");
            transform.position = new Vector3(940f, 600f, 0);
            // transform.position = new Vector3(transform.position.x + 327, transform.position.y + 25, 0);
            transform.DOMove(new Vector2(transform.position.x, transform.position.y - 50), 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine).SetId(0);
        }

        if (manager.dialog.textBoxCounter == 14)
        {
            Debug.Log("Changed arrow position");
            transform.position = new Vector3(1775f, 300f, 0);
            // transform.position = new Vector3(transform.position.x + 327, transform.position.y + 25, 0);
            transform.DOMove(new Vector2(transform.position.x, transform.position.y - 50), 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine).SetId(0);
        }
    }
}

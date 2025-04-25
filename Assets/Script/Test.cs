using SB;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Test : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _sprRenderer = null;

    [SerializeField]
    private GamePresenter gamePresenter = null;

    private void Start()
    {
        //Time.timeScale = 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(0);
        }

        if (!Input.GetMouseButton(0))
            return;

        var mPos = Input.mousePosition;

        var pos = Camera.main.ScreenToWorldPoint(mPos);
        pos.z = 0;
        transform.position = pos;

        if (Input.GetMouseButtonDown(0))
        {
            gamePresenter.Touch(TouchPhase.Began, pos);
        }
        else
        {
            gamePresenter.Touch(TouchPhase.Moved, pos);
        }
    }
}

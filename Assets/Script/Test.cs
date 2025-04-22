using SB;
using UnityEngine;

public class Test : MonoBehaviour
{

    [SerializeField]
    private SpriteRenderer renderer = null;
    
    // Update is called once per frame
    void Update()
    {
        if (!Input.GetMouseButton(0))
            return;

        Hexagon hexagon = new Hexagon(Vector2.zero);
        
        var mPos = Input.mousePosition;

        var pos = Camera.main.ScreenToWorldPoint(mPos);
        pos.z = 0;
        transform.position = pos;

        Debug.Log($"isContains : {hexagon.IsContainsPosition(transform.position)}");
        renderer.color = hexagon.IsContainsPosition(transform.position) ? Color.green : Color.red;
    }
}

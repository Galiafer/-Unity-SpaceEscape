using AngryLab;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private const float LEFT_BOARD = -20f;

    private void Update()
    {
        if(gameObject.activeSelf)
            if(transform.position.x < LEFT_BOARD) SPManager.instance.DisablePoolObject(gameObject);
    }
}

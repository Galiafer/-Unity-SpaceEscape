using UnityEngine;

public class CirclesRotation : MonoBehaviour
{
    [SerializeField] private float _speed;

    #region Variables
    private bool _rotation = true;
    #endregion

    #region Unity Functions
    private void Update() => Rotate(_rotation);
    #endregion

    #region Rotation Functions
    private void Rotate(bool rotateLeft)
    {
        CheckRotation();

        transform.eulerAngles += new Vector3(0f, 0f, (rotateLeft) ? 1f : -1f) * _speed * Time.deltaTime;
    }

    private void CheckRotation()
    {
        #region Mobile
        if (Input.touchCount > 0)
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                if (_rotation == true)
                    _rotation = false;
                else
                    _rotation = true;
            }
        #endregion
        
        // Uncomment this part below for PC contorlling, and comment code above
        //#region PC
        //if (Input.GetMouseButtonDown(0))
        //{
        //    if (_rotation == true)
        //        _rotation = false;
        //    else
        //        _rotation = true;
        //}
        //#endregion
    }
    #endregion
}

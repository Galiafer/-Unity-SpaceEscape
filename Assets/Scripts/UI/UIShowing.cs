using System.Collections.Generic;
using UnityEngine;

public class UIShowing : MonoBehaviour
{
    [SerializeField] private GameObject _restartParentUI;

    private List<HidingUI> _hidingUIs;

    private void Start()
    {
        _hidingUIs = new List<HidingUI>(Resources.FindObjectsOfTypeAll<HidingUI>());

        Circle.OnPlayerDie += ShowUIElement;
    }

    private void ShowUIElement()
    {
        foreach (HidingUI hidingUI in _hidingUIs)
            hidingUI.gameObject.SetActive(true);

        SceneHandler.FreezeTime();
    }
}

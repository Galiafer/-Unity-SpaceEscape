using UnityEngine;
using UnityEngine.UI;

public class ShowMaxScore : MonoBehaviour
{
    [SerializeField] private Text _text;

    private void Start() => _text.text = $"Max Score: {PlayerPrefs.GetInt("PlayerScore")}";
}

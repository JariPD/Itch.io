using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypeText : MonoBehaviour
{
    public string WriteText;
    public int WriteSpeed = 30;

    void Start()
    {
        StartCoroutine(TypeDialog(WriteText));
    }

    public IEnumerator TypeDialog(string dialog)
    {
        TextMeshProUGUI textToType = GetComponent<TextMeshProUGUI>();
        foreach (var letter in dialog.ToCharArray())
        {
            textToType.text += letter;
            yield return new WaitForSeconds(1f / WriteSpeed);
        }
    }
}

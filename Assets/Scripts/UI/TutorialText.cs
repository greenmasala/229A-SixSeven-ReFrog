using TMPro;
using UnityEngine;

public class TutorialText : MonoBehaviour
{
    public GameObject TutorialTextbox;
    public TextMeshProUGUI TutorialTextInput;
    [TextAreaAttribute]
    public string Text;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        TutorialTextInput.text = Text;
        TutorialTextbox.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        TutorialTextbox.SetActive(false);
    }
}

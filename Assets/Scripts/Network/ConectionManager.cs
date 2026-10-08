using UnityEngine;
using TMPro;

public class ConectionManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField codeInput;
    
    public void Host()
    {
        BasicSpawner.Instance.HostGame();
    }

    public void Join()
    {
        string code = codeInput.text.Trim().ToUpper();
        BasicSpawner.Instance.JoinGame(code);
    }
}

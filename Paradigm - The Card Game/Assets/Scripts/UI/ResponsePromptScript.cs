using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResponsePromptScript : MonoBehaviour
{
    [SerializeField]
    private GameObject yesBtnObject;

    [SerializeField]
    private GameObject noBtnObject;

    private TMP_Text prmptText;
    private Button yeBtn;
    private Button noBtn;

    
    private void Awake()
    {
        prmptText = gameObject.GetComponent<TMP_Text>();
        yeBtn = yesBtnObject.GetComponent<Button>();
        noBtn = noBtnObject.GetComponent<Button>();
    }

    // Start is called before the first frame update
    void Start()
    {
        yeBtn.onClick.AddListener(YesTest);
        noBtn.onClick.AddListener(NoTest);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void YesTest()
    {
        print("You have more to add to the stack");
        print("Allow player to make a legal move");
        Destroy(gameObject);
    }

    private void NoTest()
    {
        print("Resolving Stack nothing left to add");
        Destroy(gameObject);
    }

    public void SetPromptText(string s)
    {
        prmptText.text = s + " Initate an Ability?";
    }
}

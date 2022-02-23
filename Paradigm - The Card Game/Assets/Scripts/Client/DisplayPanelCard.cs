using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayPanelCard : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private QueueCardTransferSO queueCardTransferSO;
    
    [SerializeField]
    private Sprite selectedBorderSprite;

    [SerializeField]
    private Image borderImageReference;

    [SerializeField]
    private Button cardButton;

    [SerializeField]
    private TMPro.TMP_Text cardName;

    [SerializeField]
    private TMPro.TMP_Text cardDesc;

    private CardSO cardData;
    private Sprite defaultBorderSprite;
    private Image cardArt;
    private bool isDefaultBorderActive = true;
    private bool canBeSelected = true;
    public delegate void HasBeenSelected(object sender, bool isSelected);
    public static event HasBeenSelected HasBeenSelectedEvent; //for multiplayer this cant be static

    private void Awake()
    {
        //cardData = queueCardTransferSO.GetQueueCard();
        cardButton = gameObject.GetComponentInChildren<Button>();
        defaultBorderSprite = borderImageReference.sprite;
        cardArt = gameObject.GetComponent<Image>();
        CardSelectionOverlayForNetwork.IsDoneChoosing += SetSelectionMode;

    }

    private void SetSelectionMode(object sender, bool isSelectingDisabled)
    {
        if(isSelectingDisabled)
        {
            canBeSelected = false;
        }
        else
        {
            canBeSelected = true;
        }
    }

    void Start()
    {
        if (cardButton != null)
        {
            cardButton.onClick.AddListener(TestButton);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TestButton()
    {
        if(canBeSelected)
        {
            if (isDefaultBorderActive)
            {
                borderImageReference.sprite = selectedBorderSprite;
                isDefaultBorderActive = false;
                HasBeenSelectedEvent.Invoke(this, true);

            }
            else
            {
                borderImageReference.sprite = defaultBorderSprite;
                isDefaultBorderActive = true;
                HasBeenSelectedEvent.Invoke(this, false);
            }
        }
        
    }
}

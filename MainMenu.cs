using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MainMenu : MonoBehaviour {
    public GameObject ItemSearchGame;//префаб заготовки 
    public GameObject content;
    private RectTransform rectTransfrom;
    private void Start()
    {
        for(int i = 0; i < 100; i++)
        {
            AddItemToContentPanel(i);
        }
    }
    public void SearchSessions()
    {

    }
    void AddItemToContentPanel(int index)
    {
        GameObject item = Instantiate(ItemSearchGame, content.transform);
        var iSG = item.GetComponent<ItemSeacrhGames>();
        item.gameObject.transform.localPosition = new Vector2(0, -75 - (100 * index));
        rectTransfrom = content.GetComponent<RectTransform>();

        Debug.Log(rectTransfrom.sizeDelta);  // получаем размер  типа Vector2

        rectTransfrom.sizeDelta = new Vector2(50, 50); // задаем размер   new Vectro2 (width, height)
        //content.GetComponent<RectTransform>().rect.Set(-342, 192, 684, (index + 1) * 100);
        iSG.text.text = index + "";
    }
}

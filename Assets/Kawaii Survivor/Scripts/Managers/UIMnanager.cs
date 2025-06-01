using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMnanager : MonoBehaviour, IGameStateListener
{
    [Header("Panels")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject waveTransitionPanel;
    [SerializeField] private GameObject shopPanel;

    private List<GameObject> panels = new List<GameObject>();
    
    private void Awake()
    {
        panels.AddRange(new GameObject[]
        {
            menuPanel,
            gamePanel,
            waveTransitionPanel,
            shopPanel
        });
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GmaeStateChangeCallback(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.MENU:
                ShowPanel(menuPanel);
                break;
            case GameState.GAME:
                ShowPanel(gamePanel);
                break;
            case GameState.WAVETRANSITION:
                ShowPanel(waveTransitionPanel);
                break;
            case GameState.SHOP:
                ShowPanel(shopPanel);
                break;
        }
    }
    
    private void ShowPanel(GameObject panel)
    {
        foreach (GameObject p in panels)
            p.SetActive(p == panel);
    }
}

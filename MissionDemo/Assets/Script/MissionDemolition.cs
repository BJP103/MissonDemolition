using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public enum GameMode
{
    idle,
    playing,
    levelEnd
}

public class MissionDemolition : MonoBehaviour
{
    static private MissionDemolition S;
    [Header("Inscribed")]
    public Text uitLevel;
    public Text uitSShots;
    public Vector3 castlePos;
    public GameObject[] castles;

    [Header("Dynamic")]
    public int level;
    public int levelMax;
    public int shotsTaken;
    public GameObject castle;
    public GameMode mode=GameMode.idle;
    public string showing = "Show Slingshot";

    // Start is called before the first frame update
    void Start()
    {
        S = this;
        level = 0;
        shotsTaken = 0;
        levelMax = castles.Length;
        StartLevel();

    }

    void StartLevel()
    {
        // Get rid of the old castle if one exists
        if (castle != null)
        {
            Destroy(castle);
        }
        //Desroy old projectiles if they exist (the method is not yet written)
        Projectile.DESTROY_PROJECTILES();
        castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlePos;

        Goal.goalMet = false;

        UpdateGUI();

        mode= GameMode.playing;
    }
    void UpdateGUI()
    {
        //Show the data in the GUITexts
        uitLevel.text = "Level" +(level + 1)+" of"+levelMax;
        uitSShots.text = "Shots Fired: " + shotsTaken;
    }
    private void Update()
    {
        UpdateGUI();
        // Check for level end
        if ((mode == GameMode.playing) && Goal.goalMet) {
            mode = GameMode.levelEnd;

            Invoke("NextLevel", 2f);
        }
    }
    void NextLevel()
    {
        level++;
        if (level == levelMax)
        {
            level = 0;
            shotsTaken = 0;

        }
        StartLevel();
    }
    static public void SHOT_FIRED()
    {
        S.shotsTaken++;
    }
    static public GameObject Get_CASTLE()
    {
        return S.castle;
    }
}

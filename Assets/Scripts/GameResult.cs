using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameResult : MonoBehaviour{
    public static GameResult gameResult;
    private readonly KeyCode[] keyCodes = {
        KeyCode.Return, KeyCode.Escape, KeyCode.Space, KeyCode.R, KeyCode.F5
    };
    [SerializeField] TextMeshPro[] GameResultTMP;
    [SerializeField] TextMeshPro[] GameResultScoreTmp;
    [SerializeField] SpriteRenderer[] GameResultRenderer;
    [SerializeField] SpriteRenderer[] GameClearResultSprite;
    private void Awake()
    {
        gameResult = this;
    }
    public void ResultSetting(){
        print("Show Result");

        Music music;
        music = MainSystem.NowOnMusic;

        MainSystem mainSystem;
        mainSystem = MainSystem.mainSystem;
        GameManager gameManager;
        gameManager = MainSystem.gameManager;
        SpriteManager spriteManager;
        spriteManager = MainSystem.spriteManager;

        int dif = mainSystem.difficultyNum;
        int score = GameManager.GamePlayScore;
        int scoreDif = score - MainSystem.NowOnMusic.playHighScore[dif];

        GameClearResultSprite[0].enabled = false;
        GameClearResultSprite[1].enabled = false;
        GameClearResultSprite[2].enabled = false;
        GameClearResultSprite[3].enabled = false;
        GameClearResultSprite[4].enabled = false;

        var colorWhite = new Color32(255,255,255,255);
        var colorGray = new Color32(255,255,255,100);
        for (int i = 0; i < 18; i++){
            GameResultScoreTmp[i].color = colorGray;
        }

        // Song Information ----------------------------------------------------------------
        GameResultTMP[00].text = music.MusicName;
        GameResultTMP[01].text = music.MusicArtist;
        // Difficulty       ----------------------------------------------------------------
        GameResultTMP[02].text = music.Difficulty[dif].ToString();
        GameResultTMP[03].text = MainSystem.spriteManager.getDifficultyText(music.Difficulty[dif]);
        // ScoreSetting     ----------------------------------------------------------------
        char[] scoreChar = (string.Format("{0:D9}", score)).ToCharArray();
        char[] scoreDifChar = (string.Format("{0:D9}", Mathf.Abs(scoreDif))).ToCharArray();
        for (int i = 0; i < 9; i++){
            GameResultScoreTmp[i].text = scoreChar[i].ToString();
            if (score >= Mathf.Pow(10, (8 - i))){
                GameResultScoreTmp[i].color = colorWhite;
            }
        }
        for (int i = 0; i < 9; i++){
            GameResultScoreTmp[i + 9].text = scoreDifChar[i].ToString();
            if (score >= Mathf.Pow(10, (8 - i))){
                GameResultScoreTmp[i + 9].color = colorWhite;
            }
        }
        if (scoreDif >= 0){
            GameResultScoreTmp[18].text = "+";
        }
        else{
            GameResultScoreTmp[18].text = "-";
        }
        // GuageSetting     ----------------------------------------------------------------
        GameResultTMP[04].text = string.Format("{0:F2}", GameManager.GamePlayClearRate);
        // JudgeSetting     ----------------------------------------------------------------
        print(GameManager.Record[1].ToString());
        GameResultTMP[05].text = GameManager.Record[1].ToString();
        GameResultTMP[06].text = (GameManager.Record[0] + GameManager.Record[2]).ToString();
        GameResultTMP[07].text = (GameManager.Rough[0] + GameManager.Rough[1]).ToString();
        GameResultTMP[08].text = (GameManager.Lost[0] + GameManager.Lost[1]).ToString();
        GameResultTMP[09].text = GameManager.MaxSustain.ToString();
        GameResultTMP[10].text = music.NoteCount[dif].ToString();
        // User Information ----------------------------------------------------------------
        GameResultTMP[11].text = "LV." + UserInfoManager.userLevel.ToString();
        GameResultTMP[12].text = UserInfoManager.userName;
        GameResultTMP[13].text = string.Format("{0:F2}", UserInfoManager.userLevelProgress);
        GameResultTMP[14].text = string.Format("{0:F2}", UserInfoManager.userSpecialProgress);
        GameResultTMP[15].text = string.Format("{0:F2}", UserInfoManager.userRating);


        // Song Information ----------------------------------------------------------------
        GameResultRenderer[00].sprite = music.sprJacket;
        // Difficulty       ----------------------------------------------------------------
        GameResultRenderer[01].sprite = spriteManager.getDifficultySprite(dif);
        // ScoreSetting     ----------------------------------------------------------------
        GameResultRenderer[02].sprite = spriteManager.getRankSprite(getScoreIndex(score));
        if (GameManager.isAllPerfect) 
        { 
            if (GameManager.Record[0] == 0 && GameManager.Record[2] == 0){
                GameClearResultSprite[0].enabled = true;    // Pure RecorD+
            }
            else{
                GameClearResultSprite[1].enabled = true;    // Pure Record
            }
        }
        else if (GameManager.isFullCombo) 
        { 
            GameClearResultSprite[2].enabled = true;        // Maximum Record
        }
        else if (GameManager.isHardGame){
            if (GameManager.GamePlayClearRate == 0){
                GameClearResultSprite[4].enabled = true;    // Record Recounce
            }
            else{
                GameClearResultSprite[2].enabled = true;    // exquisite Record
            }
        }
        else{
            if (GameManager.GamePlayClearRate >= 70.0f){
                GameClearResultSprite[3].enabled = true;    // Record Complete
            }
            else{
                GameClearResultSprite[4].enabled = true;    // Record Recounce
            }
        }
        
        // JudgeSetting     ----------------------------------------------------------------
        // User Information ----------------------------------------------------------------
        /*GameResultRenderer[04].sprite 
            = spriteManager.getCharacterIconSprite(userInfoManager.characterIndex);
        GameResultRenderer[05].sprite 
            = spriteManager.getCharacterIconSprite(userInfoManager.characterIndex);  */

        StartCoroutine(IChangeScene());
    }
    private IEnumerator IChangeScene(){
        yield return new WaitForSeconds(1.0f);
        while(true){
            print("looping");
            if(Input.GetKeyDown(keyCodes[0]) 
            || Input.GetKeyDown(keyCodes[1]) 
            || Input.GetKeyDown(keyCodes[2])){
                // 음악 선택창으로 넘어감
                MainSystem.mainSystem.RunISelectScene();
                break;
            }
            if(Input.GetKeyDown(keyCodes[3])
            || Input.GetKeyDown(keyCodes[4])){
                // 게임 재시작
                break;
            }
            yield return null;
        }
        yield return new WaitForSeconds(1.0f);
        try {
            SceneManager.LoadScene(MainSystem.gameType);
        }
        catch {
            SceneManager.LoadScene("Select");
        }
        
    }
    private int getScoreIndex(int score)
    {
        if (score >= 99000000) { return 00; }               // S+
        else if (score >= 98000000) { return 01; }          // S
        else if (score >= 97000000) { return 02; }          // AA+
        else if (score >= 95000000) { return 03; }          // AA
        else if (score >= 92500000) { return 04; }          // A+
        else if (score >= 90000000) { return 05; }          // A
        else if (score >= 85000000) { return 06; }          // B
        else if (score >= 80000000) { return 07; }          // C
        else { return 08; }                                 // D
    }
}

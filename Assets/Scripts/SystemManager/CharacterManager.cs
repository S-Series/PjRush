using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CharacterManager : MonoBehaviour{
    private static ICharacter[] characters;
    public static ICharacter SelectedCharacter;
    private void Awake() {
        characters = new ICharacter[]{
            new Character1(),
            new Character2()
        };
        setCharacter(0);
    }
    public static void setCharacter(int index){
        SelectedCharacter = characters[index];
    }
    public static void UseSkill(){
        SelectedCharacter.skill();
    }
}
public interface ICharacter{
    void skill();
}
class Character1 : ICharacter{
    public string characterName = "";
    public float[] abilities = {1.0f, 1.0f, 1.0f, 1.0f};
    public float[] abilitiesGrowth = {1.0f, 1.0f, 1.0f, 1.0f};
    public void skill()
    {
        
    }
}

class Character2 : ICharacter{
        public string characterName = "";
    public float[] abilities = {1.0f, 1.0f, 1.0f, 1.0f};
    public float[] abilitiesGrowth = {1.0f, 1.0f, 1.0f, 1.0f};
    public void skill()
    {
        
    }
}
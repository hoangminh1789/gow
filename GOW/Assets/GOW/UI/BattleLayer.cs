using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GOW
{

    public class BattleLayer : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _txtTime;
        [SerializeField] TextMeshProUGUI _txtCharacterCount;
        [SerializeField] TextMeshProUGUI _txtPlayerHP;
        [SerializeField] Transform _popupLose = null;
        
        Battle _battle = null;
        
        void Awake()
        {
            
        }

        void Start()
        {
            _battle = Battle.Instance;
            
            _battle.OnCharacterChanged.AddListener( OnCharacter_Chaged  );
            _battle.OnPlayerHPChanged.AddListener(  OnPlayerHp_Changed  );
            _battle.OnTimeChanged.AddListener(      OnTime_Changed      );
            _battle.OnLose.AddListener(             OnLose              );
        }

        void OnCharacter_Chaged()
        {
            this.SetCharCount(_battle.AllCharacters.Count);
        }

        void OnPlayerHp_Changed(int hp, int maxHp)
        {
            this.SetPlayerHp(hp, maxHp);
        }
        
        void OnTime_Changed(int time)
        {
            _txtTime.text = "Time: " + time;
        }

        public void SetCharCount(int count)
        {
            _txtCharacterCount.text = "Char Count: " + count;
        }
        
        public void SetPlayerHp(int hp, int maxHp)
        {
            _txtPlayerHP.text = "PlayerHP: " + hp + "/" + maxHp;
        }
        
        public void OnLose()
        {
            _popupLose.SetActive(true);
        }
        
        public void OnClick_ClosePopup()
        {
            SceneManager.LoadScene("GOW/_Scenes/Main");
            _popupLose.SetActive(false);
        }

        public void OnClick_AddMonster()
        {
            _battle.CreateEnemy(30);
        }
    }
}

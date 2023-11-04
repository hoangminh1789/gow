using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GOW
{

    public class BattleLayer : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _txtTime;
        [SerializeField] TextMeshProUGUI _txtCharacterCount;

        Battle _battle = null;
        
        void Awake()
        {
            
        }

        void Start()
        {
            _battle = Battle.Instance;
            
            _battle.OnCharacterChanged.AddListener( OnCharacter_Chaged  );
            _battle.OnTimeChanged.AddListener(      OnTime_Changed      );
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnCharacter_Chaged()
        {
            _txtCharacterCount.text = "Char: " + _battle.AllCharacters.Count;
        }

        void OnTime_Changed(int time)
        {
            _txtTime.text = "Time: " + time;
        }
    }
}

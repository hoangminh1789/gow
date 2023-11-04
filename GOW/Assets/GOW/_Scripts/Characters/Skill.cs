using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOW
{
    public class Skill : MonoBehaviour
    {
        [SerializeField] List<SkillModel> _manualSkillModels = new List<SkillModel>();
        [SerializeField] List<SkillModel> _skillModels = new List<SkillModel>();

        int _index = 0;
        
        void Awake()
        {
            ThisChar = GetComponent<Character>();
        }
        
        void Start()
        {

        }

        public Character ThisChar { get; private set; } = null;

        public SkillModel AutoSkillModel
        {
            get
            {
                for (int i = 0, n = _skillModels.Count; i < n; ++i)
                {
                    int idx = (_index + i) % n;
                    if (_skillModels[idx].IsReady)
                    {
                        _index = idx;
                        return _skillModels[idx];
                    }
                }

                _index = 0;
                return _skillModels[_index];
            }
        }

        public SkillModel ManualSkillModel => _manualSkillModels.Count > 0 ? _manualSkillModels[0] : null;

        public void UseSkill(SkillModel model)
        {
            _index++;
            model.Use();
        }
        
        void Update()
        {
            for (int i = 0, n = _skillModels.Count; i < n; ++i)
            {
                _skillModels[i].Tick(Time.deltaTime);
            }
        }
    }
}
